
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

        public override bool Equals(object obj)
        {
            //This is a bypass for error handling
            if (obj != null && obj is ErrorTypeDenoter)
            {
                return true;
            }
            if (obj != null && obj is ArrayTypeDenoter)
            {
                return integerLiteral.spelling.compareTo(((ArrayTypeDenoter)obj).integerLiteral.spelling) == 0 &&
                                    t.equals(((ArrayTypeDenoter)obj).t);
            }
            else
            {
                return false;
            }
        }
    }
}
