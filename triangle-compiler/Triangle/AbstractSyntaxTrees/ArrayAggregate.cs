
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class ArrayAggregate : AST
    {
        public int ElementCount { get; internal set; }

        public ArrayAggregate(SourcePosition position) : base(position)
        {
            ElementCount = 0;
        }
    }
}
