
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcActualParameter : ActualParameter
    {
        private Identifier identifier;

        public ProcActualParameter(Identifier iAST, SourcePosition position) : base(position)
        {
            this.identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProcActualParameter(this, o);
        }
    }
}
