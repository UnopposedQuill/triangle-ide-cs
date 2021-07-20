
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstDeclaration : Declaration
    {
        private Identifier identifier;
        private Expression e;

        public ConstDeclaration(Identifier iAST, Expression eAST,
                SourcePosition position) : base(position)
        {
            identifier = iAST;
            e = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitConstDeclaration(this, o);
        }
    }
}
