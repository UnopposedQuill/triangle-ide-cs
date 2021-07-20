
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Expression : AST
    {
        private TypeDenoter type;

        public Expression(SourcePosition position) : base(position)
        {
            type = null;
        }

        public TypeDenoter GetTypeDenoter()
        {
            return type;
        }

        public void SetTypeDenoter(TypeDenoter type)
        {
            this.type = type;
        }
    }
}
