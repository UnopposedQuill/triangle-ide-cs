
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Command : AST
    {
        public Command(SourcePosition position) : base(position)
        {

        }

        public abstract override object Visit(IVisitor v, object o);
    }
}