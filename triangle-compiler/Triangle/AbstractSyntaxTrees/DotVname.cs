
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class DotVname : Vname
    {
        private Identifier identifier;
        private Vname v;

        public DotVname(Vname vAST, Identifier iAST, SourcePosition position)
                : base(position)
        {
            v = vAST;
            identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitDotVname(this, o);
        }
    }
}
