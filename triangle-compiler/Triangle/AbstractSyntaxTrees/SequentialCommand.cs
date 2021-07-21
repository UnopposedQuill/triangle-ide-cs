
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SequentialCommand : Command
    {
        private Command command1, command2;

        public SequentialCommand(Command command1AST, Command command2AST,
                SourcePosition position) : base(position)
        {
            command1 = command1AST;
            command2 = command2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSequentialCommand(this, o);
        }
    }
}
