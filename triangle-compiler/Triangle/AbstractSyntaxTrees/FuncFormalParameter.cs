
using System;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class FuncFormalParameter : FormalParameter
    {
        private Identifier identifier;
        private FormalParameterSequence fps;
        private TypeDenoter type;

        public FuncFormalParameter(Identifier identifier, FormalParameterSequence fpsAST,
                TypeDenoter tAST, SourcePosition position) : base(position)
        {
            this.identifier = identifier;
            fps = fpsAST;
            type = tAST;
        }

        /**
         * Will return true if the parameters and the type of the parameters
         * are the same
         */
        public override bool Equals(object obj)
        {
            return obj is FuncFormalParameter ffp && fps.Equals(ffp.fps) && type.Equals(ffp.type);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(identifier, fps, type);
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitFuncFormalParameter(this, o);
        }
    }
}
