

namespace TriangleCompiler.Triangle
{
    public class SourceFile : System.IDisposable
    {
        //These constants are used to properly control file reading
        public const char EOL = '\n';
        public const char CR = '\r';
        public const char EOT = '\u0000';

        private readonly System.IO.StreamReader source;
        private int currentLine;

        public SourceFile(string filename)
        {
            try
            {
                source = new System.IO.StreamReader(filename);
                currentLine = 1;
            }
            catch (System.IO.FileNotFoundException)
            {
                source = null;
                currentLine = 0;
            }
        }

        public char GetNextChar()
        {
            try
            {
                int c = source.Read();

                if (c == -1)
                {
                    c = EOT;
                }
                else if (c == EOL)
                {
                    currentLine++;
                }
                return (char) c;
            }
            catch (System.IO.IOException)
            {
                return EOT;
            }
        }

        public int GetCurrentLine()
        {
            return currentLine;
        }

        public void Dispose()
        {
            source.Dispose();
            System.GC.SuppressFinalize(this);
        }
    }
}
