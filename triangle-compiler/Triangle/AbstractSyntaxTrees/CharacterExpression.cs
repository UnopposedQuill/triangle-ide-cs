
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CharacterExpression : Expression
    {
        public CharacterLiteral CharacterLiteral { get; }

        public CharacterExpression(CharacterLiteral clAST, SourcePosition position)
            : base(position)
        {
            CharacterLiteral = clAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCharacterExpression(this, o);
        }
    }
}
