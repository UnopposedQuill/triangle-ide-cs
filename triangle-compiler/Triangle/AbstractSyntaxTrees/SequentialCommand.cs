
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SequentialCommand : Command
    {
        public Command Command1 { get; }
        public Command Command2 { get; }

        public SequentialCommand(Command command1AST, Command command2AST,
                SourcePosition position) : base(position)
        {
            Command1 = command1AST;
            Command2 = command2AST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSequentialCommand(this, o);
        }
    }
}
