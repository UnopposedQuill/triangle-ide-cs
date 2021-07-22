
namespace TriangleCompiler.Triangle
{
    class ErrorReporter
    {
        private int errorCount;

        public ErrorReporter()
        {
            errorCount = 0;
        }

        public void ReportError(string message, string tokenName, SourcePosition position)
        {
            System.Console.WriteLine("ERROR: ");

            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == '%')
                {
                    System.Console.WriteLine(tokenName);
                }
                else
                {
                    System.Console.WriteLine(message[i]);
                }
            }
            System.Console.WriteLine(" " + position.Start + ".." + position.Finish);
            errorCount++;
        }

        public void ReportRestriction(string message)
        {
            System.Console.WriteLine("RESTRICTION: " + message);
        }

        public int getErrorCount()
        {
            return errorCount;
        }
    }
}
