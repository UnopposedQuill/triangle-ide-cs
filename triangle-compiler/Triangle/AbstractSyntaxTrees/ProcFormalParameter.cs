
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ProcFormalParameter : FormalParameter
    {
        private Identifier identifier;
        private FormalParameterSequence formalParameterSequence;

        public ProcFormalParameter(Identifier iAST, FormalParameterSequence fpsAST,
                SourcePosition position) : base(position)
        {
            identifier = iAST;
            formalParameterSequence = fpsAST;
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
                    formalParameterSequence.Equals(parameter.formalParameterSequence);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(identifier, formalParameterSequence);
        }
    }
}
