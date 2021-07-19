
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class BinaryExpression : Expression
    {
        private Expression e1, e2;
        private Operator o;

        public BinaryExpression(Expression e1AST, Operator oAST, Expression e2AST,
                SourcePosition position) : base(position)
        {
            o = oAST;
            e1 = e1AST;
            e2 = e2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitBinaryExpression(this, o);
        }
    }
}
