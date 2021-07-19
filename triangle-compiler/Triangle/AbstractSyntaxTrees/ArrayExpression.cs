
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ArrayExpression : Expression
    {
        private ArrayAggregate aa;

        public ArrayExpression(ArrayAggregate aaAST, SourcePosition position) : base(position)
        {
            aa = aaAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitArrayExpression(this, o);
        }
    }
}
