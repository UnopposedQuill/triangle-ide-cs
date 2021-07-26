
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ArrayTypeDenoter : TypeDenoter
    {
        public IntegerLiteral IntegerLiteral { get; }
        public TypeDenoter Type { get; internal set; }

        public ArrayTypeDenoter(IntegerLiteral ilAST, TypeDenoter tAST,
                        SourcePosition position) : base(position)
        {
            IntegerLiteral = ilAST;
            Type = tAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitArrayTypeDenoter(this, o);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(IntegerLiteral, Type);
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
                return IntegerLiteral.Spelling.CompareTo(((ArrayTypeDenoter)obj).IntegerLiteral.Spelling) == 0 &&
                                    Type.Equals(((ArrayTypeDenoter)obj).Type);
            }
            else
            {
                return false;
            }
        }
    }
}
