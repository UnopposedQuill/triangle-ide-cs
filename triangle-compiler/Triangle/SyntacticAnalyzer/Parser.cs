
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.SyntacticAnalyzer
{
    class Parser
    {
        private readonly Scanner scanner;
        private readonly ErrorReporter errorReporter;
        private Token currentToken;
        private SourcePosition previousTokenPosition;

        /**
         * This will build a parser, in cohesion with a Lexical Scanner, both
         * which will work together into parsing the source file
         */
        public Parser(Scanner scanner, ErrorReporter errorReporter)
        {
            this.scanner = scanner;
            this.errorReporter = errorReporter;
            //I use a dummy position to begin the cycle
            previousTokenPosition = new SourcePosition();
        }


        #region Internal Methods

        /**
         * Checks the currentToken against expectedToken.
         * If matches, it will be taken and fetches next token.
         * Otherwise, reports a syntactic error.
         */
        private void Accept(int expectedToken)
        {
            if (currentToken.kind == expectedToken)
            {
                previousTokenPosition = currentToken.position;
                currentToken = scanner.Scan();
            }
            else
            {
                syntacticError("\"%\" expected here", Token.Spell(expectedToken));
            }
        }

        /**
         * Accepts currentToken with no conditions
         */
        private void AcceptIt()
        {
            previousTokenPosition = currentToken.position;
            currentToken = scanner.Scan();
        }

        /**
         * Records the start position of a phrase.
         * This is defined to be the position of the first
         * character of the first token of said phrase.
         */
        private void Start(SourcePosition position)
        {
            position.start = currentToken.position.start;
        }

        /**
         * Records the finish position of a phrase.
         * This is defined to be the position of the last
         * character of the last token of said phrase.
         */
        private void Finish(SourcePosition position)
        {
            position.finish = previousTokenPosition.finish;
        }

        private void syntacticError(string messageTemplate, string tokenQuoted)
        {
            SourcePosition position = currentToken.position;
            errorReporter.ReportError(messageTemplate, tokenQuoted, position);
            throw (new SyntaxError());
        }

        #endregion

        #region Programs
        
        /**
         * This will take care of setting the parser and scanner in motion.
         * Notice that this is the only public method of the parseX kind.
         */
        public Program ParseProgram()
        {
            Program programAST;

            previousTokenPosition.start = 0;
            previousTokenPosition.finish = 0;
            currentToken = scanner.Scan();

            try
            {
                Command cAST = ParseCommand();
                programAST = new Program(cAST, previousTokenPosition);
                if (currentToken.kind != Token.EOT)
                {
                    syntacticError("\"%\" not expected after end of program",
                        currentToken.spelling);
                }
            }
            catch (SyntaxError)
            {
                return null;
            }
            return programAST;
        }

        #endregion

        #region Commands

        /**
         * This will parse a command, note that commands may be compound.
         * Meaning that you may have several sequential commands.
         * As long as there are semicolons it will keep going to check
         * for further commands.
         */
        private Command ParseCommand()
        {
            Command commandAST;
            SourcePosition commandPosition = new();

            Start(commandPosition);
            commandAST = ParseSingleCommand();
            while (currentToken.kind == Token.SEMICOLON)
            {
                AcceptIt();
                Command command2AST = ParseSingleCommand();
                Finish(commandPosition);
                commandAST = new SequentialCommand(commandAST, command2AST, commandPosition);
            }
            return commandAST;
        }

        /**
         * Will parse a single command, that is to be returned into above
         * It will go through each type of commands to check which matches
         */
        private Command ParseSingleCommand()
        {
            Command commandAST = null;
            
            SourcePosition commandPosition = new();
            Start(commandPosition);

            switch (currentToken.kind)
            {
                case Token.IDENTIFIER:
                    {
                        Identifier iAST = ParseIdentifier();
                        if (currentToken.kind == Token.LPAREN)
                        {
                            AcceptIt();
                            ActualParameterSequence apsAST = ParseActualParameterSequence();
                            Accept(Token.RPAREN);
                            Finish(commandPosition);
                            commandAST = new CallCommand(iAST, apsAST, commandPosition);
                        }
                        else
                        {
                            Vname vAST = ParseRestOfVname(iAST);
                            Accept(Token.BECOMES);
                            Expression eAST = ParseExpression();
                            Finish(commandPosition);
                            commandAST = new AssignCommand(vAST, eAST, commandPosition);
                        }
                        break;
                    }
                case Token.LOOP:
                    {
                        //Loop token gets accepted, and we move to next token
                        AcceptIt();
                        commandAST = ParseLoopCommand();
                        Accept(Token.REPEAT);
                        break;
                    }
                case Token.LET:
                    {
                        AcceptIt();
                        Declaration dAST = ParseDeclaration();
                        Accept(Token.IN);
                        Command cAST = ParseCommand();
                        Finish(commandPosition);
                        Accept(Token.END);
                        commandAST = new LetCommand(dAST, cAST, commandPosition);
                        break;
                    }
                case Token.IF:
                    {
                        AcceptIt();
                        Expression eAST = ParseExpression();
                        Accept(Token.THEN);
                        Command command1AST = ParseCommand();
                        Accept(Token.ELSE);
                        Command command2AST = ParseCommand();
                        Finish(commandPosition);
                        Accept(Token.END);
                        commandAST = new IfCommand(eAST, command1AST, command2AST, commandPosition);
                        break;
                    }
                case Token.SKIP:
                    {
                        AcceptIt();
                        commandAST = new EmptyCommand(commandPosition);
                        break;
                    }
                default:
                    {
                        syntacticError("\"%\" cannot start a command",
                                currentToken.spelling);
                        break;
                    }
            }
            return commandAST;
        }
        
        /**
         * Will parse a loop command.
         * It will go through each type of loop to determine which loop.
         */
        private Command ParseLoopCommand()
        {
            Command commandAST = null;

            SourcePosition commandPosition = new();
            Start(commandPosition);

            switch (currentToken.kind) {
                case Token.WHILE:
                case Token.UNTIL:
                    {
                        //Loop kind will store the kind of loop the user has asked
                        int loopKind = currentToken.kind;
                        AcceptIt();
                        Expression eAST = ParseExpression();
                        Accept(Token.DO);
                        Command cAST = ParseCommand();
                        Finish(commandPosition);
                        commandAST = loopKind == Token.WHILE
                                ? new WhileLoopCommand(eAST, cAST, commandPosition)
                                : new UntilLoopCommand(eAST, cAST, commandPosition);
                        break;
                    }
                case Token.DO:
                    {
                        AcceptIt();
                        Command cAST = ParseCommand();
                        Finish(commandPosition);
                        if (!(currentToken.kind == Token.WHILE || currentToken.kind == Token.UNTIL)) {
                            syntacticError("Unexpected \"%\"", currentToken.spelling);
                        }
                        int loopKind = currentToken.kind;
                        AcceptIt();
                        Expression eAST = ParseExpression();
                        commandAST = loopKind == Token.WHILE
                                ? new DoWhileLoopCommand(eAST, cAST, commandPosition)
                                : new DoUntilLoopCommand(eAST, cAST, commandPosition);
                        break;
                    }
                case Token.FOR:
                    {
                        AcceptIt();
                        Identifier identifier = ParseIdentifier();
                        Accept(Token.IS);
                        Expression idenAST = ParseExpression();
                        ConstDeclaration initialDeclaration = new ConstDeclaration(identifier, idenAST, commandPosition);
                        Accept(Token.TO);
                        Expression eAST = ParseExpression();
                        Accept(Token.DO);
                        Command cAST = ParseCommand();
                        commandAST = new ForLoopCommand(initialDeclaration, eAST, cAST, commandPosition);
                        break;
                    }
                default:
                    {
                        syntacticError("Unexpected \"%\"", currentToken.spelling);
                        break;
                    }
            }
            return commandAST;
        }

        #endregion

        #region Literals

        // parseIntegerLiteral parses an integer-literal, and constructs
        // a leaf AST to represent it.
        private IntegerLiteral ParseIntegerLiteral()
        {
            IntegerLiteral IL;

            if (currentToken.kind == Token.INTLITERAL)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                IL = new IntegerLiteral(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            }
            else 
            {
                IL = null;
                syntacticError("integer literal expected here", "");
            }
            return IL;
        }

        // parseCharacterLiteral parses a character-literal, and constructs a leaf
        // AST to represent it.
        private CharacterLiteral parseCharacterLiteral()
        {
            CharacterLiteral CL;

            if (currentToken.kind == Token.CHARLITERAL)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                CL = new CharacterLiteral(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            } 
            else {
                CL = null;
                syntacticError("character literal expected here", "");
            }
            return CL;
        }

        // parseIdentifier parses an identifier, and constructs a leaf AST to
        // represent it.
        private Identifier parseIdentifier()
        {
            Identifier I;

            if (currentToken.kind == Token.IDENTIFIER)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                I = new Identifier(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            } 
            else 
            {
                I = null;
                syntacticError("identifier expected here", "");
            }
            return I;
        }

        // parseOperator parses an operator, and constructs a leaf AST to
        // represent it.
        private Operator parseOperator()
        {
            Operator O;

            if (currentToken.kind == Token.OPERATOR)
            {
                previousTokenPosition = currentToken.position;
                String spelling = currentToken.spelling;
                O = new Operator(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            } else {
                O = null;
                syntacticError("operator expected here", "");
            }
                return O;
        }

        #endregion

    }
}
