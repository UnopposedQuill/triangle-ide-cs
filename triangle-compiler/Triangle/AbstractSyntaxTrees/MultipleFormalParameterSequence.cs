
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleFormalParameterSequence : FormalParameterSequence
    {
        public FormalParameter FormalParameter { get; }
        public FormalParameterSequence FormalParameterSequence { get; }

        public MultipleFormalParameterSequence(FormalParameter formalParameter, FormalParameterSequence parameterSequence,
                SourcePosition position) : base(position)
        {
            FormalParameter = formalParameter;
            FormalParameterSequence = parameterSequence;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleFormalParameterSequence(this, o);
        }

        public override bool Equals(object obj)
        {
            return obj is MultipleFormalParameterSequence sequence &&
                FormalParameter.Equals(sequence.FormalParameter) && FormalParameterSequence.Equals(sequence.FormalParameterSequence);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FormalParameter, FormalParameterSequence);
        }
    }
}
