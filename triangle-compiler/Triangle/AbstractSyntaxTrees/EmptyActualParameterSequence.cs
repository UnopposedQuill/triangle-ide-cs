
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class EmptyActualParameterSequence : ActualParameterSequence
    {
        public EmptyActualParameterSequence(SourcePosition position) : base(position)
        {

        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitEmptyActualParameterSequence(this, o);
        }
    }
}
