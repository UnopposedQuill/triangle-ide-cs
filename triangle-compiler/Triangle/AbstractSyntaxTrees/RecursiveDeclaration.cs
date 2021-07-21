
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecursiveDeclaration : Declaration
    {
        private Declaration declaration;

        public RecursiveDeclaration(Declaration dAST, SourcePosition position)
            : base(position)
        {
            declaration = dAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitRecursiveDeclaration(this, o);
        }
    }
}
