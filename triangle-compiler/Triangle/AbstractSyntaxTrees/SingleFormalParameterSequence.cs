
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleFormalParameterSequence : FormalParameterSequence
    {
        public FormalParameter FormalParameter { get; }

        public SingleFormalParameterSequence(FormalParameter formalParameter,
                SourcePosition position) : base(position)
        {
            FormalParameter = formalParameter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSingleFormalParameterSequence(this, o);
        }

        public override bool Equals(object obj)
        {
            return obj is SingleFormalParameterSequence sequence &&
                FormalParameter.Equals(sequence.FormalParameter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(FormalParameter);
        }
    }
}
