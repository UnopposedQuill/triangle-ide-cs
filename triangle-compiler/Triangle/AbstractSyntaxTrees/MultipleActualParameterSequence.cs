
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleActualParameterSequence : ActualParameterSequence
    {
        public ActualParameter ActualParameter { get; }
        public ActualParameterSequence ActualParameterSequence { get; }

        public MultipleActualParameterSequence(ActualParameter actualParameter, ActualParameterSequence parameterSequence,
                SourcePosition position) : base(position)
        {
            ActualParameter = actualParameter;
            ActualParameterSequence = parameterSequence;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleActualParameterSequence(this, o);
        }
    }
}
