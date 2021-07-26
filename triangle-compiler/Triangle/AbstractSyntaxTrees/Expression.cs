
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Expression : AST
    {
        public TypeDenoter Type { get; internal set; }

        public Expression(SourcePosition position) : base(position)
        {
            Type = null;
        }
    }
}
