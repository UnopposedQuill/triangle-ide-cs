
using System.IO;
using System.Text;

namespace TriangleCompiler.Triangle.ProgramWriter
{
    public class HTMLWriter
    {
        private readonly FileStream htmlFileStream;

        /**
         * This constructor will take care of writing the header data to a new
         * file in the output folder with the respective source name
         * Note that the rest of the file will be introduced using other methods
         * and FinishHTML is the one in care of closing the file
         */
        public HTMLWriter(string sourceName)
        {
            htmlFileStream = File.Create("output/" + sourceName + ".html");
            WriteToHTMLFile("<!DOCTYPE html>");
            WriteToHTMLFile("\n");
            WriteToHTMLFile("<html>");
            WriteToHTMLFile("\n");

            WriteToHTMLFile("<p style=\"font-family: monospace; font-size:160%;\">");
        }

        public void WriteKeyWord(string keyword)
        {
            WriteToHTMLFile("<b>" + keyword + "</b>");
        }

        public void WriteElse(string word)
        {
            WriteToHTMLFile(word);
        }

        public void WriteLiteral(string word)
        {
            WriteToHTMLFile("<span style=\"color:blue\">" + word + "</span>");
        }

        public void WriteComment(string comment)
        {
            WriteToHTMLFile("<span style=\"color:green\">" + comment + "</span><br>\n");
        }

        /**
         * This method will finish the HTML by adding the final header information
         * and then closing the stream file
         */
        public void FinishHTML()
        {
            WriteToHTMLFile("</p>" + "\n" +
                    "</html>");
            htmlFileStream.Close();
        }

        /**
         * This method will take care of writing the string value into the specified stream
         */
        private void WriteToHTMLFile(string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            htmlFileStream.Write(info, 0, info.Length);
        }
    }
}
