
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncActualParameter : ActualParameter
    {
        public Identifier Identifier { get; }

        public FuncActualParameter(Identifier iAST, SourcePosition position) : base(position)
        {
            Identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncActualParameter(this, o);
        }
    }
}
