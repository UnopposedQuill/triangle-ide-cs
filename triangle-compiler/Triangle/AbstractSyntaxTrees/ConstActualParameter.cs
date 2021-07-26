
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstActualParameter : ActualParameter
    {
        public Expression Expression { get; }

        public ConstActualParameter(Expression eAST, SourcePosition position)
                : base(position)
        {
            Expression = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitConstActualParameter(this, o);
        }
    }
}
