
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class AST
    {
        public SourcePosition Position { get; set; }
        //public RuntimeEntity entity;

        public AST(SourcePosition position)
        {
            Position = position;
        }

        /**
         * This method is the one that allows us to go through each of the ASTs
         * in a recursive manner
         */
        public abstract object Visit(IVisitor v, object o);
    }
}