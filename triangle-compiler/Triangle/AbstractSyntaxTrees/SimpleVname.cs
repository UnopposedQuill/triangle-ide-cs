
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SimpleVname : Vname
    {
        private Identifier Identifier { get; set; }

        public SimpleVname(Identifier iAST, SourcePosition position) : base(position)
        {
            Identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSimpleVname(this, o);
        }
    }

    
}
