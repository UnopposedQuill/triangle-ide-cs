
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class LetCommand : Command
    {
        private Declaration declaration;
        private Command command;

        public LetCommand(Declaration declaration, Command command,
                SourcePosition position) : base(position)
        {
            this.declaration = declaration;
            this.command = command;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitLetCommand(this, o);
        }
    }
}
