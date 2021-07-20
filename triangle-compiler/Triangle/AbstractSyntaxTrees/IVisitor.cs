
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public interface IVisitor
    {
        // Programs
        public abstract object VisitProgram(Program ast, object o);

        // Commands
        public abstract object VisitAssignCommand(AssignCommand ast, object o);
        public abstract object VisitCallCommand(CallCommand ast, object o);
        public abstract object VisitEmptyCommand(EmptyCommand ast, object o);
        public abstract object VisitIfCommand(IfCommand ast, object o);
        public abstract object VisitLetCommand(LetCommand ast, object o);
        public abstract object VisitSequentialCommand(SequentialCommand ast, object o);
        public abstract object VisitWhileLoopCommand(LoopCommand ast, object o);
        public abstract object VisitDoWhileLoopCommand(LoopCommand ast, object o);
        public abstract object VisitUntilLoopCommand(LoopCommand ast, object o);
        public abstract object VisitDoUntilLoopCommand(LoopCommand ast, object o);
        public abstract object VisitForLoopCommand(ForLoopCommand ast, object o);

        // Expressions
        public abstract object VisitArrayExpression(ArrayExpression ast, object o);
        public abstract object VisitBinaryExpression(BinaryExpression ast, object o);
        public abstract object VisitCallExpression(CallExpression ast, object o);
        public abstract object VisitCharacterExpression(CharacterExpression ast, object o);
        public abstract object VisitEmptyExpression(EmptyExpression ast, object o);
        public abstract object VisitIfExpression(IfExpression ast, object o);
        public abstract object VisitIntegerExpression(IntegerExpression ast, object o);
        public abstract object VisitLetExpression(LetExpression ast, object o);
        public abstract object VisitRecordExpression(RecordExpression ast, object o);
        public abstract object VisitUnaryExpression(UnaryExpression ast, object o);
        public abstract object VisitVnameExpression(VnameExpression ast, object o);

        // Declarations
        public abstract object VisitBinaryOperatorDeclaration(BinaryOperatorDeclaration ast, object o);
        public abstract object VisitConstDeclaration(ConstDeclaration ast, object o);
        public abstract object VisitFuncDeclaration(FuncDeclaration ast, object o);
        public abstract object VisitProcDeclaration(ProcDeclaration ast, object o);
        public abstract object VisitSequentialDeclaration(SequentialDeclaration ast, object o);
        public abstract object VisitTypeDeclaration(TypeDeclaration ast, object o);
        public abstract object VisitUnaryOperatorDeclaration(UnaryOperatorDeclaration ast, object o);
        public abstract object VisitVarDeclaration(VarDeclaration ast, object o);
        public abstract object VisitVarDeclarationInitialized(VarDeclarationInitialized ast, object o);
        public abstract object VisitRecursiveDeclaration(RecursiveDeclaration ast, object o);
        public abstract object VisitLocalDeclaration(LocalDeclaration ast, object o);

        // Array Aggregates
        public abstract object VisitMultipleArrayAggregate(MultipleArrayAggregate ast, object o);
        public abstract object VisitSingleArrayAggregate(SingleArrayAggregate ast, object o);

        // Record Aggregates
        public abstract object VisitMultipleRecordAggregate(MultipleRecordAggregate ast, object o);
        public abstract object VisitSingleRecordAggregate(SingleRecordAggregate ast, object o);

        // Formal Parameters
        public abstract object VisitConstFormalParameter(ConstFormalParameter ast, object o);
        public abstract object VisitFuncFormalParameter(FuncFormalParameter ast, object o);
        public abstract object VisitProcFormalParameter(ProcFormalParameter ast, object o);
        public abstract object VisitVarFormalParameter(VarFormalParameter ast, object o);

        public abstract object VisitEmptyFormalParameterSequence(EmptyFormalParameterSequence ast, object o);
        public abstract object VisitMultipleFormalParameterSequence(MultipleFormalParameterSequence ast, object o);
        public abstract object VisitSingleFormalParameterSequence(SingleFormalParameterSequence ast, object o);

        // Actual Parameters
        public abstract object VisitConstActualParameter(ConstActualParameter ast, object o);
        public abstract object VisitFuncActualParameter(FuncActualParameter ast, object o);
        public abstract object VisitProcActualParameter(ProcActualParameter ast, object o);
        public abstract object VisitVarActualParameter(VarActualParameter ast, object o);

        public abstract object VisitEmptyActualParameterSequence(EmptyActualParameterSequence ast, object o);
        public abstract object VisitMultipleActualParameterSequence(MultipleActualParameterSequence ast, object o);
        public abstract object VisitSingleActualParameterSequence(SingleActualParameterSequence ast, object o);

        // Type Denoters
        public abstract object VisitAnyTypeDenoter(AnyTypeDenoter ast, object o);
        public abstract object VisitArrayTypeDenoter(ArrayTypeDenoter ast, object o);
        public abstract object VisitBoolTypeDenoter(BoolTypeDenoter ast, object o);
        public abstract object VisitCharTypeDenoter(CharTypeDenoter ast, object o);
        public abstract object VisitErrorTypeDenoter(ErrorTypeDenoter ast, object o);
        public abstract object VisitSimpleTypeDenoter(SimpleTypeDenoter ast, object o);
        public abstract object VisitIntTypeDenoter(IntTypeDenoter ast, object o);
        public abstract object VisitRecordTypeDenoter(RecordTypeDenoter ast, object o);

        public abstract object VisitMultipleFieldTypeDenoter(MultipleFieldTypeDenoter ast, object o);
        public abstract object VisitSingleFieldTypeDenoter(SingleFieldTypeDenoter ast, object o);

        // Literals, Identifiers and Operators
        public abstract object VisitCharacterLiteral(CharacterLiteral ast, object o);
        public abstract object VisitIdentifier(Identifier ast, object o);
        public abstract object VisitIntegerLiteral(IntegerLiteral ast, object o);
        public abstract object VisitOperator(Operator ast, object o);

        // Value-or-variable names
        public abstract object VisitDotVname(DotVname ast, object o);
        public abstract object VisitSimpleVname(SimpleVname ast, object o);
        public abstract object VisitSubscriptVname(SubscriptVname ast, object o);
    }
}
