
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleArrayAggregate : ArrayAggregate
    {
        public Expression Expression { get; }
        public ArrayAggregate ArrayAggregate { get; }

        public MultipleArrayAggregate(Expression expression, ArrayAggregate arrayAggregate,
                SourcePosition position) : base(position)
        {
            Expression = expression;
            ArrayAggregate = arrayAggregate;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleArrayAggregate(this, o);
        }
    }
}
