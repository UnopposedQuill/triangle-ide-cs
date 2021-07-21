
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class WhileLoopCommand : LoopCommand
    {
        public WhileLoopCommand(Expression expression, Command command, SourcePosition position) : base(expression, command, position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitWhileLoopCommand(this, o);
        }
    }
}
