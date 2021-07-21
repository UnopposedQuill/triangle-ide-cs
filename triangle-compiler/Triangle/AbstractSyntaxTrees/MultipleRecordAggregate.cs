
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleRecordAggregate : RecordAggregate
    {
        private Identifier identifier;
        private Expression expression;
        private RecordAggregate recordAggregate;

        public MultipleRecordAggregate(Identifier identifier, Expression expression,
                RecordAggregate recordAggregate, SourcePosition position) : base(position)
        {
            this.identifier = identifier;
            this.expression = expression;
            this.recordAggregate = recordAggregate;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleRecordAggregate(this, o);
        }
    }
}
