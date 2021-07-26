
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class AssignCommand : Command
    {
        public Vname Vname { get; }
        public Expression Expression { get; }

        public AssignCommand(Vname vAST, Expression eAST, SourcePosition position) : base(position)
        {
            Vname = vAST;
            Expression = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitAssignCommand(this, o);
        }
    }
}
