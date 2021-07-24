
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcDeclaration : Declaration
    {
        public Identifier Identifier { get;}
        public FormalParameterSequence FormalParameterSequence { get; }
        public Command Command { get; }

        public ProcDeclaration(Identifier iAST, FormalParameterSequence fpsAST,
                Command cAST, SourcePosition position) : base(position)
        {
            Identifier = iAST;
            FormalParameterSequence = fpsAST;
            Command = cAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProcDeclaration(this, o);
        }
    }
}
