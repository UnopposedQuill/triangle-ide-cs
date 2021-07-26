
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecordExpression : Expression
    {
        public RecordAggregate RecordAggregate { get;}

        public RecordExpression(RecordAggregate raAST, SourcePosition position)
                : base(position)
        {
            RecordAggregate = raAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitRecordExpression(this, o);
        }
    }
}
