
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LetExpression : Expression
    {
        public Declaration Declaration { get; }
        public Expression Expression { get; }

        public LetExpression(Declaration declaration, Expression expression,
                SourcePosition position) : base(position)
        {
            Declaration = declaration;
            Expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLetExpression(this, o);
        }
    }
}
