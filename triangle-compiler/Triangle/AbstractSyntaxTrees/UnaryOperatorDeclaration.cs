
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UnaryOperatorDeclaration : Declaration
    {
        public Operator Operator { get; }
        public TypeDenoter ArgumentTypeDenoter { get; }
        public TypeDenoter ResultTypeDenoter { get; }

        public UnaryOperatorDeclaration(Operator @operator, TypeDenoter argumentTypeDenoter, TypeDenoter resultTypeDenoter,
                SourcePosition position) : base(position)
        {
            Operator = @operator;
            ArgumentTypeDenoter = argumentTypeDenoter;
            ResultTypeDenoter = resultTypeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitUnaryOperatorDeclaration(this, o);
        }
    }
}
