using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    /**
     * This class features small ASTs representing standard types or standard
     * declarations
     */
    internal class StdEnvironment
    {
        public static TypeDenoter
            booleanType, charType, integerType, anyType, errorType;

        public static TypeDeclaration
            booleanDecl, charDecl, integerDecl;

        // These are small ASTs representing "declarations" of standard entities.

        public static ConstDeclaration
            falseDecl, trueDecl, maxintDecl;

        public static UnaryOperatorDeclaration
            notDecl, negDecl;

        public static BinaryOperatorDeclaration
            andDecl, orDecl, addDecl, subtractDecl, multiplyDecl, divideDecl, moduloDecl,
            equalDecl, unequalDecl, lessDecl, notlessDecl, greaterDecl, notgreaterDecl;

        public static ProcDeclaration
            getDecl, putDecl, getintDecl, putintDecl, geteolDecl, puteolDecl, indexCheck;

        public static FuncDeclaration
            chrDecl, ordDecl, eolDecl, eofDecl;
    }
}