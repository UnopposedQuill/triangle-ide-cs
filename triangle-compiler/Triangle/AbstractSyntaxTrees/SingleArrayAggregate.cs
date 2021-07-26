
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleArrayAggregate : ArrayAggregate
    {
        public Expression Expression{ get; }

        public SingleArrayAggregate(Expression eAST, SourcePosition position)
                : base(position)
        {
            Expression = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSingleArrayAggregate(this, o);
        }
    }
}
