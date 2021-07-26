
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LocalDeclaration : Declaration
    {
        public Declaration Declaration1 { get; }
        public Declaration Declaration2 { get; }

        public LocalDeclaration(Declaration declaration1, Declaration declaration2,
                SourcePosition position) : base(position)
        {
            this.Declaration1 = declaration1;
            this.Declaration2 = declaration2;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLocalDeclaration(this, o);
        }
    }
}
