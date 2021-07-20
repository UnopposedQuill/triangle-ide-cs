
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class LoopCommand : Command
    {
        private Expression expression;
        private Command command;

        public LoopCommand(Expression expression, Command command,
                SourcePosition position) : base(position)
        {
            this.expression = expression;
            this.command = command;
        }
    }
}
