
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class UnaryOperatorDeclaration : Declaration
    {
        private Operator Operator { get; set; }
        private TypeDenoter ArgumentTypeDenoter { get; set; }
        private TypeDenoter ResultTypeDenoter { get; set; }

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
