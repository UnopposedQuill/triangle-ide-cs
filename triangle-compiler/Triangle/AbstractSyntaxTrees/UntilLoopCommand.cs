
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UntilLoopCommand : LoopCommand
    {
        public UntilLoopCommand(Expression expression, Command command, SourcePosition position)
                : base(expression, command, position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitUntilLoopCommand(this, o);
        }
    }
}
