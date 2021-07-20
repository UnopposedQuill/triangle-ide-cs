
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IntTypeDenoter : TypeDenoter
    {
        public IntTypeDenoter(SourcePosition position) : base(position)
        {

        }

        public override bool Equals(object obj)
        {
            //This is a bypass for error handling
            if (obj != null && obj is ErrorTypeDenoter)
            {
                return true;
            }
            return obj != null && obj is IntTypeDenoter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIntTypeDenoter(this, o);
        }
    }
}
