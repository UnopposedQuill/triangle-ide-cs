
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class DoLoopCommand : LoopCommand
    {
        public DoLoopCommand(Expression eAST, Command cAST, SourcePosition position)
                : base(eAST, cAST, position)
        {

        }
    }
}
