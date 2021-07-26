
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SubscriptVname : Vname
    {
        public Expression Expression { get; }
        public Vname Vname { get; }

        public SubscriptVname(Vname vname, Expression expression,
                SourcePosition position) : base(position)
        {
            Expression = expression;
            Vname = vname;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSubscriptVname(this, o);
        }
    }
}
