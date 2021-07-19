
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CharTypeDenoter : TypeDenoter
    {
        public CharTypeDenoter(SourcePosition position) : base(position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCharTypeDenoter(this, o);
        }

        public override bool Equals(object obj)
        {
            //This is a bypass for error handling
            if (obj != null && obj is ErrorTypeDenoter)
            {
                return true;
            }
            return obj != null && obj is CharTypeDenoter;
        }
    }
}
