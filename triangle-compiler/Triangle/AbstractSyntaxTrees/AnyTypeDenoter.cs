
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class AnyTypeDenoter : TypeDenoter
    {
        public AnyTypeDenoter(SourcePosition position) : base(position)
        {

        }

        public object Visit(IVisitor v, object o)
        {
            return v.VisitAnyTypeDenoter(this, o);
        }

        /**
         * Since this is a control class, we need it to return different
         * when comparing it to anything.
         */
        public override bool Equals(object obj)
        {
            return false;
        }
    }
}
