
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ForLoopCommand : Command
    {
        public ConstDeclaration InitialDeclaration { get; }
        public Expression HaltingExpression { get; }
        public Command Command { get; }

        public ForLoopCommand(ConstDeclaration initialDeclaration,
                Expression haltingExpression, Command c, SourcePosition position)
                : base(position)

        {
            InitialDeclaration = initialDeclaration;
            HaltingExpression = haltingExpression;
            Command = c;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitForLoopCommand(this, o);
        }
    }
}
