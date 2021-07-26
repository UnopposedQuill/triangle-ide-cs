
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleRecordAggregate : RecordAggregate
    {
        public Identifier Identifier { get; }
        public Expression Expression { get; }
        public RecordAggregate RecordAggregate { get; }

        public MultipleRecordAggregate(Identifier identifier, Expression expression,
                RecordAggregate recordAggregate, SourcePosition position) : base(position)
        {
            Identifier = identifier;
            Expression = expression;
            RecordAggregate = recordAggregate;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleRecordAggregate(this, o);
        }
    }
}
