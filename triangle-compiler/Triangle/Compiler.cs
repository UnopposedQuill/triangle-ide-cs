using System;

using TriangleCompiler.Triangle.AbstractSyntaxTrees;
using TriangleCompiler.Triangle.SyntacticAnalyzer;

namespace TriangleCompiler.Triangle
{
    class Compiler
    {
        //Filename for the object program, default is obj.tam
        public static string objectName = "obj.tam";

        private static ErrorReporter errorReporter;
        private static Scanner scanner;
        private static Parser parser;
        /*private static Checker checker;
        private static Encoder encoder;
        private static Drawer drawer;*/

        private static Program programAST;

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
        public static bool CompileProgram(string sourceName, string objectName,
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
                Scanner HTMLScanner = new Scanner(sourceFile);
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
            /*checker = new Checker(errorReporter);
            encoder = new Encoder(errorReporter);
            drawer = new Drawer();*/

            // scanner.enableDebugging();
            programAST = parser.ParseProgram();             // 1st pass
            /*if (errorReporter.getErrorCount() == 0)
            {
                //if (showingAST) {
                //    drawer.draw(programAST);
                //}

                Console.WriteLine("Contextual Analysis ...");
                checker.check(programAST);              // 2nd pass
                if (showingAST)
                {
                    drawer.draw(programAST);
                }
                if (errorReporter.getErrorCount() == 0)
                {
                    Console.WriteLine("Code Generation ...");
                    encoder.encodeRun(programAST, showingTable);    // 3rd pass
                }
            }*/

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

        /**
         * Triangle compiler main program.
         *
         * @param	args	the only command-line argument to the program specifies
         *                  the source filename.
         */
        public static int Main(string[] args)
        {
            bool compiledOK;

            Console.WriteLine(args.Length);
            foreach (string s in args)
            {
                Console.WriteLine(s);
            }

            //Only the program path was added on commands
            if (args.Length <= 0)
            {
                Console.WriteLine("Usage: tc filename");
                return 1;
            }

            String sourceName = args[0];
            compiledOK = CompileProgram(sourceName, objectName, false, false, false, false);

            return compiledOK ? 0 : 1;
        }
    }
}
