
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IntegerExpression : Expression
    {
        private IntegerLiteral integerLiteral;

        public IntegerExpression(IntegerLiteral integerLiteral,
            SourcePosition position) : base(position)
        {
            this.integerLiteral = integerLiteral;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIntegerExpression(this, o);
        }
    }
}
