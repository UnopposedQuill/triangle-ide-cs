
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class DoUntilLoopCommand : DoLoopCommand
    {
        public DoUntilLoopCommand(Expression eAST, Command cAST, SourcePosition position)
                : base(eAST, cAST, position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitDoUntilLoopCommand(this, o);
        }
    }
}
