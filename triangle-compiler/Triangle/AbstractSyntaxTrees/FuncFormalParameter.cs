
using System;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncFormalParameter : FormalParameter
    {
        public Identifier Identifier { get; }
        public FormalParameterSequence FormalParameterSequence { get; }
        public TypeDenoter Type { get; internal set; }

        public FuncFormalParameter(Identifier identifier, FormalParameterSequence fpsAST,
                TypeDenoter tAST, SourcePosition position) : base(position)
        {
            this.Identifier = identifier;
            FormalParameterSequence = fpsAST;
            Type = tAST;
        }

        /**
         * Will return true if the parameters and the type of the parameters
         * are the same
         */
        public override bool Equals(object obj)
        {
            return obj is FuncFormalParameter ffp && FormalParameterSequence.Equals(ffp.FormalParameterSequence) && Type.Equals(ffp.Type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Identifier, FormalParameterSequence, Type);
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncFormalParameter(this, o);
        }
    }
}
