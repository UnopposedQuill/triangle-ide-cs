
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LetCommand : Command
    {
        public Declaration Declaration { get; }
        public Command Command { get; }

        public LetCommand(Declaration declaration, Command command,
                SourcePosition position) : base(position)
        {
            Declaration = declaration;
            Command = command;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLetCommand(this, o);
        }
    }
}
