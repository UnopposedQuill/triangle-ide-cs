
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleArrayAggregate : ArrayAggregate
    {
        private Expression expression;
        private ArrayAggregate arrayAggregate;

        public MultipleArrayAggregate(Expression expression, ArrayAggregate arrayAggregate,
                SourcePosition position) : base(position)
        {
            this.expression = expression;
            this.arrayAggregate = arrayAggregate;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleArrayAggregate(this, o);
        }
    }
}
