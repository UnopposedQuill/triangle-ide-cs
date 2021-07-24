
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
    }
}
