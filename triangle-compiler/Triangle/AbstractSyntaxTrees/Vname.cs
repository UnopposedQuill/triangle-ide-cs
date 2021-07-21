
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Vname : AST
    {
        private bool IsVariable { get; set; }
        private bool Indexed { get; set; }
        private int Offset { get; set; }
        private TypeDenoter Type { get; set; }
        
        /**
         * Will create a new vname, but filling it is up to the caller
         */
        public Vname(SourcePosition position) : base(position)
        {
            IsVariable = false;
            Type = null;
        }
    }
}
