
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class BoolTypeDenoter : TypeDenoter
    {
        public BoolTypeDenoter(SourcePosition position) : base(position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitBoolTypeDenoter(this, o);
        }

        public override bool Equals(object obj)
        {
            //This is a bypass for error handling
            if ((obj != null) && (obj is ErrorTypeDenoter))
            {
                return true;
            }
            return ((obj != null) && (obj is BoolTypeDenoter));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
