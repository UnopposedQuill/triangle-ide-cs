
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class EmptyExpression : Expression
    {
        public EmptyExpression(SourcePosition position) : base(position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitEmptyExpression(this, o);
        }
    }
}
