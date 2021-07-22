

namespace TriangleCompiler.Triangle
{
    public class SourceFile
    {
        //These constants are used to properly control file reading
        public const char EOL = '\n';
        public const char CR = '\r';
        public const char EOT = '\u0000';

        private readonly System.IO.FileStream source;
        //private readonly System.IO.StreamReader source;
        private int currentLine;

        public SourceFile(string filename)
        {
            try
            {
                source = System.IO.File.OpenRead(filename);
                //source = new System.IO.StreamReader(filename);
                currentLine = 1;
            }
            catch (System.IO.IOException)
            {
                source = null;
                currentLine = 0;
            }
        }

        public char GetNextChar()
        {
            try
            {
                int c = source.ReadByte();

                //If the end has been reached, then it will return -1
                if (c == -1)
                {
                    c = EOT;
                    source.Close();//End of file, close stream
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
    }
}
