
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    /**
     * This class works as a linked simple list of several identifiers
     * Point is that they will be stored in a linear fashion
     */
    internal class IdentifierEntry
    {
        internal string Id { get; }
        internal Declaration Attribute { get; }
        internal int Level { get; set; }
        internal IdentifierEntry PreviousEntry { get; set; }

        public IdentifierEntry(string id, Declaration attribute, int level, IdentifierEntry previousEntry)
        {
            Id = id;
            Attribute = attribute;
            Level = level;
            PreviousEntry = previousEntry;
        }
    }
}