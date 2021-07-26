
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Vname : AST
    {
        public bool IsVariable { get; internal set; }
        public bool Indexed { get; }
        public int Offset { get; }
        public TypeDenoter Type { get; internal set; }
        
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
