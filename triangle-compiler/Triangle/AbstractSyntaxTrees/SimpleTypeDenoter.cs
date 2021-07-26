
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SimpleTypeDenoter : TypeDenoter
    {
        public Identifier Identifier { get; }

        public SimpleTypeDenoter(Identifier iAST, SourcePosition position)
                : base(position)
        {
            Identifier = iAST;
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
            return HashCode.Combine(base.GetHashCode(), Identifier);
        }
    }
}
