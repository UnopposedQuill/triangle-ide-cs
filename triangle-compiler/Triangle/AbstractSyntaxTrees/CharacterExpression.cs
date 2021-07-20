
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CharacterExpression : Expression
    {
        private CharacterLiteral characterLiteral;

        public CharacterExpression(CharacterLiteral clAST, SourcePosition position)
            : base(position)
        {
            characterLiteral = clAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCharacterExpression(this, o);
        }
    }
}
