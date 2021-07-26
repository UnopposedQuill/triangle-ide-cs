
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcActualParameter : ActualParameter
    {
        public Identifier Identifier { get; }

        public ProcActualParameter(Identifier iAST, SourcePosition position) : base(position)
        {
            Identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProcActualParameter(this, o);
        }
    }
}
