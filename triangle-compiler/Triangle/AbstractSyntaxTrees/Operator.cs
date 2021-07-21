
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Operator : Terminal
    {
        private Declaration declaration;

        public Operator(string spelling, SourcePosition position) : base(spelling, position)
        {
            declaration = null;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitOperator(this, o);
        }
    }
}
