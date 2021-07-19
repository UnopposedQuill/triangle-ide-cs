
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class BinaryOperatorDeclaration : Declaration
    {
        private Operator o;
        private TypeDenoter arg1Type, arg2Type, resultType;

        public BinaryOperatorDeclaration(Operator oAST, AnyTypeDenoter arg1TypeAST,
                TypeDenoter arg2TypeAST, TypeDenoter resultTypeAST,
                SourcePosition position) : base(position)
        {
            o = oAST;
            arg1Type = arg1TypeAST;
            arg2Type = arg2TypeAST;
            resultType = resultTypeAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitBinaryOperatorDeclaration(this, o);
        }
    }
}
