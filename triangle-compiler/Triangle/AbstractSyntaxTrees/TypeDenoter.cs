
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class TypeDenoter : AST
    {
        public TypeDenoter(SourcePosition position) : base(position)
        {
        }

        //Every type subclass has to implement a new Equals
        public abstract override bool Equals(object obj);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
