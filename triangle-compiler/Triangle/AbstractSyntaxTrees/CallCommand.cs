
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CallCommand : Command
    {
        private Identifier identifier;
        private ActualParameterSequence aps;

        public CallCommand(Identifier iAST, ActualParameterSequence apsAST,
                SourcePosition position) : base(position)
        {
            identifier = iAST;
            aps = apsAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCallCommand(this, o);
        }
    }
}
