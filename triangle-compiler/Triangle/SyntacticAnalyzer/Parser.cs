
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
         * This will build a Parser, in cohesion with a Lexical Scanner, both
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
                SyntacticError("\"%\" expected here", Token.Spell(expectedToken));
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
         * Records the Start position of a phrase.
         * This is defined to be the position of the first
         * character of the first token of said phrase.
         */
        private void Start(SourcePosition position)
        {
            position.Start = currentToken.position.Start;
        }

        /**
         * Records the Finish position of a phrase.
         * This is defined to be the position of the last
         * character of the last token of said phrase.
         */
        private void Finish(SourcePosition position)
        {
            position.Finish = previousTokenPosition.Finish;
        }

        private void SyntacticError(string messageTemplate, string tokenQuoted)
        {
            SourcePosition position = currentToken.position;
            errorReporter.ReportError(messageTemplate, tokenQuoted, position);
            throw (new SyntaxError());
        }

        #endregion

        #region Programs
        
        /**
         * This will take care of setting the Parser and scanner in motion.
         * Notice that this is the only public method of the ParseX kind.
         */
        public Program ParseProgram()
        {
            Program programAST;

            previousTokenPosition.Start = 0;
            previousTokenPosition.Finish = 0;
            currentToken = scanner.Scan();

            try
            {
                Command cAST = ParseCommand();
                programAST = new Program(cAST, previousTokenPosition);
                if (currentToken.kind != Token.EOT)
                {
                    SyntacticError("\"%\" not expected after end of program",
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
         * This will Parse a command, note that commands may be compound.
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
         * Will Parse a single command, that is to be returned into above
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
                        //Loop token gets Accepted, and we move to next token
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
                        SyntacticError("\"%\" cannot Start a command",
                                currentToken.spelling);
                        break;
                    }
            }
            return commandAST;
        }
        
        /**
         * Will Parse a loop command.
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
                            SyntacticError("Unexpected \"%\"", currentToken.spelling);
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
                        SyntacticError("Unexpected \"%\"", currentToken.spelling);
                        break;
                    }
            }
            return commandAST;
        }

        #endregion

        #region Literals

        // ParseIntegerLiteral Parses an integer-literal, and constructs
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
                SyntacticError("integer literal expected here", "");
            }
            return IL;
        }

        // ParseCharacterLiteral Parses a character-literal, and constructs a leaf
        // AST to represent it.
        private CharacterLiteral ParseCharacterLiteral()
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
                SyntacticError("character literal expected here", "");
            }
            return CL;
        }

        // ParseIdentifier Parses an identifier, and constructs a leaf AST to
        // represent it.
        private Identifier ParseIdentifier()
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
                SyntacticError("identifier expected here", "");
            }
            return I;
        }

        // ParseOperator Parses an operator, and constructs a leaf AST to
        // represent it.
        private Operator ParseOperator()
        {
            Operator O;

            if (currentToken.kind == Token.OPERATOR)
            {
                previousTokenPosition = currentToken.position;
                string spelling = currentToken.spelling;
                O = new Operator(spelling, previousTokenPosition);
                currentToken = scanner.Scan();
            } else {
                O = null;
                SyntacticError("operator expected here", "");
            }
                return O;
        }

        #endregion

        #region Expressions

        private Expression ParseExpression()
        {
            Expression expressionAST;
            SourcePosition expressionPos = new();
            Start(expressionPos);

            switch (currentToken.kind)
            {
                case Token.LET: 
                {
                    AcceptIt();
                    Declaration dAST = ParseDeclaration();
                    Accept(Token.IN);
                    Expression eAST = ParseExpression();
                    Finish(expressionPos);
                    expressionAST = new LetExpression(dAST, eAST, expressionPos);
                        break;
                }
                case Token.IF:
                {
                    AcceptIt();
                    Expression e1AST = ParseExpression();
                    Accept(Token.THEN);
                    Expression e2AST = ParseExpression();
                    Accept(Token.ELSE);
                    Expression e3AST = ParseExpression();
                    Finish(expressionPos);
                    expressionAST = new IfExpression(e1AST, e2AST, e3AST, expressionPos);
                    break;
                }
                default:
                {
                    expressionAST = ParseSecondaryExpression();
                    break;
                }
            }
            return expressionAST;
        }

        private Expression ParseSecondaryExpression()
        {
            Expression expressionAST;

            SourcePosition expressionPos = new();
            Start(expressionPos);

            expressionAST = ParsePrimaryExpression();
            while (currentToken.kind == Token.OPERATOR)
            {
                Operator opAST = ParseOperator();
                Expression e2AST = ParsePrimaryExpression();
                expressionAST = new BinaryExpression(expressionAST, opAST, e2AST,
                        expressionPos);
            }
            return expressionAST;
        }

        private Expression ParsePrimaryExpression()
        {
            Expression expressionAST = null; // in case there's a syntactic error

            SourcePosition expressionPos = new();
            Start(expressionPos);

            switch (currentToken.kind)
            {
                case Token.INTLITERAL:
                    {
                        IntegerLiteral ilAST = ParseIntegerLiteral();
                        Finish(expressionPos);
                        expressionAST = new IntegerExpression(ilAST, expressionPos);
                        break;
                    }
                case Token.CHARLITERAL:
                    {
                        CharacterLiteral clAST = ParseCharacterLiteral();
                        Finish(expressionPos);
                        expressionAST = new CharacterExpression(clAST, expressionPos);
                        break;
                    }
                case Token.LBRACKET:
                    {
                        AcceptIt();
                        ArrayAggregate aaAST = ParseArrayAggregate();
                        Accept(Token.RBRACKET);
                        Finish(expressionPos);
                        expressionAST = new ArrayExpression(aaAST, expressionPos);
                        break;
                    }
                case Token.LCURLY:
                    {
                        AcceptIt();
                        RecordAggregate raAST = ParseRecordAggregate();
                        Accept(Token.RCURLY);
                        Finish(expressionPos);
                        expressionAST = new RecordExpression(raAST, expressionPos);
                        break;
                    }
                case Token.IDENTIFIER:
                    {
                        Identifier iAST = ParseIdentifier();
                        if (currentToken.kind == Token.LPAREN)
                        {
                            AcceptIt();
                            ActualParameterSequence apsAST = ParseActualParameterSequence();
                            Accept(Token.RPAREN);
                            Finish(expressionPos);
                            expressionAST = new CallExpression(iAST, apsAST, expressionPos);

                        }
                        else
                        {
                            Vname vAST = ParseRestOfVname(iAST);
                            Finish(expressionPos);
                            expressionAST = new VnameExpression(vAST, expressionPos);
                        }
                        break;
                    }
                case Token.OPERATOR:
                    {
                        Operator opAST = ParseOperator();
                        Expression eAST = ParsePrimaryExpression();
                        Finish(expressionPos);
                        expressionAST = new UnaryExpression(opAST, eAST, expressionPos);
                        break;
                    }
                case Token.LPAREN:
                    {
                        AcceptIt();
                        expressionAST = ParseExpression();
                        Accept(Token.RPAREN);
                        break;
                    }
                default:
                    {
                        SyntacticError("\"%\" cannot Start an expression",
                                currentToken.spelling);
                        break;
                    }

            }
            return expressionAST;
        }

        private RecordAggregate ParseRecordAggregate()
        {
        RecordAggregate aggregateAST;

            SourcePosition aggregatePos = new();
            Start(aggregatePos);

            Identifier iAST = ParseIdentifier();
            Accept(Token.IS);
            Expression eAST = ParseExpression();

            if (currentToken.kind == Token.COMMA)
            {
                AcceptIt();
                RecordAggregate aAST = ParseRecordAggregate();
                Finish(aggregatePos);
                aggregateAST = new MultipleRecordAggregate(iAST, eAST, aAST, aggregatePos);
            }
            else
            {
                Finish(aggregatePos);
                aggregateAST = new SingleRecordAggregate(iAST, eAST, aggregatePos);
            }
            return aggregateAST;
        }

        private ArrayAggregate ParseArrayAggregate()
        {
            ArrayAggregate aggregateAST;

            SourcePosition aggregatePos = new();
            Start(aggregatePos);

            Expression eAST = ParseExpression();
            if (currentToken.kind == Token.COMMA)
            {
                AcceptIt();
                ArrayAggregate aAST = ParseArrayAggregate();
                Finish(aggregatePos);
                aggregateAST = new MultipleArrayAggregate(eAST, aAST, aggregatePos);
            }
            else
            {
                Finish(aggregatePos);
                aggregateAST = new SingleArrayAggregate(eAST, aggregatePos);
            }
            return aggregateAST;
        }

        #endregion

        #region Value or Variables

        private Vname ParseVname()
        {
            Vname vnameAST;
            Identifier iAST = ParseIdentifier();
            vnameAST = ParseRestOfVname(iAST);
            return vnameAST;
        }

        Vname ParseRestOfVname(Identifier identifierAST)
        {
            SourcePosition vnamePos;
            vnamePos = identifierAST.Position;
            Vname vAST = new SimpleVname(identifierAST, vnamePos);

            while (currentToken.kind == Token.DOT || currentToken.kind == Token.LBRACKET) 
            {
                if (currentToken.kind == Token.DOT) 
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    vAST = new DotVname(vAST, iAST, vnamePos);
                }
                else
                {
                    AcceptIt();
                    Expression eAST = ParseExpression();
                    Accept(Token.RBRACKET);
                    Finish(vnamePos);
                    vAST = new SubscriptVname(vAST, eAST, vnamePos);
                }
            }
            return vAST;
        }

        #endregion

        #region Declarations

        private Declaration ParseCompoundDeclaration()
        {
            Declaration declarationAST = null;

            SourcePosition declarationPos = new SourcePosition();
            Start(declarationPos);
            switch (currentToken.kind)
            {
                case Token.RECURSIVE:
                    {
                        AcceptIt();
                        declarationAST = ParseProcFuncs();
                        Accept(Token.END);
                        Finish(declarationPos);
                        declarationAST = new RecursiveDeclaration(declarationAST, declarationPos);
                        break;
                    }
                case Token.LOCAL:
                    {
                        AcceptIt();
                        Declaration dAST1 = ParseDeclaration();
                        Accept(Token.IN);
                        Declaration dAST2 = ParseDeclaration();
                        Accept(Token.END);
                        Finish(declarationPos);
                        declarationAST = new LocalDeclaration(dAST1, dAST2, declarationPos);
                        break;
                    }
                case Token.CONST:
                case Token.VAR:
                case Token.TYPE:
                case Token.FUNC:
                case Token.PROC:
                    {
                        declarationAST = ParseSingleDeclaration();
                        break;
                    }
                default:
                    {
                        SyntacticError("\"%\" Cannot Start a Compound Declaration",
                                currentToken.spelling);
                        break;
                    }
            }
            return declarationAST;
        }

        //This method was modified to work with the new rule named ParseCompoundDeclaration, and with a recursive call.
        private Declaration ParseDeclaration()
        {
            Declaration declarationAST;

            SourcePosition declarationPos = new SourcePosition();
            Start(declarationPos);
            declarationAST = ParseCompoundDeclaration();
            while (currentToken.kind == Token.SEMICOLON)
            {
                AcceptIt();
                Declaration d2AST = ParseCompoundDeclaration();
                Finish(declarationPos);
                declarationAST = new SequentialDeclaration(declarationAST, d2AST,
                        declarationPos);
            }
            return declarationAST;
        }

        /**
         * This will check for ProcFuncs, it may return one or more
         */
        private Declaration ParseProcFuncs()
        {
            Declaration declarationAST = null; // in case there's a syntactic error

            SourcePosition declarationPos = new();
            Start(declarationPos);

            if (currentToken.kind == Token.PROC || currentToken.kind == Token.FUNC)
            {
                declarationAST = ParseSingleProcFunc();
                Finish(declarationPos);
            }
            else
            {
                SyntacticError("\"%\" not expected while parsing further proc-funcs, expected \"PROC\" or \"FUNC\"",
                        currentToken.spelling);
            }

            do
            {
                Accept(Token.AND);
                if (currentToken.kind == Token.PROC || currentToken.kind == Token.FUNC)
                {
                    Start(declarationPos);
                    Declaration dAST2 = ParseSingleProcFunc();
                    Finish(declarationPos);
                    declarationAST = new SequentialDeclaration(declarationAST, dAST2, declarationPos);
                }
                else
                {
                    SyntacticError("\"%\" not expected while parsing further proc-funcs, expected \"PROC\" or \"FUNC\"",
                            currentToken.spelling);
                }
            } while (currentToken.kind == Token.AND);
            return declarationAST;
        }

        /**
         * This will check for a ProcFunc
         * @return An AST representing a single ProcFunc
         * @throws SyntaxError If there is no procfunc or there was a wrong token
         */
        private Declaration ParseSingleProcFunc()
        {
            Declaration declarationAST = null; // in case there's a syntactic error

            SourcePosition declarationPos = new SourcePosition();
            Start(declarationPos);
            switch (currentToken.kind)
            {
                case Token.PROC:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.LPAREN);
                    FormalParameterSequence fpsAST = ParseFormalParameterSequence();

                    Accept(Token.RPAREN);
                    Accept(Token.IS);
                    Command cAST = ParseCommand();
                    Accept(Token.END);
                    Finish(declarationPos);
                    declarationAST = new ProcDeclaration(iAST, fpsAST, cAST, declarationPos);
                    break;
                }
                case Token.FUNC:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.LPAREN);
                    FormalParameterSequence fpsAST = ParseFormalParameterSequence();
                    Accept(Token.RPAREN);
                    Accept(Token.COLON);
                    TypeDenoter tAST = ParseTypeDenoter();
                    Accept(Token.IS);
                    Expression eAST = ParseExpression();
                    Finish(declarationPos);
                    declarationAST = new FuncDeclaration(iAST, fpsAST, tAST, eAST,
                            declarationPos);
                    break;
                }
            }
            return declarationAST;
        }

        private Declaration ParseSingleDeclaration()
        {
            Declaration declarationAST = null; // in case there's a syntactic error

            SourcePosition declarationPos = new SourcePosition();
            Start(declarationPos);

            switch (currentToken.kind)
            {

                case Token.CONST:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.IS);
                    Expression eAST = ParseExpression();
                    Finish(declarationPos);
                    declarationAST = new ConstDeclaration(iAST, eAST, declarationPos);
                    break;
                }
                case Token.VAR:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();

                    switch (currentToken.kind)
                    {
                        case Token.COLON:
                            {
                                AcceptIt();
                                TypeDenoter tAST = ParseTypeDenoter();
                                Finish(declarationPos);
                                declarationAST = new VarDeclaration(iAST, tAST, declarationPos);
                                break;
                            }
                        case Token.INIT:
                            {
                                AcceptIt();
                                Expression eAST = ParseExpression();
                                Finish(declarationPos);
                                declarationAST = new VarDeclarationInitialized(iAST, eAST, declarationPos);
                                break;
                            }
                    }
                    break;
                }
                case Token.TYPE:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.IS);
                    TypeDenoter tAST = ParseTypeDenoter();
                    Finish(declarationPos);
                    declarationAST = new TypeDeclaration(iAST, tAST, declarationPos);
                    break;
                }
                case Token.FUNC:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.LPAREN);
                    FormalParameterSequence fpsAST = ParseFormalParameterSequence();
                    Accept(Token.RPAREN);
                    Accept(Token.COLON);
                    TypeDenoter tAST = ParseTypeDenoter();
                    Accept(Token.IS);
                    Expression eAST = ParseExpression();
                    Finish(declarationPos);
                    declarationAST = new FuncDeclaration(iAST, fpsAST, tAST, eAST,
                            declarationPos);
                    break;
                }
                case Token.PROC:
                {
                    AcceptIt();
                    Identifier iAST = ParseIdentifier();
                    Accept(Token.LPAREN);
                    FormalParameterSequence fpsAST = ParseFormalParameterSequence();
                    Accept(Token.RPAREN);
                    Accept(Token.IS);
                    Command cAST = ParseCommand();
                    Accept(Token.END);
                    Finish(declarationPos);
                    declarationAST = new ProcDeclaration(iAST, fpsAST, cAST, declarationPos);
                    break;
                }
                default:
                {
                    SyntacticError("\"%\" cannot Start a declaration",
                            currentToken.spelling);
                    break;
                }
            }
            return declarationAST;
        }

        #endregion



    }
}
