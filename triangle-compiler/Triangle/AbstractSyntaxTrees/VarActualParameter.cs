
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VarActualParameter : ActualParameter
    {
        private Vname Vname { get; set; }

        public VarActualParameter(Vname vname, SourcePosition position)
                : base(position)
        {
            Vname = vname;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitVarActualParameter(this, o);
        }
    }
}
