
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ForLoopCommand : Command
    {
        private ConstDeclaration initialDeclaration;
        private Expression haltingExpression;
        private Command c;

        public ForLoopCommand(ConstDeclaration initialDeclaration,
                Expression haltingExpression, Command c, SourcePosition position)
                : base(position)

        {
            this.initialDeclaration = initialDeclaration;
            this.haltingExpression = haltingExpression;
            this.c = c;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitForLoopCommand(this, o);
        }

        public ConstDeclaration GetInitialDeclaration()
        {
            return initialDeclaration;
        }

        public void SetInitialDeclaration(ConstDeclaration constDeclaration)
        {
            initialDeclaration = constDeclaration;
        }

        public Expression GetHaltingExpression()
        {
            return haltingExpression;
        }

        public void SetHaltingExpression(Expression expression)
        {
            haltingExpression = expression;
        }

        public Command GetCommand()
        {
            return c;
        }

        public void SetCommand(Command command)
        {
            c = command;
        }
    }
}
