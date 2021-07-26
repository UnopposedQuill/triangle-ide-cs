
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncDeclaration : Declaration
    {
        public Identifier Identifier { get; }
        public FormalParameterSequence FormalParameterSequence { get; }
        public TypeDenoter Type { get; internal set; }
        public Expression Expression { get; }

        public FuncDeclaration(Identifier identifier, FormalParameterSequence fps,
                TypeDenoter type, Expression expression, SourcePosition position)
                : base(position)
        {
            Identifier = identifier;
            FormalParameterSequence = fps;
            Type = type;
            Expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncDeclaration(this, o);
        }
    }
}
