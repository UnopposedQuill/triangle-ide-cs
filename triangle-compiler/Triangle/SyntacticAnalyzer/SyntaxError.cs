using System;
using System.Runtime.Serialization;

namespace TriangleCompiler.Triangle.SyntacticAnalyzer
{
    [Serializable]
    internal class SyntaxError : Exception
    {
        public SyntaxError()
        {
        }

        public SyntaxError(string message) : base(message)
        {
        }

        public SyntaxError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SyntaxError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}