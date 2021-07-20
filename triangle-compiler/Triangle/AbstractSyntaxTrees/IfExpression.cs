
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IfExpression : Expression
    {
        private Expression expression1, expression2, expression3;

        public IfExpression(Expression expression1, Expression expression2, Expression expression3,
                SourcePosition position) : base(position)
        {
            this.expression1 = expression1;
            this.expression2 = expression2;
            this.expression3 = expression3;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIfExpression(this, o);
        }
    }
}
