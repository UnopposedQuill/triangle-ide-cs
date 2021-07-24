
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class LoopCommand : Command
    {
        public Expression Expression;
        public Command Command;

        public LoopCommand(Expression expression, Command command,
                SourcePosition position) : base(position)
        {
            Expression = expression;
            Command = command;
        }
    }
}
