
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class AssignCommand : Command
    {
        public Vname Variable { get; }
        public Expression Expression { get; }

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
