
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleFormalParameterSequence : FormalParameterSequence
    {
        private FormalParameter formalParameter;
        private FormalParameterSequence parameterSequence;

        public MultipleFormalParameterSequence(FormalParameter formalParameter, FormalParameterSequence parameterSequence,
                SourcePosition position) : base(position)
        {
            this.formalParameter = formalParameter;
            this.parameterSequence = parameterSequence;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleFormalParameterSequence(this, o);
        }

        public override bool Equals(object obj)
        {
            return obj is MultipleFormalParameterSequence sequence &&
                formalParameter.Equals(sequence.formalParameter) && parameterSequence.Equals(sequence.parameterSequence);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(formalParameter, parameterSequence);
        }
    }
}
