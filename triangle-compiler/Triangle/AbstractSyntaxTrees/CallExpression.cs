
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CallExpression : Expression
    {
        internal Identifier Identifier { get; }
        internal ActualParameterSequence ActualParameterSequence { get; }

        public CallExpression(Identifier iAST, ActualParameterSequence apsAST,
                SourcePosition position) : base(position)
        {
            Identifier = iAST;
            ActualParameterSequence = apsAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCallExpression(this, o);
        }
    }
}
