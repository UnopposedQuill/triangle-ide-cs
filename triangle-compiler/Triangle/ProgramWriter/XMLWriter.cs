
using System.IO;
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ProgramWriter
{
    class XMLWriter
    {
        private readonly Program programAST;

        public XMLWriter(Program programAST)
        {
            this.programAST = programAST;
        }

        public void WriteProgramAST(string sourceName)
        {
            Directory.CreateDirectory("output");
            FileStream xmlFileStream = File.Create("output/" + sourceName + ".xml");

            XMLWriterVisitor xmlWriterVisitor = new(xmlFileStream);
            programAST.Visit(xmlWriterVisitor, null);
        }
    }
}
