
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    internal class PendingCallExpression : PendingCall
    {
        private readonly CallExpression callExpression;

        public PendingCallExpression(IdentificationTable callContextIdTable, CallExpression callExpression)
                : base(callContextIdTable)
        {
            this.callExpression = callExpression;
        }

        internal override void VisitPendingCall(IVisitor v, object o)
        {
            callExpression.Visit(v, o);
        }

        internal override Identifier GetProcFuncIdentifier()
        {
            return callExpression.Identifier;
        }
    }
}
