
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class EmptyCommand : Command
    {
        public EmptyCommand(SourcePosition position) : base(position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitEmptyCommand(this, o);
        }
    }
}
