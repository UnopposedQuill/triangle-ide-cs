
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SimpleTypeDenoter : TypeDenoter
    {
        private Identifier identifier;

        public SimpleTypeDenoter(Identifier iAST, SourcePosition position)
                : base(position)
        {
            identifier = iAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSimpleTypeDenoter(this, o);
        }

        public override bool Equals(object obj)
        {
            return false; // Should not happen
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), identifier);
        }
    }
}
