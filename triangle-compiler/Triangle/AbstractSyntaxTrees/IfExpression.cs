
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IfExpression : Expression
    {
        /// <summary>
        /// Expression to be evaluated as condition, if true return Expression1
        /// if false return Expression2
        /// </summary>
        public Expression ConditionExpression { get; }

        /// <summary>
        /// Expression to be returned if condition is true
        /// </summary>
        public Expression Expression1 { get; }

        /// <summary>
        /// Expression to be returned if condition is false
        /// </summary>
        public Expression Expression2 { get; }

        public IfExpression(Expression expression1, Expression expression2, Expression expression3,
                SourcePosition position) : base(position)
        {
            ConditionExpression = expression1;
            Expression1 = expression2;
            Expression2 = expression3;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIfExpression(this, o);
        }
    }
}
