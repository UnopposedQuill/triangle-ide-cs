
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleRecordAggregate : RecordAggregate
    {
        public Identifier Identifier { get; }
        public Expression Expression { get; }

        public SingleRecordAggregate(Identifier identifier, Expression expression,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSingleRecordAggregate(this, o);
        }
    }
}
