
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IfCommand : Command
    {
        public Expression Expression { get; }
        public Command Command1 { get; }
        public Command Command2 { get; }

        public IfCommand(Expression e, Command command1, Command command2,
                SourcePosition position) : base(position)
        {
            Expression = e;
            Command1 = command1;
            Command2 = command2;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIfCommand(this, o);
        }
    }
}
