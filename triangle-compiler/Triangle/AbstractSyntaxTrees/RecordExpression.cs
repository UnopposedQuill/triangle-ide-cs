
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecordExpression : Expression
    {
        private RecordAggregate recordAggregate;

        public RecordExpression(RecordAggregate raAST, SourcePosition position)
                : base(position)
        {
            recordAggregate = raAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitRecordExpression(this, o);
        }
    }
}
