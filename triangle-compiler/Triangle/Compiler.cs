using System;

using TriangleCompiler.Triangle.AbstractSyntaxTrees;
using TriangleCompiler.Triangle.SyntacticAnalyzer;
using TriangleCompiler.Triangle.ContextualAnalyzer;
using TriangleCompiler.Triangle.ProgramWriter;
using TriangleCompiler.Triangle.CodeGenerator;

namespace TriangleCompiler.Triangle
{
    class Compiler
    {
        //Filename for the object program, default is obj.tam
        public static string objectName = "obj.tam";

        private ErrorReporter errorReporter;
        private Scanner scanner;
        private Parser parser;
        private Checker checker;
        private Encoder encoder;
        /*private Drawer drawer;*/

        public Program ProgramAST { get; private set; }

        /**
         * Compile the source program to TAM machine code.
         *
         * @param	sourceName	the name of the file containing the
         *				source program.
         * @param	objectName	the name of the file containing the
         *				object program.
         * @param	showingAST	true iff the AST is to be displayed after
         *				contextual analysis (not currently implemented).
         * @param	showingTable	true iff the object description details are to
         *				be displayed during code generation (not
         *				currently implemented).
         * @param   showingHTML     true iff the compiler is to be outputting
         *              the HTML from the source code.
         * @param   showingXML      true iff the compiler is to be outputting
         *              the XML from the source code.
         * @return	true iff the source program is free of compile-time errors,
         *          otherwise false.
         */
        public bool CompileProgram(string sourceName, string objectName,
                                       bool showingAST, bool showingTable,
                                        bool showingHTML, bool showingXML)
        {

            Console.WriteLine("********** " +
                               "Triangle Compiler (C# Version 1.0)" +
                               " **********");

            /* Before I do any compiling, I need to do a run for the HTML, which
            can be outputted regardless of Lexical correctness.
            The htmlRun method will make it run on its own until the file finishes
            */
            if (showingHTML)
            {
                Console.WriteLine("Lexical Analysis ...");
                SourceFile sourceFile = new(sourceName);
                Scanner HTMLScanner = new(sourceFile);
                HTMLScanner.HTMLRun(new ProgramWriter.HTMLWriter(sourceName[sourceName.LastIndexOf('/')..].Replace(".tri", "")));
            }
            
            //Now I proceed the compiling
            Console.WriteLine("Syntactic Analysis ...");

            /*
            if (source == null)
            {
                Console.WriteLine("Can't access source file " + sourceName);
                return false;
            }
            */

            SourceFile source = new(sourceName);
            scanner = new Scanner(source);
            errorReporter = new ErrorReporter();
            parser = new Parser(scanner, errorReporter);
            checker = new Checker(errorReporter);
            encoder = new Encoder(errorReporter);
            /*drawer = new Drawer();*/

            // scanner.enableDebugging();
            ProgramAST = parser.ParseProgram();             // 1st pass
            if (errorReporter.getErrorCount() == 0)
            {
                //if (showingAST) {
                //    drawer.draw(programAST);
                //}

                if (showingXML)
                {
                    WriteXMLProgram(ProgramAST, sourceName[sourceName.LastIndexOf('/')..].Replace(".tri", ""));
                }

                Console.WriteLine("Contextual Analysis ...");
                checker.Check(ProgramAST);              // 2nd pass
                
                /*if (showingAST)
                {
                    drawer.draw(programAST);
                }*/
                
                if (errorReporter.getErrorCount() == 0)
                {
                    Console.WriteLine("Code Generation ...");
                    encoder.encodeRun(ProgramAST, showingTable);    // 3rd pass
                }
            }

            bool successful = errorReporter.getErrorCount() == 0;
            if (successful)
            {
                //encoder.saveObjectProgram(objectName);
                Console.WriteLine("Compilation was successful.");
            }
            else
            {
                Console.WriteLine("Compilation was unsuccessful.");
            }
            return successful;
        }

        private static void WriteXMLProgram(Program programAST, String sourceName)
        {
            XMLWriter xmlWriter = new(programAST);

            xmlWriter.WriteProgramAST(sourceName);
        }

        /// <summary>
        /// Triangle compiler main program.
        /// </summary>
        /// <param name="args">The files to be compiled</param>
        /// <returns>
        /// 0 if all programs were compiled successfully. 1 if one or more failed. 2 if
        /// no file was specified. -1 if one of the files was not found, if one file was not
        /// found it will halt compiling.
        /// </returns>
        public static int Main(string[] args)
        {
            bool compiledOK = true;

            //Console.WriteLine(args.Length);

            //Only the program path was added on commands
            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: tc filename");
                return 2;
            }

            Compiler compiler = new();

            Console.WriteLine("Files to compile:");
            foreach (string s in args)
            {
                Console.WriteLine("\t-" + s);
                if (!System.IO.File.Exists(s))
                {
                    Console.WriteLine("Error: File: " + s + " doesn't exist");
                    return -1;
                }
                compiledOK = compiledOK && compiler.CompileProgram(s, objectName, false, false, true, true);
            }

            return compiledOK ? 0 : 1;
        }
    }
}
