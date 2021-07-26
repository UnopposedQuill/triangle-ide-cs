
using System.IO;
using System.Text;
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ProgramWriter
{
    class XMLWriterVisitor : IVisitor
    {
        private readonly FileStream xmlFileStream;

        public XMLWriterVisitor(FileStream xmlFileStream)
        {
            this.xmlFileStream = xmlFileStream;
        }

        #region Programs

        public object VisitProgram(Program ast, object o)
        {
            WriteToXMLFile("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n");
            WriteToXMLFile("<Program>\n");
            ast.Command.Visit(this, null);
            WriteToXMLFile("</Program>\n");
            return null;
        }

        #endregion

        #region Commands

        public object VisitAssignCommand(AssignCommand ast, object o)
        {
            WriteToXMLFile("<AssignCommand>\n");
            ast.Vname.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</AssignCommand>\n");
            return null;
        }

        public object VisitCallCommand(CallCommand ast, object o)
        {
            WriteToXMLFile("<CallCommand>\n");
            ast.Identifier.Visit(this, null);
            ast.ActualParameterSequence.Visit(this, null);
            WriteToXMLFile("</CallCommand>\n");
            return null;
        }

        public object VisitEmptyCommand(EmptyCommand ast, object o)
        {
            WriteToXMLFile("<EmptyCommand/>\n");
            return null;
        }

        public object VisitIfCommand(IfCommand ast, object o)
        {
            WriteToXMLFile("<IfCommand>\n");
            ast.Expression.Visit(this, null);
            ast.Command1.Visit(this, null);
            ast.Command2.Visit(this, null);
            WriteToXMLFile("</IfCommand>\n");
            return null;
        }

        public object VisitLetCommand(LetCommand ast, object o)
        {
            WriteToXMLFile("<LetCommand>\n");
            ast.Declaration.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</LetCommand>\n");
            return null;

        }

        public object VisitSequentialCommand(SequentialCommand ast, object o)
        {
            WriteToXMLFile("<SequentialCommand>\n");
            ast.Command1.Visit(this, null);
            ast.Command2.Visit(this, null);
            WriteToXMLFile("</SequentialCommand>\n");
            return null;
        }

        public object VisitWhileLoopCommand(WhileLoopCommand ast, object o)
        {
            WriteToXMLFile("<WhileLoopCommand>\n");
            ast.Expression.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</WhileLoopCommand>\n");
            return null;
        }

        public object VisitDoWhileLoopCommand(DoWhileLoopCommand ast, object o)
        {
            WriteToXMLFile("<Do-WhileLoopCommand>\n");
            ast.Expression.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</Do-WhileLoopCommand>\n");
            return null;
        }

        public object VisitUntilLoopCommand(UntilLoopCommand ast, object o)
        {
            WriteToXMLFile("<UntilLoopCommand>\n");
            ast.Expression.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</UntilLoopCommand>\n");
            return null;
        }

        public object VisitDoUntilLoopCommand(DoUntilLoopCommand ast, object o)
        {
            WriteToXMLFile("<Do-UntilLoopCommand>\n");
            ast.Expression.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</Do-UntilLoopCommand>\n");
            return null;
        }

        public object VisitForLoopCommand(ForLoopCommand ast, object o)
        {
            WriteToXMLFile("<ForLoopCommand>\n");
            ast.InitialDeclaration.Visit(this, null);
            ast.HaltingExpression.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</ForLoopCommand>\n");
            return null;
        }

        #endregion

        #region Expressions

        public object VisitArrayExpression(ArrayExpression ast, object o)
        {
            WriteToXMLFile("<ArrayExpression>\n");
            ast.ArrayAggregate.Visit(this, null);
            WriteToXMLFile("</ArrayExpression>\n");
            return null;
        }


        public object VisitBinaryExpression(BinaryExpression ast, object o)
        {
            WriteToXMLFile("<BinaryExpression>\n");
            ast.Expression1.Visit(this, null);
            ast.Operator.Visit(this, null);
            ast.Expression2.Visit(this, null);
            WriteToXMLFile("</BinaryExpression>\n");
            return null;
        }


        public object VisitCallExpression(CallExpression ast, object o)
        {
            WriteToXMLFile("<CallExpression>\n");
            ast.Identifier.Visit(this, null);
            ast.ActualParameterSequence.Visit(this, null);
            WriteToXMLFile("</CallExpression>\n");
            return null;
        }


        public object VisitCharacterExpression(CharacterExpression ast, object o)
        {
            WriteToXMLFile("<CharacterExpression>\n");
            ast.CharacterLiteral.Visit(this, null);
            WriteToXMLFile("</CharacterExpression>\n");
            return null;
        }


        public object VisitEmptyExpression(EmptyExpression ast, object o)
        {
            WriteToXMLFile("<EmptyExpression/>\n");
            return null;
        }


        public object VisitIfExpression(IfExpression ast, object o)
        {
            WriteToXMLFile("<IfExpression>\n");
            ast.ConditionExpression.Visit(this, null);
            ast.Expression1.Visit(this, null);
            ast.Expression2.Visit(this, null);
            WriteToXMLFile("</IfExpression>\n");
            return null;
        }


        public object VisitIntegerExpression(IntegerExpression ast, object o)
        {
            WriteToXMLFile("<IntegerExpression>\n");
            ast.IntegerLiteral.Visit(this, null);
            WriteToXMLFile("</IntegerExpression>\n");
            return null;
        }


        public object VisitLetExpression(LetExpression ast, object o)
        {
            WriteToXMLFile("<LetExpression>\n");
            ast.Declaration.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</LetExpression>\n");
            return null;
        }


        public object VisitRecordExpression(RecordExpression ast, object o)
        {
            WriteToXMLFile("<RecordExpression>\n");
            ast.RecordAggregate.Visit(this, null);
            WriteToXMLFile("</RecordExpression>\n");
            return null;
        }


        public object VisitUnaryExpression(UnaryExpression ast, object o)
        {
            WriteToXMLFile("<UnaryExpression>\n");
            ast.Operator.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</UnaryExpression>\n");
            return null;
        }


        public object VisitVnameExpression(VnameExpression ast, object o)
        {
            WriteToXMLFile("<VnameExpression>\n");
            ast.Vname.Visit(this, null);
            WriteToXMLFile("</VnameExpression>\n");
            return null;
        }

        #endregion

        #region Declarations

        public object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, object o)
        {
            WriteToXMLFile("BinaryOperatorDeclaration>\n");
            ast.Argument1Type.Visit(this, null);
            ast.Operator.Visit(this, null);
            ast.Argument2Type.Visit(this, null);
            ast.ResultType.Visit(this, null);
            WriteToXMLFile("</BinaryOperatorDeclaration>\n");
            return null;
        }


        public object VisitConstDeclaration(ConstDeclaration ast, object o)
        {
            WriteToXMLFile("<ConstantDeclaration>\n");
            ast.Identifier.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</ConstantDeclaration>\n");
            return null;
        }


        public object VisitFuncDeclaration(FuncDeclaration ast, object o)
        {
            WriteToXMLFile("<FunctionDeclaration>\n");
            ast.Identifier.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            ast.Type.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</FunctionDeclaration>\n");
            return null;
        }


        public object VisitProcDeclaration(ProcDeclaration ast, object o)
        {
            WriteToXMLFile("<ProcedureDeclaration>\n");
            ast.Identifier.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            ast.Command.Visit(this, null);
            WriteToXMLFile("</ProcedureDeclaration>\n");
            return null;
        }


        public object VisitSequentialDeclaration(SequentialDeclaration ast, object o)
        {
            WriteToXMLFile("<SequentialDeclaration>\n");
            ast.Declaration1.Visit(this, null);
            ast.Declaration2.Visit(this, null);
            WriteToXMLFile("</SequentialDeclaration>\n");
            return null;
        }


        public object VisitTypeDeclaration(TypeDeclaration ast, object o)
        {
            WriteToXMLFile("<TypeDeclaration>\n");
            ast.Identifier.Visit(this, null);
            ast.TypeDenoter.Visit(this, null);
            WriteToXMLFile("</TypeDeclaration>\n");
            return null;
        }


        public object VisitUnaryOperatorDeclaration(UnaryOperatorDeclaration ast, object o)
        {
            WriteToXMLFile("<UnaryOperatorDeclaration>\n");
            ast.ArgumentTypeDenoter.Visit(this, null);
            ast.Operator.Visit(this, null);
            ast.ResultTypeDenoter.Visit(this, null);
            WriteToXMLFile("</UnaryOperatorDeclaration>\n");
            return null;
        }


        public object VisitVarDeclaration(VarDeclaration ast, object o)
        {
            WriteToXMLFile("<VariableDeclaration>\n");
            ast.Identifier.Visit(this, null);
            ast.TypeDenoter.Visit(this, null);
            WriteToXMLFile("</VariableDeclaration>\n");
            return null;
        }


        public object VisitVarDeclarationInitialized(VarDeclarationInitialized ast, object o)
        {
            WriteToXMLFile("<VarDeclarationInitialized>\n");
            ast.Identifier.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</VarDeclarationInitialized>\n");
            return null;
        }


        public object VisitRecursiveDeclaration(RecursiveDeclaration ast, object o)
        {
            WriteToXMLFile("<RecursiveDeclaration>\n");
            ast.Declaration.Visit(this, null);
            WriteToXMLFile("</RecursiveDeclaration>\n");
            return null;
        }


        public object VisitLocalDeclaration(LocalDeclaration ast, object o)
        {
            WriteToXMLFile("<LocalDeclaration>\n");
            ast.Declaration1.Visit(this, null);
            ast.Declaration2.Visit(this, null);
            WriteToXMLFile("</LocalDeclaration>\n");
            return null;
        }

        #endregion

        #region Aggregates

        public object VisitMultipleArrayAggregate(MultipleArrayAggregate ast, object o)
        {
            WriteToXMLFile("<MultipleArrayAggregate>\n");
            ast.Expression.Visit(this, null);
            ast.ArrayAggregate.Visit(this, null);
            WriteToXMLFile("</MultipleArrayAggregate>\n");
            return null;
        }


        public object VisitSingleArrayAggregate(SingleArrayAggregate ast, object o)
        {
            WriteToXMLFile("<SingleArrayAggregate>\n");
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</SingleArrayAggregate>\n");
            return null;
        }


        public object VisitMultipleRecordAggregate(MultipleRecordAggregate ast, object o)
        {
            WriteToXMLFile("<MultipleRecordAggregate>\n");
            ast.Identifier.Visit(this, null);
            ast.Expression.Visit(this, null);
            ast.RecordAggregate.Visit(this, null);
            WriteToXMLFile("</MultipleRecordAggregate>\n");
            return null;
        }


        public object VisitSingleRecordAggregate(SingleRecordAggregate ast, object o)
        {
            WriteToXMLFile("<SingleRecordAggregate>\n");
            ast.Identifier.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</SingleRecordAggregate\n>");
            return null;
        }

        #endregion

        #region Parameters

        public object VisitConstFormalParameter(ConstFormalParameter ast, object o)
        {
            WriteToXMLFile("<ConstantFormalParameters>/n");
            ast.Identifier.Visit(this, null);
            ast.Type.Visit(this, null);
            WriteToXMLFile("</ConstantFormalParameters>/n");
            return null;
        }


        public object VisitFuncFormalParameter(FuncFormalParameter ast, object o)
        {
            WriteToXMLFile("<FunctionFormalParameter>\n");
            ast.Identifier.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            ast.Type.Visit(this, null);
            WriteToXMLFile("</FunctionFormalParameter>\n");
            return null;
        }


        public object VisitProcFormalParameter(ProcFormalParameter ast, object o)
        {
            WriteToXMLFile("<ProcedureFormalParameter>\n");
            ast.Identifier.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            WriteToXMLFile("</ProcedureFormalParameter>\n");
            return null;
        }


        public object VisitVarFormalParameter(VarFormalParameter ast, object o)
        {
            WriteToXMLFile("<VariableFormalParameter>\n");
            ast.Identifier.Visit(this, null);
            ast.TypeDenoter.Visit(this, null);
            WriteToXMLFile("</VariableFormalParameter>\n");
            return null;
        }


        public object VisitEmptyFormalParameterSequence(EmptyFormalParameterSequence ast, object o)
        {
            WriteToXMLFile("<EmptyFormalParameterSequence/>\n");
            return null;
        }


        public object VisitMultipleFormalParameterSequence(MultipleFormalParameterSequence ast, object o)
        {
            WriteToXMLFile("<MultipleFormalParameterSequence>\n");
            ast.FormalParameter.Visit(this, null);
            ast.FormalParameterSequence.Visit(this, null);
            WriteToXMLFile("</MultipleFormalParameterSequence>\n");
            return null;
        }


        public object VisitSingleFormalParameterSequence(SingleFormalParameterSequence ast, object o)
        {
            WriteToXMLFile("<SingleFormalParameterSequence>\n");
            ast.FormalParameter.Visit(this, null);
            WriteToXMLFile("</SingleFormalParameterSequence>\n");
            return null;
        }


        public object VisitConstActualParameter(ConstActualParameter ast, object o)
        {
            WriteToXMLFile("<ConstantActualParameter>\n");
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</ConstantActualParameter>\n");
            return null;
        }


        public object VisitFuncActualParameter(FuncActualParameter ast, object o)
        {
            WriteToXMLFile("<FunctionActualParameter>\n");
            ast.Identifier.Visit(this, null);
            WriteToXMLFile("</FunctionActualParameter>\n");
            return null;
        }


        public object VisitProcActualParameter(ProcActualParameter ast, object o)
        {
            WriteToXMLFile("<ProcedureActualParameter>\n");
            ast.Identifier.Visit(this, null);
            WriteToXMLFile("</ProcedureActualParameter>\n");
            return null;
        }


        public object VisitVarActualParameter(VarActualParameter ast, object o)
        {
            WriteToXMLFile("<VariableActualParameter>\n");
            ast.Vname.Visit(this, null);
            WriteToXMLFile("</VariableActualParameter>\n");
            return null;
        }


        public object VisitEmptyActualParameterSequence(EmptyActualParameterSequence ast, object o)
        {
            WriteToXMLFile("<EmptyActualParameterSequence/>\n");
            return null;
        }


        public object VisitMultipleActualParameterSequence(MultipleActualParameterSequence ast, object o)
        {
            WriteToXMLFile("<MultipleActualParameterSequence>\n");
            ast.ActualParameter.Visit(this, null);
            ast.ActualParameterSequence.Visit(this, null);
            WriteToXMLFile("</MultipleActualParameterSequence>\n");
            return null;
        }


        public object VisitSingleActualParameterSequence(SingleActualParameterSequence ast, object o)
        {
            WriteToXMLFile("<SingleActualParameterSequence>\n");
            ast.ActualParameter.Visit(this, null);
            WriteToXMLFile("</SingleActualParameterSequence>\n");
            return null;
        }

        #endregion

        #region TypeDenoters

        public object VisitAnyTypeDenoter(AnyTypeDenoter ast, object o)
        {
            WriteToXMLFile("<AnyTypeDenoter/>\n");
            return null;
        }


        public object VisitArrayTypeDenoter(ArrayTypeDenoter ast, object o)
        {
            WriteToXMLFile("<ArrayTypeDenoter>\n");
            ast.IntegerLiteral.Visit(this, null);
            ast.Type.Visit(this, null);
            WriteToXMLFile("</ArrayTypeDenoter>\n");
            return null;
        }


        public object VisitBoolTypeDenoter(BoolTypeDenoter ast, object o)
        {
            WriteToXMLFile("<BoolTypeDenoter/>\n");
            return null;
        }


        public object VisitCharTypeDenoter(CharTypeDenoter ast, object o)
        {
            WriteToXMLFile("<CharTypeDenoter/>\n");
            return null;
        }


        public object VisitErrorTypeDenoter(ErrorTypeDenoter ast, object o)
        {
            WriteToXMLFile("<ErrorTypeDenoter/>\n");
            return null;
        }


        public object VisitSimpleTypeDenoter(SimpleTypeDenoter ast, object o)
        {
            WriteToXMLFile("<SimpleTypeDenoter>\n");
            ast.Identifier.Visit(this, null);
            WriteToXMLFile("</SimpleTypeDenoter>\n");
            return null;
        }


        public object VisitIntTypeDenoter(IntTypeDenoter ast, object o)
        {
            WriteToXMLFile("<IntTypeDenoter/>\n");
            return null;
        }


        public object VisitRecordTypeDenoter(RecordTypeDenoter ast, object o)
        {
            WriteToXMLFile("<RecordTypeDenoter>\n");
            ast.FieldTypeDenoter.Visit(this, null);
            WriteToXMLFile("</RecordTypeDenoter>\n");
            return null;
        }


        public object VisitMultipleFieldTypeDenoter(MultipleFieldTypeDenoter ast, object o)
        {
            WriteToXMLFile("<MultipleFieldTypeDenoter>\n");
            ast.Identifier.Visit(this, null);
            ast.TypeDenoter.Visit(this, null);
            ast.FieldTypeDenoter.Visit(this, null);
            WriteToXMLFile("</MultipleFieldTypeDenoter>\n");
            return null;
        }


        public object VisitSingleFieldTypeDenoter(SingleFieldTypeDenoter ast, object o)
        {
            WriteToXMLFile("<SingleFieldTypeDenoter>\n");
            ast.Identifier.Visit(this, null);
            ast.TypeDenoter.Visit(this, null);
            WriteToXMLFile("</SingleFieldTypeDenoter>\n");
            return null;
        }

        #endregion

        #region Literals, Identifiers and Operators

        public object VisitCharacterLiteral(CharacterLiteral ast, object o)
        {
            WriteToXMLFile("<CharaterLiteral " + "value=\"" + ast.Spelling + "\"/>\n");
            return null;
        }


        public object VisitIdentifier(Identifier ast, object o)
        {
            WriteToXMLFile("<Identifier " + "value=\"" + ast.Spelling + "\"/>\n");
            return null;
        }


        public object VisitIntegerLiteral(IntegerLiteral ast, object o)
        {
            WriteToXMLFile("<IntegerLiteral " + "value=\"" + ast.Spelling + "\"/>\n");
            return null;
        }


        public object VisitOperator(Operator ast, object o)
        {
            WriteToXMLFile("<Operator " + "value=\"" + ast.Spelling + "\"/>\n");
            return null;
        }

        #endregion

        #region Values or Variables Names

        public object VisitDotVname(DotVname ast, object o)
        {
            WriteToXMLFile("<DotVname>\n");
            ast.Identifier.Visit(this, null);
            ast.Vname.Visit(this, null);
            WriteToXMLFile("</DotVname>\n");
            return null;
        }


        public object VisitSimpleVname(SimpleVname ast, object o)
        {
            WriteToXMLFile("<SimpleVname>\n");
            ast.Identifier.Visit(this, null);
            WriteToXMLFile("</SimpleVname>\n");
            return null;
        }


        public object VisitSubscriptVname(SubscriptVname ast, object o)
        {
            WriteToXMLFile("<SubscriptVname>\n");
            ast.Vname.Visit(this, null);
            ast.Expression.Visit(this, null);
            WriteToXMLFile("</SubscriptVname>\n");
            return null;
        }

        #endregion

        private void WriteToXMLFile(string content)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(content);
            xmlFileStream.Write(info, 0, info.Length);
            xmlFileStream.Flush();
        }
    }
}
