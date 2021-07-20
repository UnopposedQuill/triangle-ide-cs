
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncActualParameter : ActualParameter
    {
        private Identifier identifier;

        public FuncActualParameter(Identifier iAST, SourcePosition position) : base(position)
        {
            identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncActualParameter(this, o);
        }
    }
}
