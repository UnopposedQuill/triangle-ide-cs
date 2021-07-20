
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LetExpression : Expression
    {
        private Declaration declaration;
        private Expression expression;

        public LetExpression(Declaration declaration, Expression expression,
                SourcePosition position) : base(position)
        {
            this.declaration = declaration;
            this.expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLetExpression(this, o);
        }
    }
}
