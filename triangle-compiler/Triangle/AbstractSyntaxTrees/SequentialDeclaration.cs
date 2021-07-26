
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SequentialDeclaration : Declaration
    {
        public Declaration Declaration1 { get; }
        public Declaration Declaration2 { get; }

        public SequentialDeclaration(Declaration d1AST, Declaration d2AST,
                SourcePosition position) : base(position)
        {
            Declaration1 = d1AST;
            Declaration2 = d2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSequentialDeclaration(this, o);
        }
    }
}
