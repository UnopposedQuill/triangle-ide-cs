namespace TriangleCompiler.Triangle
{
    public class SourcePosition
    {
        public int start, finish;

        /**
         * This creates a dummy source position, used for some control elements
         */
        public SourcePosition()
        {
            start = 0;
            finish = 0;
        }

        /**
         * Creates a new source position based on paramenters
         */
        public SourcePosition(int start, int finish)
        {
            this.start = start;
            this.finish = finish;
        }

        public override string ToString()
        {
            return "(" + start + ", " + finish + ")";
        }
    }
}