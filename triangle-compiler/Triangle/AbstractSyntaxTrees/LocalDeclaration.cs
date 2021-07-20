
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LocalDeclaration : Declaration
    {
        private Declaration declaration1, declaration2;

        public LocalDeclaration(Declaration declaration1, Declaration declaration2,
                SourcePosition position) : base(position)
        {
            this.declaration1 = declaration1;
            this.declaration2 = declaration2;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLocalDeclaration(this, o);
        }
    }
}
