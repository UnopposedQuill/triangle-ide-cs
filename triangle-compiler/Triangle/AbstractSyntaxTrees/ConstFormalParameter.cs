
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstFormalParameter : FormalParameter
    {
        public Identifier Identifier { get; }
        public TypeDenoter Type { get; internal set; }

        public ConstFormalParameter(Identifier iAST, TypeDenoter tAST,
                SourcePosition position) : base(position)
        {
            Identifier = iAST;
            Type = tAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitConstFormalParameter(this, o);
        }

        /**
         * Checks if the formal parameter is of the same type, important
         * for parameter type checking in checker
         */
        public override bool Equals(object obj)
        {
            if (obj is ConstFormalParameter constFormalParameter)
            {
                return Type.Equals(constFormalParameter.Type);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
