
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Program : AST
    {
        private Command C;

        public Program(Command cAST, SourcePosition position) : base(position)
        {
            C = cAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            throw new System.NotImplementedException();
        }
    }
}
