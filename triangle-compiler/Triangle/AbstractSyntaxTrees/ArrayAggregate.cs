
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class ArrayAggregate : AST
    {
        private int elementCount;

        public ArrayAggregate(SourcePosition position) : base(position)
        {
            elementCount = 0;
        }

        public int GetElementCount()
        {
            return elementCount;
        }

        public void SetElementCount(int elementCount)
        {
            this.elementCount = elementCount;
        }
    }
}
