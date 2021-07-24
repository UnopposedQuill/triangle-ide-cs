
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcFormalParameter : FormalParameter
    {
        public Identifier Identifier { get; }
        public FormalParameterSequence FormalParameterSequence { get; }

        public ProcFormalParameter(Identifier iAST, FormalParameterSequence fpsAST,
                SourcePosition position) : base(position)
        {
            Identifier = iAST;
            FormalParameterSequence = fpsAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitProcFormalParameter(this, o);
        }

        /**
         * This language will only check the signature for the function, regardless of the name
         * That means that two functions will different argument names will still be treated the same
         */
        public override bool Equals(object obj)
        {
            return obj is ProcFormalParameter parameter &&
                    FormalParameterSequence.Equals(parameter.FormalParameterSequence);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identifier, FormalParameterSequence);
        }
    }
}
