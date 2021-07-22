
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UnaryExpression : Expression
    {
        private Expression Expression { get; set; }
        private Operator Operator { get; set; }

        public UnaryExpression(Operator oAST, Expression eAST,
                SourcePosition position) : base(position)
        {
            Operator = oAST;
            Expression = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitUnaryExpression(this, o);
        }
    }
}
