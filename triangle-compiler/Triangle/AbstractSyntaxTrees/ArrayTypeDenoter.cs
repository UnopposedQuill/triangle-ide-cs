
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ArrayTypeDenoter : TypeDenoter
    {
        private IntegerLiteral integerLiteral;
        private TypeDenoter t;

        public ArrayTypeDenoter(IntegerLiteral ilAST, TypeDenoter tAST,
                        SourcePosition position) : base(position)
        {
            integerLiteral = ilAST;
            t = tAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitArrayTypeDenoter(this, o);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(integerLiteral, t);
        }

        public override bool Equals(object obj)
        {
            //This is a bypass for error handling
            if (obj != null && obj is ErrorTypeDenoter)
            {
                return true;
            }
            if (obj != null && obj is ArrayTypeDenoter)
            {
                return integerLiteral.GetSpelling().CompareTo(((ArrayTypeDenoter)obj).integerLiteral.GetSpelling()) == 0 &&
                                    t.Equals(((ArrayTypeDenoter)obj).t);
            }
            else
            {
                return false;
            }
        }
    }
}
