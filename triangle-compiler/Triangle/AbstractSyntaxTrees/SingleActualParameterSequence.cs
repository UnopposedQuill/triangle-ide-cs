
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleActualParameterSequence : ActualParameterSequence
    {
        public ActualParameter ActualParameter { get; }

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
