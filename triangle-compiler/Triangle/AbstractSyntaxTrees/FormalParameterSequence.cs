
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class FormalParameterSequence : AST
    {
        public FormalParameterSequence(SourcePosition position) : base(position)
        {
        }

        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
    }
}
