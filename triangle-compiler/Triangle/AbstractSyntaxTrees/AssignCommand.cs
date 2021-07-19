
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class AssignCommand : Command
    {
        private Vname v;
        public Expression e;

        public AssignCommand(Vname vAST, Expression eAST, SourcePosition position) : base(position)
        {
            v = vAST;
            e = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitAssignCommand(this, o);
        }
    }
}
