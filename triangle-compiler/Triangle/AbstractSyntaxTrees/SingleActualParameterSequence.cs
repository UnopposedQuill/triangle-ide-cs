
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleActualParameterSequence : ActualParameterSequence
    {
        private ActualParameter ActualParameter { get; set; }

        public SingleActualParameterSequence(ActualParameter apAST,
                SourcePosition position) : base(position)
        {
            ActualParameter = apAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSingleActualParameterSequence(this, o);
        }
    }
}
