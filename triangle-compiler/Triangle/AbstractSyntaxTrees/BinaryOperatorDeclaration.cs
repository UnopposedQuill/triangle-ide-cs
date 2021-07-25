
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class BinaryOperatorDeclaration : Declaration
    {
        public Operator Operator { get; }
        public TypeDenoter Argument1Type { get; }
        public TypeDenoter Argument2Type { get; }
        public TypeDenoter ResultType { get;}

        public BinaryOperatorDeclaration(Operator oAST, TypeDenoter arg1TypeAST,
                TypeDenoter arg2TypeAST, TypeDenoter resultTypeAST,
                SourcePosition position) : base(position)
        {
            Operator = oAST;
            Argument1Type = arg1TypeAST;
            Argument2Type = arg2TypeAST;
            ResultType = resultTypeAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitBinaryOperatorDeclaration(this, o);
        }
    }
}
