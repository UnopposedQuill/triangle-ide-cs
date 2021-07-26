
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class RecordAggregate: AST
    {
        public FieldTypeDenoter Type { get; internal set; }

        public RecordAggregate(SourcePosition position) : base(position)
        {
            Type = null;
        }
    }
}