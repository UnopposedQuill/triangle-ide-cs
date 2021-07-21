
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VarFormalParameter : FormalParameter
    {
        private Identifier Identifier { get; set; }
        private TypeDenoter TypeDenoter { get; set; }

        public VarFormalParameter(Identifier identifier, TypeDenoter typeDenoter,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitVarFormalParameter(this, o);
        }

        /**
         * Only the signature counts towards this
         * meaning the same may be different but the function will be the same
         */
        public override bool Equals(object obj)
        {
            return obj is VarFormalParameter parameter &&
                TypeDenoter.Equals(parameter.TypeDenoter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), Identifier, TypeDenoter);
        }
    }
}
