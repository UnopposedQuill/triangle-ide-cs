
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CallExpression : Expression
    {
        private Identifier identifier;
        private ActualParameterSequence aps;

        public CallExpression(Identifier iAST, ActualParameterSequence apsAST,
                SourcePosition position) : base(position)
        {
            identifier = iAST;
            aps = apsAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCallExpression(this, o);
        }
    }
}
