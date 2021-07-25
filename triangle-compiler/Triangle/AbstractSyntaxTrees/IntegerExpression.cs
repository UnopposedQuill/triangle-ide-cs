
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IntegerExpression : Expression
    {
        public IntegerLiteral IntegerLiteral { get; }

        public IntegerExpression(IntegerLiteral integerLiteral,
            SourcePosition position) : base(position)
        {
            IntegerLiteral = integerLiteral;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIntegerExpression(this, o);
        }
    }
}
