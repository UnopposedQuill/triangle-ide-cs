
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcDeclaration : Declaration
    {
        private Identifier identifier;
        private FormalParameterSequence formalParameterSequence;
        private Command command;

        public ProcDeclaration(Identifier iAST, FormalParameterSequence fpsAST,
                Command cAST, SourcePosition position) : base(position)
        {
            identifier = iAST;
            formalParameterSequence = fpsAST;
            command = cAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProcDeclaration(this, o);
        }
    }
}
