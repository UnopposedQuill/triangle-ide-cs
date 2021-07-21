
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SequentialDeclaration : Declaration
    {
        private Declaration declaration1, declaration2;

        public SequentialDeclaration(Declaration d1AST, Declaration d2AST,
                SourcePosition position) : base(position)
        {
            declaration1 = d1AST;
            declaration2 = d2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSequentialDeclaration(this, o);
        }
    }
}
