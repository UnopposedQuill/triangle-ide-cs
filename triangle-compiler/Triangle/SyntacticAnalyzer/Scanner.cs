
namespace TriangleCompiler.Triangle.SyntacticAnalyzer
{
    public class Scanner
    {

        private readonly SourceFile sourceFile;
        private bool writingHTML;
        private ProgramWriter.HTMLWriter htmlWriter;

        private char currentChar;
        private string currentSpelling;
        private bool currentlyScanningToken;

        /*
         * Creates a new scanner capable of getting data from the specified source file
         */
        public Scanner(SourceFile source)
        {
            sourceFile = source;
            currentChar = sourceFile.GetNextChar();
            writingHTML = false;
        }

        /*
         * 
         */
        public Token Scan()
        {
            Token token;
            SourcePosition sourcePosition;
            int kind;

            currentlyScanningToken = false;
            while (currentChar is '!'
                   or ' '
                   or '\n'
                   or '\r'
                   or '\t')
            {
                ScanSeparator();
            }

            currentlyScanningToken = true;
            currentSpelling = "";
            sourcePosition = new SourcePosition
            {
                Start = sourceFile.GetCurrentLine()
            };

            kind = ScanToken();

            sourcePosition.Finish = sourceFile.GetCurrentLine();
            bool wasIdentifier = kind == Token.IDENTIFIER;
            token = new Token(kind, currentSpelling.ToString(), sourcePosition);
            if (writingHTML && wasIdentifier && token.kind != Token.IDENTIFIER)
                this.htmlWriter.WriteKeyWord(token.spelling);
            else if (writingHTML && wasIdentifier)
            {
                this.htmlWriter.WriteElse(token.spelling);
            }
            return token;
        }

        /*
         * Appends the current character to the current token spelling, and
         * then proceeds to the next character from the source file.
         */
        private void TakeIt()
        {
            if (currentlyScanningToken)
            {
                currentSpelling += currentChar;
            }
            currentChar = sourceFile.GetNextChar();
        }

        // Skips through separators
        private void ScanSeparator()
        {
            switch (currentChar)
            {
                case '!':
                    {
                        TakeIt();
                        string comment = "!";
                        while (currentChar is not SourceFile.EOL and not SourceFile.CR and not SourceFile.EOT)
                        {
                            comment += currentChar;
                            TakeIt();
                        }
                        if (currentChar == SourceFile.CR)
                        {
                            TakeIt();
                        }
                        if (currentChar == SourceFile.EOL)
                        {
                            TakeIt();
                        }
                        if (writingHTML)
                            htmlWriter.WriteComment(comment);
                    }
                    break;
                case '\n':
                    {
                        if (writingHTML)
                        {
                            htmlWriter.WriteElse("<br>\n");
                        }
                        break;
                    }
                case '\r':
                    {
                        TakeIt();
                        break;
                    }
                case ' ':
                case '\t':
                    if (writingHTML)
                    {
                        htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                    }
                    TakeIt();
                    break;
                default:
                    break;
            }
        }

        /**
         * Builds a token and returns the type of token it built
         */
        private int ScanToken()
        {
            switch (currentChar)
            {
                case 'a':  case 'b':  case 'c':  case 'd':  case 'e':
                case 'f':  case 'g':  case 'h':  case 'i':  case 'j':
                case 'k':  case 'l':  case 'm':  case 'n':  case 'o':
                case 'p':  case 'q':  case 'r':  case 's':  case 't':
                case 'u':  case 'v':  case 'w':  case 'x':  case 'y':
                case 'z':
                case 'A':  case 'B':  case 'C':  case 'D':  case 'E':
                case 'F':  case 'G':  case 'H':  case 'I':  case 'J':
                case 'K':  case 'L':  case 'M':  case 'N':  case 'O':
                case 'P':  case 'Q':  case 'R':  case 'S':  case 'T':
                case 'U':  case 'V':  case 'W':  case 'X':  case 'Y':
                case 'Z':
                    {
                        while (char.IsLetterOrDigit(currentChar))
                        {
                            TakeIt();
                        }
                        return Token.IDENTIFIER;
                    }
                case '0':  case '1':  case '2':  case '3':  case '4':
                case '5':  case '6':  case '7':  case '8':  case '9':
                    {
                        string intLiteral = System.Convert.ToString(currentChar);
                        TakeIt();
                        while (char.IsDigit(currentChar))
                        {
                            intLiteral += currentChar;
                            TakeIt();
                        }
                        if (writingHTML)
                            htmlWriter.WriteLiteral(intLiteral);
                        return Token.INTLITERAL;
                    }
                case '+':  case '-':  case '*': case '/':  case '=':
                case '<':  case '>':  case '\\':  case '&':  case '@':
                case '%':  case '^':  case '?':
                    {
                        string scanningOperator = System.Convert.ToString(currentChar);
                        TakeIt();
                        while (IsOperator(currentChar))
                        {
                            scanningOperator += currentChar;
                            TakeIt();
                        }
                        if (writingHTML)
                            htmlWriter.WriteElse(scanningOperator);
                        return Token.OPERATOR;
                    }
                case '\'':
                    {
                        TakeIt();
                        string characterLiteral = System.Convert.ToString(currentChar);
                        TakeIt(); // the quoted character
                        if (currentChar == '\'')
                        {
                            TakeIt();
                            if (writingHTML)
                                htmlWriter.WriteLiteral("\'" + characterLiteral + "\'");
                            return Token.CHARLITERAL;
                        }
                        else
                            return Token.ERROR;
                    }
                case '.':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.DOT;
                    }
                case ':':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        if (currentChar == '=')
                        {
                            if (writingHTML)
                                htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                            TakeIt();
                            return Token.BECOMES;
                        }
                        else
                            return Token.COLON;
                    }
                case ';':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.SEMICOLON;
                    }
                case ',':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.COMMA;
                    }
                case '~':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.IS;
                    }
                case '(':
                    {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.LPAREN;
                    }
                case ')':
                  {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.RPAREN;
                    }

                case '[':
                  {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.LBRACKET;
                    }

                case ']':
                  {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.RBRACKET;
                    }

                case '{':
                  {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.LCURLY;
                    }

                case '}':
                  {
                        if (writingHTML)
                            htmlWriter.WriteElse(System.Convert.ToString(currentChar));
                        TakeIt();
                        return Token.RCURLY;
                    }

                case SourceFile.EOT:
                    {
                        return Token.EOT;
                    }
                default: {
                    TakeIt();
                    return Token.ERROR;
                }
            }
        }

        public void HTMLRun(ProgramWriter.HTMLWriter htmlWriter)
        {
            writingHTML = true;
            this.htmlWriter = htmlWriter;
            Token token;
            do
            {
                token = Scan();
            } while (token.kind != Token.EOT);
            htmlWriter.FinishHTML();
        }

        // isOperator returns true iff the given character is an operator character.

        private static bool IsOperator(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/' ||
                c == '=' || c == '<' || c == '>' || c == '\\' ||
                c == '&' || c == '@' || c == '%' || c == '^' ||
                c == '?');
        }
    }
}
