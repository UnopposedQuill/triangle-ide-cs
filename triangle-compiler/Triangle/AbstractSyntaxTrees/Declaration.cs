
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Declaration : AST
    {
        //This is for duplicated identifier error checkings
        internal bool Duplicated { get; set; }

        public Declaration(SourcePosition position) : base(position)
        {
            Duplicated = false;
        }
    }
}
