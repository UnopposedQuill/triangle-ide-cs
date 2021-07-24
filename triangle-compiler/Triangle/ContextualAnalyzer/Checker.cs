
using System.Collections.Generic;
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    public class Checker : IVisitor
    {
        private IdentificationTable identificationTable;
        private readonly ErrorReporter errorReporter;
        private static readonly SourcePosition DUMMYPOS = new();


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

        #region Commands

        public object VisitAssignCommand(AssignCommand ast, object o)
        {
            TypeDenoter variableType = (TypeDenoter)ast.Variable.Visit(this, null);
            TypeDenoter expressionType = (TypeDenoter)ast.Expression.Visit(this, null);
            if (!ast.Variable.IsVariable)
            {
                errorReporter.ReportError("LHS of assignment is not a variable", "", ast.Variable.Position);
            }
            if (!expressionType.Equals(expressionType))
            {
                errorReporter.ReportError("assignment incompatibility", "", ast.Position);
            }
            return null;
        }

        public object VisitCallCommand(CallCommand ast, object o)
        {
            Declaration binding = o != null ? (Declaration) o : (Declaration)ast.Identifier.Visit(this, null);

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
            TypeDeclaration binding = new TypeDeclaration(new Identifier(id, DUMMYPOS), typeDenoter, DUMMYPOS);
            identificationTable.Enter(id, binding);
            return binding;
        }



        #endregion
    }
}
