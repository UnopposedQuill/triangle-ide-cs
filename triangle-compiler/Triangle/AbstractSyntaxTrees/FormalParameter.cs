
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class FormalParameter : Declaration
    {
        public FormalParameter(SourcePosition position) : base(position)
        {

        }

        public override abstract bool Equals(object obj);
        public override abstract int GetHashCode();
    }
}
