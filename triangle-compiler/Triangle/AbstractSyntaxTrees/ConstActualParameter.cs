
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstActualParameter : ActualParameter
    {
        private Expression e;

        public ConstActualParameter(Expression eAST, SourcePosition position)
                : base(position)
        {
            e = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitConstActualParameter(this, o);
        }
    }
}
