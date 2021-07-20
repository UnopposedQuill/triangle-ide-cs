
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Declaration : AST
    {
        //This is for duplicated identifier error checkings
        private bool duplicated;

        public Declaration(SourcePosition position) : base(position)
        {
            duplicated = false;
        }

        public bool IsDuplicated()
        {
            return duplicated;
        }

        public void setDuplicated(bool duplicated)
        {
            this.duplicated = duplicated;
        }
    }
}
