
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UnaryExpression : Expression
    {
        private Identifier Identifier { get; set; }
        private TypeDenoter TypeDenoter { get; set; }

        public UnaryExpression(Identifier identifier, TypeDenoter typeDenoter,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitUnaryExpression(this, o);
        }
    }
}
