using System.Collections.Generic;
using TriangleCompiler.Triangle.AbstractSyntaxTrees;

namespace TriangleCompiler.Triangle.ContextualAnalyzer
{
    public class IdentificationTable
    {
        internal int Level { get; set; }
        internal int RecursiveLevel { get; set; }
        private IdentifierEntry LastestEntry { get; set; }

        internal List<PendingCall> PendingCalls { get; }
        internal List<FutureCallExpression> FutureCallExpressions { get; }

        internal IdentificationTable()
        {
            Level = RecursiveLevel = 0;
            LastestEntry = null;
            PendingCalls = new List<PendingCall>();
            FutureCallExpressions = new List<FutureCallExpression>();
        }

        internal IdentificationTable(IdentificationTable oldTable)
        {
            Level = oldTable.Level;
            LastestEntry = oldTable.LastestEntry;
            FutureCallExpressions = oldTable.FutureCallExpressions;
        }

        /**
         * Opens a new level in the identification table, 1 higher than the
         * current topmost level.
         */
        internal void OpenScope()
        {
            Level++;
        }

        /**
         * Closes the topmost level in the identification table, discarding
         * all entries belonging to that level.
         */
        internal void CloseScope()
        {
            //Presumably, IdentificationTable.Level > 0
            IdentifierEntry entry = LastestEntry;

            while (entry.Level == Level)
            {
                entry = entry.PreviousEntry;
            }
            //"entry" got to an entry whose level is lower
            LastestEntry = entry;
            Level--;
        }

        internal void CloseLocalScope()
        {
            IdentifierEntry entry = LastestEntry, local = entry, localEntry;

            // Presumably, idTable.level > 1.
            // First, I need to point local towards the first declaration in this scope
            while (entry.Level == Level)
            {
                local = entry;
                local.Level -= 2;
                entry = local.PreviousEntry;
            }

            //Now, I need to skip all the entries belonging to the previous scope (local variables' scope)
            while (entry.Level == Level - 1)
            {
                localEntry = entry;
                entry = localEntry.PreviousEntry;
            }

            //Now I anchor the entries I defined by using local declarations to the level they were in
            local.PreviousEntry = entry;

            //And submit the changes in the scope level
            Level -= 2;
        }

        public void OpenRecursiveScope()
        {
            RecursiveLevel++;
        }

        public void CloseRecursiveScope()
        {
            RecursiveLevel--;
        }

        /**
         * Makes a new entry in this identification table for the given identifier
         * and attribute. New entry will be place on the current level. It will be
         * flagged as "duplicated" true iff there is an entry of the same identifier
         * in the same level.
         */
        internal void Enter(string id, Declaration attribute)
        {
            bool present = false;
            IdentifierEntry entry = LastestEntry;

            //Check for duplicate entry
            while (entry != null && entry.Level >= Level)
            {
                if (entry.Id.Equals(id))
                {
                    present = true;
                    break;
                }
                entry = entry.PreviousEntry;
            }

            //Process duplication, and then add
            attribute.Duplicated = present;
            LastestEntry = new IdentifierEntry(id, attribute, Level, LastestEntry);
        }

        /**
         * Finds an entry for the given identifier in the identification table,
         * if any. If there are several entries for that identifier, finds the
         * entry at the highest level, in accordance with the scope rules.
         * Returns null iff no entry is found.
         */
        internal Declaration Retrieve(string id)
        {
            IdentifierEntry entry = LastestEntry;

            while (entry != null)
            {
                if (entry.Id.Equals(id))//Found the entry
                {
                    return entry.Attribute;
                }
                entry = entry.PreviousEntry;
            }
            //No more entries to search, didn't find
            return null;
        }

        /**
         * Finds an entry for the given identifier in the identification table,
         * if any. If there are several entries for that identifier, finds the
         * entry at the highest level, in accordance with the scope rules.
         * Returns null iff no entry is found.
         * otherwise returns the attribute field of the entry found. 
         * This method is capable of filtering types of Declarations, it is used 
         * when the same operator is defined for both Unary and Binary Expressions
         */
        internal Declaration Retrieve(string id, System.Type declarationClass)
        {
            IdentifierEntry entry = LastestEntry;

            while (entry != null)
            {
                //Both the type of declaration and identificator have to match
                if (entry.Attribute.GetType() == declarationClass && entry.Id.Equals(id))
                {
                    return entry.Attribute;
                }
                entry = entry.PreviousEntry;
            }
            //Didn't find a match
            return null;
        }

        internal void AddPendingCall(PendingCall pendingCall)
        {
            PendingCalls.Add(pendingCall);
        }

        internal void AddFutureCallExpression(FutureCallExpression futureCallExpression)
        {
            FutureCallExpressions.Add(futureCallExpression);
        }

        /**
         * Once a new function has been declared, this method will be called to patch
         * the future calls to the specified identifier.
         */
        internal List<PendingCall> CheckPendingCalls(Identifier identifier)
        {

            List<PendingCall> toVisit = new();
            int declarationLevel = Level - 1;

            foreach (PendingCall pendingCall in PendingCalls)
            {
                // If the level is greater, and the identifier matches, I need to visit it
                if (pendingCall.Level > declarationLevel && pendingCall.GetProcFuncIdentifier().Equals(identifier))
                {
                    toVisit.Add(pendingCall);
                    //@TODO: Check if the next line doesn't interfere
                    PendingCalls.Remove(pendingCall);
                }
            }

            return toVisit;
        }
    }
}