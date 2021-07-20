
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ErrorTypeDenoter : TypeDenoter
    {
        public ErrorTypeDenoter(SourcePosition position) : base(position)
        {
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitErrorTypeDenoter(this, o);
        }

        /**
         * This is a bypass for error handling
         */
        public override bool Equals(object obj)
        {
            return obj is TypeDenoter;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
