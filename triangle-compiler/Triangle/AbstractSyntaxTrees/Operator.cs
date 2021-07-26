
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Operator : Terminal
    {
        public Declaration Declaration { get; internal set; }

        public Operator(string spelling, SourcePosition position) : base(spelling, position)
        {
            Declaration = null;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitOperator(this, o);
        }
    }
}
