
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    internal class PendingCallCommand : PendingCall
    {
        private readonly CallCommand callCommand;

        internal PendingCallCommand(IdentificationTable callContextIdentificationTable,
                CallCommand callCommand) : base(callContextIdentificationTable)
        {
            this.callCommand = callCommand;
        }

        internal override void VisitPendingCall(IVisitor v, object o)
        {
            callCommand.Visit(v, o);
        }

        internal override Identifier GetProcFuncIdentifier()
        {
            return callCommand.Identifier;
        }
    }
}
