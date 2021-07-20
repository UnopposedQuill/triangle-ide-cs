
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IntegerLiteral : Terminal
    {
        public IntegerLiteral(string spelling, SourcePosition position) : base(spelling, position)
        {
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIntegerLiteral(this, o);
        }
    }
}
