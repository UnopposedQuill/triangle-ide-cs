using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    internal abstract class PendingCall
    {
        internal IdentificationTable CallContextIdentificationTable { get; }
        internal int Level { get; }

        internal PendingCall(IdentificationTable callContextIdentificationTable)
        {
            CallContextIdentificationTable = callContextIdentificationTable;
            Level = callContextIdentificationTable.Level;
        }

        internal abstract void VisitPendingCall(IVisitor v, object o);

        internal abstract Identifier GetProcFuncIdentifier();
    }
}