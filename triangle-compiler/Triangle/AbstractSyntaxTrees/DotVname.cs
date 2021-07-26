
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class DotVname : Vname
    {
        public Identifier Identifier { get; }
        public Vname Vname { get; }

        public DotVname(Vname vAST, Identifier iAST, SourcePosition position)
                : base(position)
        {
            Vname = vAST;
            Identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitDotVname(this, o);
        }
    }
}
