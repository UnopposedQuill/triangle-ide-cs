
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class ArrayAggregate : AST
    {
        public int ElementCount { get; }

        public ArrayAggregate(SourcePosition position) : base(position)
        {
            ElementCount = 0;
        }
    }
}
