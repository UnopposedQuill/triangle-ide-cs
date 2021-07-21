
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class EmptyFormalParameterSequence : FormalParameterSequence
    {
        public EmptyFormalParameterSequence(SourcePosition position) : base(position)
        {

        }

        /**
         * Will be the same as another one of the same type
         */
        public override bool Equals(object obj)
        {
            return obj is EmptyFormalParameterSequence;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitEmptyFormalParameterSequence(this, o);
        }
    }
}
