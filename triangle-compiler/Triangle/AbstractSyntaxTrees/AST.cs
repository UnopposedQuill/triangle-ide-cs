
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class AST
    {
        private SourcePosition position;
        //public RuntimeEntity entity;

        public AST(SourcePosition position)
        {
            this.position = position;
        }

        public SourcePosition GetPosition()
        {
            return position;
            //entity = null;
        }

        /**
         * This method is the one that allows us to go through each of the ASTs
         * in a recursive manner
         */
        public abstract object Visit(IVisitor v, object o);
    }
}