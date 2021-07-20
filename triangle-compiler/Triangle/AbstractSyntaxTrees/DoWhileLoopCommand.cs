
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class DoWhileLoopCommand : DoLoopCommand
    {
        public DoWhileLoopCommand(Expression eAST, Command cAST, SourcePosition position)
                : base(eAST, cAST, position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitDoWhileLoopCommand(this, o);
        }
    }
}
