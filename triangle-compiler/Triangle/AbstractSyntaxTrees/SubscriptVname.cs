
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SubscriptVname : Vname
    {
        private Expression Expression { get; set; }
        private Vname Vname { get; set; }

        public SubscriptVname(Expression expression, Vname vname,
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
