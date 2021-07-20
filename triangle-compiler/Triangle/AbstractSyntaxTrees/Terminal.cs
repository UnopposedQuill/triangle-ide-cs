namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Terminal : AST
    {
        private readonly string spelling;

        public Terminal(string spelling, SourcePosition position) : base(position)
        {

        }

        public string GetSpelling()
        {
            return spelling;
        }
    }
}