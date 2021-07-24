
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CallCommand : Command
    {
        public Identifier Identifier { get; }
        public ActualParameterSequence ActualParameterSequence { get; }

        public CallCommand(Identifier iAST, ActualParameterSequence apsAST,
                SourcePosition position) : base(position)
        {
            Identifier = iAST;
            ActualParameterSequence = apsAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCallCommand(this, o);
        }
    }
}
