
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VnameExpression : Expression
    {
        public Vname Vname { get; }

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
