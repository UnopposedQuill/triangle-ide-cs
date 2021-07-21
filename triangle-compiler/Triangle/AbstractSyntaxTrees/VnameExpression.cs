
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VnameExpression : Expression
    {
        private Vname Vname { get; set; }

        public VnameExpression(Vname vname, SourcePosition position) : base(position)
        {
            Vname = vname;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitVnameExpression(this, o);
        }
    }
}
