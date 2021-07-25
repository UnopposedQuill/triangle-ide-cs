
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ArrayExpression : Expression
    {
        public ArrayAggregate ArrayAggregate { get; }

        public ArrayExpression(ArrayAggregate aaAST, SourcePosition position) : base(position)
        {
            ArrayAggregate = aaAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitArrayExpression(this, o);
        }
    }
}
