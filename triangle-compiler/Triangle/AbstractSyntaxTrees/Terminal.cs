namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public abstract class Terminal : AST
    {
        public string Spelling { get; }

        public Terminal(string spelling, SourcePosition position) : base(position)
        {
            this.Spelling = spelling;
        }
    }
}