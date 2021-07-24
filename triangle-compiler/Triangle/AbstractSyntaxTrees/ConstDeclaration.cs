
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class ConstDeclaration : Declaration
    {
        public Identifier Identifier { get;}
        public Expression Expression { get; }

        public ConstDeclaration(Identifier iAST, Expression eAST,
                SourcePosition position) : base(position)
        {
            Identifier = iAST;
            Expression = eAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitConstDeclaration(this, o);
        }
    }
}
