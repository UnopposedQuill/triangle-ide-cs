
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleActualParameterSequence : ActualParameterSequence
    {
        private ActualParameter actualParameter;
        private ActualParameterSequence parameterSequence;

        public MultipleActualParameterSequence(ActualParameter actualParameter, ActualParameterSequence parameterSequence,
                SourcePosition position) : base(position)
        {
            this.actualParameter = actualParameter;
            this.parameterSequence = parameterSequence;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleActualParameterSequence(this, o);
        }
    }
}
