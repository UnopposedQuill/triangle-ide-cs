
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Program : AST
    {
        private Command c;

        public Program(Command cAST, SourcePosition position) : base(position)
        {
            c = cAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProgram(this, o);
        }
    }
}
