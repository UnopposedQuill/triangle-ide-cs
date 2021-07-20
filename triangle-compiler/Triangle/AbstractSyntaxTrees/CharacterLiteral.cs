
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class CharacterLiteral : Terminal
    {
        public CharacterLiteral(string spelling, SourcePosition position)
                : base(spelling, position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitCharacterLiteral(this, o);
        }
    }
}
