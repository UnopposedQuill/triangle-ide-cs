
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Program : AST
    {
        public Command Command { get; }

        public Program(Command cAST, SourcePosition position) : base(position)
        {
            Command = cAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProgram(this, o);
        }
    }
}
