
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecursiveDeclaration : Declaration
    {
        public Declaration Declaration { get; }

        public RecursiveDeclaration(Declaration dAST, SourcePosition position)
            : base(position)
        {
            Declaration = dAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitRecursiveDeclaration(this, o);
        }
    }
}
