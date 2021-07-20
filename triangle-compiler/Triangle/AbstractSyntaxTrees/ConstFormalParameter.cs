
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstFormalParameter : FormalParameter
    {
        private Identifier identifier;
        private TypeDenoter t;

        public ConstFormalParameter(Identifier iAST, TypeDenoter tAST,
                SourcePosition position) : base(position)
        {
            identifier = iAST;
            t = tAST;
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
                return t.Equals(constFormalParameter.t);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
