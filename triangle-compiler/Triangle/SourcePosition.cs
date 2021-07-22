namespace TriangleCompiler.Triangle
{
    public class SourcePosition
    {
        public int Start { set; get; }
        public int Finish { set; get; }

        /**
         * This creates a dummy source position, used for some control elements
         */
        public SourcePosition()
        {
            Start = 0;
            Finish = 0;
        }

        /**
         * Creates a new source position based on paramenters
         */
        public SourcePosition(int start, int finish)
        {
            Start = start;
            Finish = finish;
        }

        public override string ToString()
        {
            return "(" + Start + ", " + Finish + ")";
        }
    }
}