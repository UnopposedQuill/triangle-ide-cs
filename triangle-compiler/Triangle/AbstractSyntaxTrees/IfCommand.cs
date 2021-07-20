
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class IfCommand : Command
    {
        private Expression e;
        private Command command1, command2;

        public IfCommand(Expression e, Command command1, Command command2,
                SourcePosition position) : base(position)
        {
            this.e = e;
            this.command1 = command1;
            this.command2 = command2;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIfCommand(this, o);
        }
    }
}
