
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class RecordAggregate: AST
    {
        private FieldTypeDenoter type;

        public RecordAggregate(SourcePosition position) : base(position)
        {
            type = null;
        }
    }
}