
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncDeclaration : Declaration
    {
        private readonly Identifier identifier;
        private readonly FormalParameterSequence fps;
        private readonly TypeDenoter type;
        private readonly Expression expression;

        public FuncDeclaration(Identifier identifier, FormalParameterSequence fps,
                TypeDenoter type, Expression expression, SourcePosition position)
                : base(position)
        {
            this.identifier = identifier;
            this.fps = fps;
            this.type = type;
            this.expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncDeclaration(this, o);
        }

        public Identifier GetIdentifier()
        {
            return identifier;
        }

        public FormalParameterSequence GetFormalParameterSequence()
        {
            return fps;
        }

        public TypeDenoter GetTypeDenoter()
        {
            return type;
        }

        public Expression GetExpression()
        {
            return expression;
        }
    }
}
