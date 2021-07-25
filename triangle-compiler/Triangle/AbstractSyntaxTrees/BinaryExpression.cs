
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class BinaryExpression : Expression
    {
        public Expression Expression1 { get; }
        public Expression Expression2 {get;}
        public Operator Operator { get; }

        public BinaryExpression(Expression e1AST, Operator oAST, Expression e2AST,
                SourcePosition position) : base(position)
        {
            Operator = oAST;
            Expression1 = e1AST;
            Expression2 = e2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitBinaryExpression(this, o);
        }
    }
}
