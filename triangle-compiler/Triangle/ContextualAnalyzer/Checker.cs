
using System.Collections.Generic;
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    public class Checker : IVisitor
    {
        private IdentificationTable identificationTable;
        private readonly ErrorReporter errorReporter;

        private static readonly SourcePosition DUMMYPOS = new();
        private static readonly Identifier DUMMYIDENTIFIER = new("", DUMMYPOS);

        public Checker(ErrorReporter errorReporter)
        {
            this.errorReporter = errorReporter;

            identificationTable = new IdentificationTable();
            EstablishStdEnvironment();
        }

        /**
         * Point of entrance for checking.
         * Every visit in Checker will change the ast's node types
         * and identifiers accordingly
         */
        public void Check(Program ast)
        {
            ast.Visit(this, null);
        }

        public object VisitProgram(Program ast, object o)
        {
            ast.Command.Visit(this, null);
            return null;
        }

        #region Auxiliar Methods

        /**
         * Reports that the identifier or operator used at a leaf of the AST
         */
        private void ReportUndeclared(Terminal leaf)
        {
            errorReporter.ReportError("\"%\" is not declared", leaf.Spelling, leaf.Position);
        }

        /**
         * This function will receive an Identifier, and then it will try and find its type
         * If it doesn't find its type, then it will return an error
         */
        private static TypeDenoter CheckFieldIdentifier(FieldTypeDenoter ast, Identifier identifier)
        {
            if (ast is MultipleFieldTypeDenoter multipleDenoter)
            {
                if (multipleDenoter.Identifier.Spelling.Equals(identifier.Spelling))
                {
                    identifier.Declaration = ast;
                    return multipleDenoter.TypeDenoter;
                }
                return CheckFieldIdentifier(multipleDenoter.FieldTypeDenoter, identifier);
            }
            if (ast is SingleFieldTypeDenoter singleDenoter)
            {
                if (singleDenoter.Identifier.Spelling.Equals(identifier.Spelling))
                {
                    identifier.Type = ast;
                    return singleDenoter.TypeDenoter;
                }
            }
            return StdEnvironment.errorType;
        }

        /**
         * Will be called in the moment a new identifier has been declared, and then
         * this will remove and then visit any pending calls in the current identifier
         * level. Note that some variables may have been declared between the pendingCall
         * being made and the identifier being declared, thus the revert and change of the
         * identification table
         */
        private void VisitPendingCalls(Identifier identifier)
        {
            List<PendingCall> pendingCalls = identificationTable.CheckPendingCalls(identifier);

            //When I go through each pending call, the identification will be changed, I need a way to revert it
            IdentificationTable currentIdentificationTable = identificationTable;

            foreach (PendingCall pendingCall in pendingCalls)
            {
                //I need to get the declaration for the pending call
                Declaration procFuncDeclaration = (Declaration)pendingCall.GetProcFuncIdentifier().Visit(this, null);

                identificationTable = pendingCall.CallContextIdentificationTable; //Sets the Id Table as how it was in the moment of the call

                pendingCall.VisitPendingCall(this, procFuncDeclaration);//And then visit each of them

                identificationTable = currentIdentificationTable;
            }
        }

        #endregion

        #region Standard Environment

        /// <summary>
        /// Creates a small AST to represent a "declaration" of a standard
        /// type and enters it in the identification table for standard use
        /// </summary>
        /// <param name="id">The identifier "name" of the type</param>
        /// <param name="typeDenoter">The type bound to be bound to the identifier</param>
        /// <returns>The bound type denoter for the new type</returns>
        private TypeDeclaration DeclareStdType(string id, TypeDenoter typeDenoter)
        {
            TypeDeclaration binding = new(new Identifier(id, DUMMYPOS), typeDenoter, DUMMYPOS);
            identificationTable.Enter(id, binding);
            return binding;
        }

        // Creates a small AST to represent the "declaration" of a standard
        // constant, and enters it in the identification table.
        private ConstDeclaration DeclareStdConst(string id, TypeDenoter constType)
        {
            // constExpr used only as a placeholder for constType
            IntegerExpression constExpr = new(null, DUMMYPOS);
            constExpr.Type = constType;

            ConstDeclaration binding = new ConstDeclaration(new Identifier(id, DUMMYPOS),
                                                                constExpr,
                                                                DUMMYPOS);
            identificationTable.Enter(id, binding);
            return binding;
        }

        // Creates a small AST to represent the "declaration" of a standard
        // procedure, and enters it in the identification table.
        private ProcDeclaration DeclareStdProc(string id, FormalParameterSequence fps)
        {

            ProcDeclaration binding;

            binding = new ProcDeclaration(new Identifier(id, DUMMYPOS), fps,
                    new EmptyCommand(DUMMYPOS), DUMMYPOS);
            identificationTable.Enter(id, binding);
            return binding;
        }

        // Creates a small AST to represent the "declaration" of a standard
        // function, and enters it in the identification table.
        private FuncDeclaration DeclareStdFunc(string id, FormalParameterSequence fps,
                TypeDenoter resultType)
        {

            FuncDeclaration binding;

            binding = new FuncDeclaration(new Identifier(id, DUMMYPOS), fps, resultType,
                    new EmptyExpression(DUMMYPOS), DUMMYPOS);
            identificationTable.Enter(id, binding);
            return binding;
        }

        // Creates a small AST to represent the "declaration" of a
        // unary operator, and enters it in the identification table.
        // This "declaration" summarises the operator's type info.
        private UnaryOperatorDeclaration DeclareStdUnaryOp(string op, TypeDenoter argType, TypeDenoter resultType)
        {

            UnaryOperatorDeclaration binding;

            binding = new UnaryOperatorDeclaration(new Operator(op, DUMMYPOS),
                    argType, resultType, DUMMYPOS);
            identificationTable.Enter(op, binding);
            return binding;
        }

        // Creates a small AST to represent the "declaration" of a
        // binary operator, and enters it in the identification table.
        // This "declaration" summarises the operator's type info.
        private BinaryOperatorDeclaration DeclareStdBinaryOp(string op, TypeDenoter arg1Type, TypeDenoter arg2type, TypeDenoter resultType)
        {

            BinaryOperatorDeclaration binding;

            binding = new BinaryOperatorDeclaration(new Operator(op, DUMMYPOS),
                    arg1Type, arg2type, resultType, DUMMYPOS);
            identificationTable.Enter(op, binding);
            return binding;
        }

        // Creates small ASTs to represent the standard types.
        // Creates small ASTs to represent "declarations" of standard types,
        // constants, procedures, functions, and operators.
        // Enters these "declarations" in the identification table.

        private void EstablishStdEnvironment()
        {

            // idTable.startIdentification();
            StdEnvironment.booleanType = new BoolTypeDenoter(DUMMYPOS);
            StdEnvironment.integerType = new IntTypeDenoter(DUMMYPOS);
            StdEnvironment.charType = new CharTypeDenoter(DUMMYPOS);
            StdEnvironment.anyType = new AnyTypeDenoter(DUMMYPOS);
            StdEnvironment.errorType = new ErrorTypeDenoter(DUMMYPOS);

            StdEnvironment.booleanDecl = DeclareStdType("Boolean", StdEnvironment.booleanType);
            StdEnvironment.falseDecl = DeclareStdConst("false", StdEnvironment.booleanType);
            StdEnvironment.trueDecl = DeclareStdConst("true", StdEnvironment.booleanType);
            StdEnvironment.notDecl = DeclareStdUnaryOp("\\", StdEnvironment.booleanType, StdEnvironment.booleanType);
            StdEnvironment.negDecl = DeclareStdUnaryOp("-", StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.andDecl = DeclareStdBinaryOp("/\\", StdEnvironment.booleanType, StdEnvironment.booleanType, StdEnvironment.booleanType);
            StdEnvironment.orDecl = DeclareStdBinaryOp("\\/", StdEnvironment.booleanType, StdEnvironment.booleanType, StdEnvironment.booleanType);

            StdEnvironment.integerDecl = DeclareStdType("Integer", StdEnvironment.integerType);
            StdEnvironment.maxintDecl = DeclareStdConst("maxint", StdEnvironment.integerType);
            StdEnvironment.addDecl = DeclareStdBinaryOp("+", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.subtractDecl = DeclareStdBinaryOp("-", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.multiplyDecl = DeclareStdBinaryOp("*", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.divideDecl = DeclareStdBinaryOp("/", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.moduloDecl = DeclareStdBinaryOp("//", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.integerType);
            StdEnvironment.lessDecl = DeclareStdBinaryOp("<", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.booleanType);
            StdEnvironment.notgreaterDecl = DeclareStdBinaryOp("<=", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.booleanType);
            StdEnvironment.greaterDecl = DeclareStdBinaryOp(">", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.booleanType);
            StdEnvironment.notlessDecl = DeclareStdBinaryOp(">=", StdEnvironment.integerType, StdEnvironment.integerType, StdEnvironment.booleanType);

            StdEnvironment.charDecl = DeclareStdType("Char", StdEnvironment.charType);
            StdEnvironment.chrDecl = DeclareStdFunc("chr", new SingleFormalParameterSequence(
                    new ConstFormalParameter(DUMMYIDENTIFIER, StdEnvironment.integerType, DUMMYPOS), DUMMYPOS), StdEnvironment.charType);
            StdEnvironment.ordDecl = DeclareStdFunc("ord", new SingleFormalParameterSequence(
                    new ConstFormalParameter(DUMMYIDENTIFIER, StdEnvironment.charType, DUMMYPOS), DUMMYPOS), StdEnvironment.integerType);
            StdEnvironment.eofDecl = DeclareStdFunc("eof", new EmptyFormalParameterSequence(DUMMYPOS), StdEnvironment.booleanType);
            StdEnvironment.eolDecl = DeclareStdFunc("eol", new EmptyFormalParameterSequence(DUMMYPOS), StdEnvironment.booleanType);
            StdEnvironment.getDecl = DeclareStdProc("get", new SingleFormalParameterSequence(
                    new VarFormalParameter(DUMMYIDENTIFIER, StdEnvironment.charType, DUMMYPOS), DUMMYPOS));
            StdEnvironment.putDecl = DeclareStdProc("put", new SingleFormalParameterSequence(
                    new ConstFormalParameter(DUMMYIDENTIFIER, StdEnvironment.charType, DUMMYPOS), DUMMYPOS));
            StdEnvironment.getintDecl = DeclareStdProc("getint", new SingleFormalParameterSequence(
                    new VarFormalParameter(DUMMYIDENTIFIER, StdEnvironment.integerType, DUMMYPOS), DUMMYPOS));
            StdEnvironment.putintDecl = DeclareStdProc("putint", new SingleFormalParameterSequence(
                    new ConstFormalParameter(DUMMYIDENTIFIER, StdEnvironment.integerType, DUMMYPOS), DUMMYPOS));
            StdEnvironment.geteolDecl = DeclareStdProc("geteol", new EmptyFormalParameterSequence(DUMMYPOS));
            StdEnvironment.puteolDecl = DeclareStdProc("puteol", new EmptyFormalParameterSequence(DUMMYPOS));
            StdEnvironment.equalDecl = DeclareStdBinaryOp("=", StdEnvironment.anyType, StdEnvironment.anyType, StdEnvironment.booleanType);
            StdEnvironment.unequalDecl = DeclareStdBinaryOp("\\=", StdEnvironment.anyType, StdEnvironment.anyType, StdEnvironment.booleanType);
        }

        #endregion

        #region Commands

        //These always are to return null and not use the given object.
        public object VisitAssignCommand(AssignCommand ast, object o)
        {
            TypeDenoter variableType = (TypeDenoter)ast.Variable.Visit(this, null);
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!ast.Variable.IsVariable)
            {
                errorReporter.ReportError("LHS of assignment is not a variable", "", ast.Variable.Position);
            }
            if (!expressionType.Equals(variableType))
            {
                errorReporter.ReportError("assignment incompatibility", "", ast.Position);
            }
            return null;
        }

        public object VisitCallCommand(CallCommand ast, object o)
        {
            Declaration binding = (Declaration) o ?? (Declaration)ast.Identifier.Visit(this, null);

            if (binding == null)
            {
                //Object referred by the id wasn't found, only allowed on recursive
                if (identificationTable.RecursiveLevel > 0)
                {
                    //Recursive is set and thus I add a new pending call for later binding
                    identificationTable.AddPendingCall(new PendingCallCommand(new IdentificationTable(identificationTable),ast));
                }
                else
                {
                    ReportUndeclared(ast.Identifier);
                }
            }
            else if (binding is ProcDeclaration declaration)
            {
                ast.ActualParameterSequence.Visit(this, declaration.FormalParameterSequence);
            }
            else if (binding is ProcFormalParameter parameter)
            {
                ast.ActualParameterSequence.Visit(this, parameter.FormalParameterSequence);
            }
            else
            {
                errorReporter.ReportError("\"%\" is not a procedure identifier", ast.Identifier.Spelling, ast.Identifier.Position);
            }

            return null;
        }
        
        public object VisitEmptyCommand(EmptyCommand ast, object o)
        {
            return null;
        }
        
        public object VisitIfCommand(IfCommand ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!expressionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.Expression.Position);
            }
            ast.Command1.Visit(this, null);
            ast.Command2.Visit(this, null);
            return null;
        }
        
        public object VisitLetCommand(LetCommand ast, object o)
        {
            identificationTable.OpenScope();
            ast.Declaration.Visit(this, null);
            ast.Command.Visit(this, null);
            identificationTable.CloseScope();
            return null;
        }
        
        public object VisitSequentialCommand(SequentialCommand ast, object o)
        {
            ast.Command1.Visit(this, null);
            ast.Command2.Visit(this, null);
            return null;
        }
        
        public object VisitWhileLoopCommand(WhileLoopCommand ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter) ast.Expression.Visit(this, null);
            if (!expressionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.Expression.Position);
            }
            ast.Command.Visit(this, null);
            return null;
        }
        
        public object VisitDoWhileLoopCommand(DoWhileLoopCommand ast, object o) {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!expressionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.Expression.Position);
            }
            ast.Command.Visit(this, null);
            return null;
        }
        
        public object VisitUntilLoopCommand(UntilLoopCommand ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!expressionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.Expression.Position);
            }
            ast.Command.Visit(this, null);
            return null;
        }
        
        public object VisitDoUntilLoopCommand(DoUntilLoopCommand ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!expressionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.Expression.Position);
            }
            ast.Command.Visit(this, null);
            return null;
        }

        public object VisitForLoopCommand(ForLoopCommand ast, object o)
        {
            TypeDenoter initialExpressionType = (TypeDenoter)ast.InitialDeclaration.Expression.Visit(this, null);
            TypeDenoter haltExpressionType = (TypeDenoter)ast.HaltingExpression.Visit(this, null);
            if (!initialExpressionType.Equals(StdEnvironment.integerType))
            {
                errorReporter.ReportError("Integer expression expected here", "", ast.InitialDeclaration.Expression.Position);
            }
            if (!haltExpressionType.Equals(StdEnvironment.integerType))
            {
                errorReporter.ReportError("Integer expression expected here", "", ast.HaltingExpression.Position);
            }
            identificationTable.OpenScope();
            identificationTable.Enter(ast.InitialDeclaration.Identifier.Spelling, ast.InitialDeclaration);
            ast.Command.Visit(this, null);
            identificationTable.CloseScope();
            return null;
        }

        #endregion

        #region Expressions
        //These are to always return the TypeDenoter of the type of the expression
        //as well as not use the given object

        //Their purpose is to define the type in these expressions, then mark their
        //ASTs.Type, which will be returned
        public object VisitArrayExpression(ArrayExpression ast, object o)
        {
            TypeDenoter elementType = (TypeDenoter)ast.ArrayAggregate.Visit(this, null);
            IntegerLiteral integer = new(System.Convert.ToString(ast.ArrayAggregate.ElementCount), ast.Position);
            ast.Type = new ArrayTypeDenoter(integer, elementType, ast.Position);
            return ast.Type;
        }

        public object VisitBinaryExpression(BinaryExpression ast, object o)
        {
            TypeDenoter expression1Type = (TypeDenoter)ast.Expression1.Visit(this, null);
            TypeDenoter expression2Type = (TypeDenoter)ast.Expression2.Visit(this, null);

            Declaration operatorBinding = (Declaration)ast.Operator.Visit(this, null);
            if (operatorBinding == null)
            {
                ReportUndeclared(ast.Operator);
                ast.Type = StdEnvironment.errorType;
            }
            else
            {
                if (operatorBinding is BinaryOperatorDeclaration binaryOperator)
                {
                    if (binaryOperator.Argument1Type == StdEnvironment.anyType)
                    {
                        //Operator is "=" or "/="
                        if (!expression1Type.Equals(expression2Type))
                        {
                            errorReporter.ReportError("incompatible argument types for \"%\"",
                            ast.Operator.Spelling, ast.Position);
                        }
                    }
                    else if (!expression1Type.Equals(binaryOperator.Argument1Type))
                    {
                        errorReporter.ReportError("wrong argument type for \"%\"",
                        ast.Operator.Spelling, ast.Expression1.Position);
                    }
                    else if (!expression2Type.Equals(binaryOperator.Argument2Type))
                    {
                        errorReporter.ReportError("wrong argument type for \"%\"",
                        ast.Operator.Spelling, ast.Expression2.Position);
                    }
                    ast.Type = binaryOperator.ResultType;
                }
                else
                {
                    errorReporter.ReportError("\"%\" is not a binary operator",
                            ast.Operator.Spelling, ast.Operator.Position);
                    ast.Type = StdEnvironment.errorType;
                }
            }
            return ast.Type;
        }

        public object VisitCallExpression(CallExpression ast, object o)
        {
            //Bind it to either the parameter, or by searching it
            Declaration binding = (Declaration)o ?? (Declaration) ast.Identifier.Visit(this, null);

            if (binding == null)
            {
                if (identificationTable.RecursiveLevel > 0)
                {
                    identificationTable.AddPendingCall(new PendingCallExpression(new IdentificationTable(identificationTable), ast));
                }
                else
                {
                    ReportUndeclared(ast.Identifier);
                    ast.Type = StdEnvironment.errorType;
                }
            }
            else if (binding is FuncDeclaration func)
            {
                ast.ActualParameterSequence.Visit(this, func.FormalParameterSequence);
                ast.Type = func.Type;

                //I filter the expressions that are the same as the ast, whose types
                //don't match, and issue an error for each of them.
                identificationTable.FutureCallExpressions.FindAll(futureCallExpression => 
                        futureCallExpression.Expression == ast && 
                        !futureCallExpression.TypeDenoterToCheck.Equals(ast.Type)
                    ).ForEach(wrongTypeFunction => errorReporter.ReportError("body of function \"%\" has wrong type", ast.Identifier.Spelling, ast.Position)
                );
            }
            else if (binding is FuncFormalParameter funcFormalParameter)
            {
                ast.ActualParameterSequence.Visit(this, funcFormalParameter.FormalParameterSequence);
                ast.Type = funcFormalParameter.Type;
            }
            else
            {
                errorReporter.ReportError("\"%\" is not a function identifier", ast.Identifier.Spelling, ast.Identifier.Position);
                ast.Type = StdEnvironment.errorType;
            }

            return ast.Type;
        }

        public object VisitCharacterExpression(CharacterExpression ast, object o)
        {

        }

        public object VisitEmptyExpression(EmptyExpression ast, object o)
        {

        }

        public object VisitIfExpression(IfExpression ast, object o)
        {

        }

        public object VisitIntegerExpression(IntegerExpression ast, object o)
        {

        }

        public object VisitLetExpression(LetExpression ast, object o)
        {

        }

        public object VisitRecordExpression(RecordExpression ast, object o)
        {

        }

        public object VisitUnaryExpression(UnaryExpression ast, object o)
        {

        }

        public object VisitVnameExpression(VnameExpression ast, object o)
        {

        }

        #endregion
    }
}
