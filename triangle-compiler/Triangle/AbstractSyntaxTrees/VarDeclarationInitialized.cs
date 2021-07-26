
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VarDeclarationInitialized : VarDeclaration
    {
        public Expression Expression { get; }

        /**
         * This will create a new instance for an Initialized Variable Declaration
         * Notice that the type will be inferred according to the expression type,
         * and then the variable will be bound to that type
         */
        public VarDeclarationInitialized(Identifier iAST, Expression expression, SourcePosition position)
                : base(iAST, null, position)
        {
            Expression = expression;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitVarDeclarationInitialized(this, o);
        }
    }
}
