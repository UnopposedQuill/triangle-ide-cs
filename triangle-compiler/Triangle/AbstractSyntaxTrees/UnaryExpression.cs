
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UnaryExpression : Expression
    {
        public Expression Expression { get; }
        public Operator Operator { get; }

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
