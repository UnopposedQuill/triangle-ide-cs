
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    /// <summary>
    /// This class stores the information of a call to a member that hasn't
    /// been declared yet, until it's either declared, or the compiling finishes
    /// </summary>
    internal class FutureCallExpression
    {
        /// <summary>
        /// The expected type for this call.
        /// </summary>
        internal TypeDenoter TypeDenoterToCheck { get; }

        /// <summary>
        /// The expression of the call
        /// </summary>
        internal Expression Expression { get; }

        ///<summary>
        ///Constructor for a call expresion with a future declaration
        ///</summary>
        internal FutureCallExpression(TypeDenoter typeDenoterToCheck, Expression expression)
        {
            TypeDenoterToCheck = typeDenoterToCheck;
            Expression = expression;
        }
    }
}