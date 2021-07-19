

namespace TriangleCompiler.Triangle
{
    public class Token
    {
        public int kind;
        public string spelling;
        public SourcePosition position;

        public Token(int kind, string spelling, SourcePosition sourcePosition)
        {
            //If it comes as an identifier, I need to see if it is a reserved
            //word. It will go through all the types (see below)
            if (kind == Token.IDENTIFIER)
            {
                int currentKind = firstReservedWord;
                bool searching = true;

                while (searching)
                {
                    int comparison = tokenTable[currentKind].CompareTo(spelling);
                    if (comparison == 0)
                    {
                        this.kind = currentKind;
                        searching = false;
                    }
                    else if (comparison > 0 || currentKind == lastReservedWord)
                    {
                        this.kind = Token.IDENTIFIER;
                        searching = false;
                    }
                    else
                    {
                        currentKind++;
                    }
                }
            }
            else
            {
                this.kind = kind;
            }
            this.spelling = spelling;
            position = sourcePosition;
        }

        public static string Spell(int kind)
        {
            return tokenTable[kind];
        }

        public override string ToString()
        {
            return "Kind=" + kind + ", spelling=" + spelling + ", position=" + position;
        }

        //Token classes
        public const int

            // Literals, identifiers and operators
            INTLITERAL  = 0,
            CHARLITERAL = 1,
            IDENTIFIER  = 2,
            OPERATOR    = 3,

            // Reserved words - Note: MUST be in alphabetical order
            AND         = 4,
            ARRAY       = 5,
            CONST       = 6,
            DO          = 7,
            ELSE        = 8,
            END         = 9,
            FOR         = 10,
            FUNC        = 11,
            IF          = 12,
            IN          = 13,
            INIT        = 14,
            LET         = 15,
            LOCAL       = 16,
            LOOP        = 17,
            OF          = 18,
            PROC        = 19,
            RECORD      = 20,
            RECURSIVE   = 21,
            REPEAT      = 22,
            SKIP        = 23,
            THEN        = 24,
            TO          = 25,
            TYPE        = 26,
            UNTIL       = 27,
            VAR         = 28,
            WHILE       = 29,

            // Punctuation
            DOT         = 30,
            COLON       = 31,
            SEMICOLON   = 32,
            COMMA       = 33,
            BECOMES     = 34,
            IS          = 35,

            // Brackets
            LPAREN      = 36,
            RPAREN      = 37,
            LBRACKET    = 38,
            RBRACKET    = 39,
            LCURLY      = 40,
            RCURLY      = 41,

            // Special tokens
            EOT         = 42,
            ERROR       = 43;

        //Now the token table
        private static readonly string[] tokenTable = {
            "<int>",
            "<char>",
            "<identifier>",
            "<operator>",
            "and",
            "array",
            "const",
            "do",
            "else",
            "end",
            "for",
            "func",
            "if",
            "in",
            "init",
            "let",
            "local",
            "loop",
            "of",
            "proc",
            "record",
            "recursive",
            "repeat",
            "skip",
            "then",
            "to",
            "type",
            "until",
            "var",
            "while",
            ".",
            ":",
            ";",
            ",",
            ":=",
            "~",
            "(",
            ")",
            "[",
            "]",
            "{",
            "}",
            "<error>"
        };

        private const int firstReservedWord = Token.AND,
                          lastReservedWord  = Token.WHILE;
    }
}
