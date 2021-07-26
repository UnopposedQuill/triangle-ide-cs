
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

            Declaration operatorBinding = (Declaration)ast.Operator.Visit(this, ast.GetType());
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
            ast.Type = StdEnvironment.charType;
            return ast.Type;
        }

        public object VisitEmptyExpression(EmptyExpression ast, object o)
        {
            return null;
        }

        public object VisitIfExpression(IfExpression ast, object o)
        {
            TypeDenoter conditionType = (TypeDenoter)ast.ConditionExpression.Visit(this, null);
            if (!conditionType.Equals(StdEnvironment.booleanType))
            {
                errorReporter.ReportError("Boolean expression expected here", "", ast.ConditionExpression.Position);
            }
            TypeDenoter expression1Type = (TypeDenoter)ast.Expression1.Visit(this, null);
            TypeDenoter expression2Type = (TypeDenoter)ast.Expression2.Visit(this, null);
            if (!expression1Type.Equals(expression2Type))
            {
                errorReporter.ReportError("incompatible limbs in if expression", "", ast.Position);
            }
            ast.Type = expression2Type;
            return ast.Type;
        }

        public object VisitIntegerExpression(IntegerExpression ast, object o)
        {
            ast.Type = StdEnvironment.integerType;
            return null;
        }

        public object VisitLetExpression(LetExpression ast, object o)
        {
            identificationTable.OpenScope();
            ast.Declaration.Visit(this, null);
            ast.Type = (TypeDenoter)ast.Expression.Visit(this, null);
            return ast.Type;
        }

        public object VisitRecordExpression(RecordExpression ast, object o)
        {
            FieldTypeDenoter fieldType = (FieldTypeDenoter)ast.RecordAggregate.Visit(this, null);
            ast.Type = new RecordTypeDenoter(fieldType, ast.Position);
            return ast.Type;
        }

        public object VisitUnaryExpression(UnaryExpression ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            Declaration binding = (Declaration)ast.Operator.Visit(this, ast.GetType());
            if (binding == null)
            {
                ReportUndeclared(ast.Operator);
                ast.Type = StdEnvironment.errorType;
            }
            else if (binding is UnaryOperatorDeclaration unaryOperator)
            {
                if (!expressionType.Equals(unaryOperator.ArgumentTypeDenoter))
                {
                    errorReporter.ReportError("wrong argument type for \"%\"", ast.Operator.Spelling, ast.Operator.Position);
                }
                ast.Type = unaryOperator.ResultTypeDenoter;
            }
            else
            {
                errorReporter.ReportError("\"%\" is not an unary operator", ast.Operator.Spelling, ast.Operator.Position);
                ast.Type = StdEnvironment.errorType;
            }
            return ast.Type;
        }

        public object VisitVnameExpression(VnameExpression ast, object o)
        {
            ast.Type = (TypeDenoter)ast.Vname.Visit(this, null);
            return ast.Type;
        }

        #endregion

        #region Declarations
        //Always return null. They don't use the given object.
        //These will make sure the component is not redefining things.
        
        //@TODO: Implement
        public object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, object o)
        {
            return null;//For now, every declaration is correct
        }

        public object VisitConstDeclaration(ConstDeclaration ast, object o)
        {
            _ = ast.Expression.Visit(this, null);//Develop the type of the Declaration
            identificationTable.Enter(ast.Identifier.Spelling, ast);//Add it as a new identifier
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitFuncDeclaration(FuncDeclaration ast, object o)
        {
            ast.Type = (TypeDenoter)ast.Expression.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast); //allows recursion
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared", ast.Identifier.Spelling, ast.Position);
            }

            identificationTable.OpenScope();
            ast.FormalParameterSequence.Visit(this, null);
            VisitPendingCalls(ast.Identifier);

            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            identificationTable.CloseScope();

            if (expressionType == null)
            {
                //It is a future call, thus we don't know its type yet
                identificationTable.AddFutureCallExpression(new FutureCallExpression(ast.Type, ast.Expression));
            }
            else if (!ast.Type.Equals(expressionType))
            {
                errorReporter.ReportError("body of function \"%\" has wrong type", ast.Identifier.Spelling, ast.Expression.Position);
            }
            return null;
        }

        public object VisitProcDeclaration(ProcDeclaration ast, object o)
        {
            identificationTable.Enter(ast.Identifier.Spelling, ast); //allows recursion
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared", ast.Identifier.Spelling, ast.Position);
            }

            identificationTable.OpenScope();
            ast.FormalParameterSequence.Visit(this, null);
            VisitPendingCalls(ast.Identifier);

            ast.Command.Visit(this, null);
            identificationTable.CloseScope();
            return null;
        }

        public object VisitSequentialDeclaration(SequentialDeclaration ast, object o)
        {
            ast.Declaration1.Visit(this, null);
            ast.Declaration2.Visit(this, null);
            return null;
        }

        public object VisitTypeDeclaration(TypeDeclaration ast, object o)
        {
            ast.TypeDenoter = (TypeDenoter)ast.TypeDenoter.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared",ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        //@TODO: Implement
        public object VisitUnaryOperatorDeclaration(UnaryOperatorDeclaration ast, object o)
        {
            return null;//For now, every declaration is correct
        }

        public object VisitVarDeclaration(VarDeclaration ast, object o)
        {
            ast.TypeDenoter = (TypeDenoter)ast.TypeDenoter.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitVarDeclarationInitialized(VarDeclarationInitialized ast, object o)
        {
            ast.TypeDenoter = (TypeDenoter)ast.Expression.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("identifier \"%\" already declared", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitRecursiveDeclaration(RecursiveDeclaration ast, object o)
        {
            identificationTable.OpenRecursiveScope();
            ast.Declaration.Visit(this, null);
            identificationTable.CloseRecursiveScope();

            
            if (identificationTable.RecursiveLevel > 0)
            {
                //We are out of every recursive level but there are still some undefined calls
                identificationTable.PendingCalls.ForEach(pendingCall =>
                {
                    //Make an error for each of them
                    errorReporter.ReportError("\"%\" is not a procedure or function identifier", pendingCall.GetProcFuncIdentifier().Spelling, pendingCall.GetProcFuncIdentifier().Position);
                });
                //Reset the list, so the compiler can keep going on checking errors
                identificationTable.PendingCalls = new List<PendingCall>();
            }
            return null;
        }

        public object VisitLocalDeclaration(LocalDeclaration ast, object o)
        {
            identificationTable.OpenScope();
            ast.Declaration1.Visit(this, null);
            identificationTable.OpenScope();
            ast.Declaration2.Visit(this, null);
            identificationTable.CloseLocalScope();
            return null;
        }

        #endregion

        #region Aggregates

        // These return the TypeDenoter for the Aggregate. Does not use the given object
        public object VisitMultipleArrayAggregate(MultipleArrayAggregate ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            TypeDenoter elementType = (TypeDenoter)ast.ArrayAggregate.Visit(this, null);
            ast.ElementCount = ast.ArrayAggregate.ElementCount + 1;
            if (!expressionType.Equals(elementType))
            {
                errorReporter.ReportError("incompatible array-aggregate element", "", ast.Expression.Position);
            }
            return elementType;
        }

        public object VisitSingleArrayAggregate(SingleArrayAggregate ast, object o)
        {
            TypeDenoter elementType = (TypeDenoter)ast.Expression.Visit(this, null);
            ast.ElementCount = 1;
            return elementType;
        }

        public object VisitMultipleRecordAggregate(MultipleRecordAggregate ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            FieldTypeDenoter recordType = (FieldTypeDenoter)ast.RecordAggregate.Visit(this, null);
            TypeDenoter fieldType = CheckFieldIdentifier(recordType, ast.Identifier);
            if (fieldType != StdEnvironment.errorType)
            {
                //It found a type for the field type, and thus an error
                errorReporter.ReportError("duplicate field \"%\" in record", ast.Identifier.Spelling, ast.Identifier.Position);
            }
            ast.Type = new MultipleFieldTypeDenoter(ast.Identifier, expressionType, recordType, ast.Position);
            return ast.Type;
        }
        public object VisitSingleRecordAggregate(SingleRecordAggregate ast, object o)
        {
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            ast.Type = new SingleFieldTypeDenoter(ast.Identifier, expressionType, ast.Position);
            return ast.Type;
        }

        #endregion

        #region Parameters

        //Always return null. Does not use the given object.
        //They are meant to construct and assign the type of each of the parameters

        public object VisitConstFormalParameter(ConstFormalParameter ast, object o)
        {
            ast.Type = (TypeDenoter)ast.Type.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("duplicated formal parameter \"%\"", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitFuncFormalParameter(FuncFormalParameter ast, object o)
        {
            identificationTable.OpenScope();
            ast.FormalParameterSequence.Visit(this, null);
            identificationTable.CloseScope();

            ast.Type = (TypeDenoter)ast.Type.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("duplicated formal parameter \"%\"", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitProcFormalParameter(ProcFormalParameter ast, object o)
        {
            identificationTable.OpenScope();
            ast.FormalParameterSequence.Visit(this, null);
            identificationTable.CloseScope();

            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("duplicated formal parameter \"%\"", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitVarFormalParameter(VarFormalParameter ast, object o)
        {
            ast.TypeDenoter = (TypeDenoter)ast.TypeDenoter.Visit(this, null);
            identificationTable.Enter(ast.Identifier.Spelling, ast);
            if (ast.Duplicated)
            {
                errorReporter.ReportError("duplicated formal parameter \"%\"", ast.Identifier.Spelling, ast.Position);
            }
            return null;
        }

        public object VisitEmptyFormalParameterSequence(EmptyFormalParameterSequence ast, object o)
        {
            return null;
        }

        public object VisitMultipleFormalParameterSequence(MultipleFormalParameterSequence ast, object o)
        {
            ast.FormalParameter.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            return null;
        }

        public object VisitSingleFormalParameterSequence(SingleFormalParameterSequence ast, object o)
        {
            ast.FormalParameter.Visit(this, null);
            return null;
        }

        //Always return null. Uses the given formal parameter
        public object VisitConstActualParameter(ConstActualParameter ast, object o)
        {
            FormalParameter formalParameter = (FormalParameter)o;
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);

            //It is a future call, thus we don't know its type yet
            if (expressionType == null)
            {
                identificationTable.AddFutureCallExpression(new FutureCallExpression(((ConstFormalParameter)formalParameter).Type,
                                                                                     ast.Expression));
            }
            if (formalParameter is not ConstFormalParameter)
            {
                errorReporter.ReportError("const actual parameter not expected here", "", ast.Position);
            }
            else if (!expressionType.Equals(((ConstFormalParameter)formalParameter).Type))
            {
                errorReporter.ReportError("wrong type for const actual parameter", "", ast.Position);
            }
            return null;
        }

        public object VisitFuncActualParameter(FuncActualParameter ast, object o)
        {
            FormalParameter formalParameter = (FormalParameter)o;

            Declaration binding = (Declaration)ast.Identifier.Visit(this, o);
            if (binding == null)
            {
                ReportUndeclared(ast.Identifier);
            }
            else if (binding is not FuncDeclaration or FuncFormalParameter)
            {
                errorReporter.ReportError("\"%\" is not a function", ast.Identifier.Spelling, ast.Position);
            }
            else if (formalParameter is not FuncFormalParameter)
            {
                errorReporter.ReportError("func actual parameter not expected here", "", ast.Position);
            }
            else
            {
                FormalParameterSequence formalParameterSequence;
                TypeDenoter type;
                if (binding is FuncDeclaration func)
                {
                    formalParameterSequence = func.FormalParameterSequence;
                    type = func.Type;
                }
                else
                {
                    formalParameterSequence = ((FuncFormalParameter)binding).FormalParameterSequence;
                    type = ((FuncFormalParameter)binding).Type;
                }
                //@TODO: Check this section
                if (!formalParameterSequence.Equals(((FuncFormalParameter)formalParameter).FormalParameterSequence))
                {
                    errorReporter.ReportError("wrong signature for function \"%\"", ast.Identifier.Spelling, ast.Identifier.Position); ;
                }
                else if (!type.Equals(((FuncFormalParameter)formalParameter).Type))
                {
                    errorReporter.ReportError("wrong type for function \"%\"", ast.Identifier.Spelling, ast.Identifier.Position);
                }
            }
            return null;
        }

        public object VisitProcActualParameter(ProcActualParameter ast, object o)
        {
            FormalParameter formalParameter = (FormalParameter)o;

            Declaration binding = (Declaration)ast.Identifier.Visit(this, o);
            if (binding == null)
            {
                ReportUndeclared(ast.Identifier);
            }
            else if (binding is not ProcDeclaration or ProcFormalParameter)
            {
                errorReporter.ReportError("\"%\" is not a procedure identifier", ast.Identifier.Spelling, ast.Position);
            }
            else if (formalParameter is not ProcFormalParameter)
            {
                errorReporter.ReportError("proc actual parameter not expected here", "", ast.Position);
            }
            else
            {
                FormalParameterSequence formalParameterSequence;
                if (binding is ProcDeclaration func)
                {
                    formalParameterSequence = func.FormalParameterSequence;
                }
                else
                {
                    formalParameterSequence = ((ProcFormalParameter)binding).FormalParameterSequence;
                }
                //@TODO: Check this section
                if (!formalParameterSequence.Equals(((FuncFormalParameter)formalParameter).FormalParameterSequence))
                {
                    errorReporter.ReportError("wrong signature for procedure \"%\"", ast.Identifier.Spelling, ast.Identifier.Position); ;
                }
            }
            return null;
        }

        public object VisitVarActualParameter(VarActualParameter ast, object o)
        {
            FormalParameter formalParameter = (FormalParameter)o;

            TypeDenoter variableType = (TypeDenoter)ast.Vname.Visit(this, null);
            if (!ast.Vname.IsVariable)
            {
                errorReporter.ReportError("actual parameter is not a variable", "", ast.Vname.Position);
            }
            else if (formalParameter is not VarFormalParameter)
            {
                errorReporter.ReportError("var actual parameter not expected here", "", ast.Vname.Position);
            }
            else if (!variableType.Equals(((VarFormalParameter)formalParameter).TypeDenoter))
            {
                errorReporter.ReportError("wrong type for var actual parameter", "", ast.Vname.Position);
            }
            return null;
        }

        public object VisitEmptyActualParameterSequence(EmptyActualParameterSequence ast, object o)
        {
            FormalParameterSequence formalParameterSequence = (FormalParameterSequence)o;
            if (formalParameterSequence is not EmptyFormalParameterSequence)
            {
                errorReporter.ReportError("too few actual parameters", "", ast.Position);
            }
            return null;
        }

        public object VisitMultipleActualParameterSequence(MultipleActualParameterSequence ast, object o)
        {
            FormalParameterSequence formalParameterSequence = (FormalParameterSequence)o;
            if (formalParameterSequence is MultipleFormalParameterSequence multipleFormalParameterSequence)
            {
                ast.ActualParameter.Visit(this, multipleFormalParameterSequence.FormalParameter);
                ast.ActualParameterSequence.Visit(this, multipleFormalParameterSequence.FormalParameterSequence);
            }
            else 
            {
                errorReporter.ReportError("too many actual parameters", "", ast.Position);
            }
            return null;
        }

        public object VisitSingleActualParameterSequence(SingleActualParameterSequence ast, object o)
        {
            FormalParameterSequence formalParameterSequence = (FormalParameterSequence)o;
            if (formalParameterSequence is SingleFormalParameterSequence singleFormalParameterSequence)
            {
                ast.ActualParameter.Visit(this, singleFormalParameterSequence.FormalParameter);
            }
            else
            {
                errorReporter.ReportError("incorrect number of actual parameters", "", ast.Position);
            }
            return null;
        }

        #endregion
    }
}
