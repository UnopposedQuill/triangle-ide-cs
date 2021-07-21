
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleRecordAggregate : RecordAggregate
    {
        private Identifier Identifier { get; set; }
        private Expression Expression { get; set; }

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
