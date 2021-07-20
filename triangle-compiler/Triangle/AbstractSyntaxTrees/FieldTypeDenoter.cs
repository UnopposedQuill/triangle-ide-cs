
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class FieldTypeDenoter : TypeDenoter
    {
        public FieldTypeDenoter(SourcePosition position) : base(position)
        {

        }

        public abstract override bool Equals(object obj);
        public abstract override int GetHashCode();
    }
}
