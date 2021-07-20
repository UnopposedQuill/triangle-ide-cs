using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Identifier : Terminal
    {
        private TypeDenoter type;
        private AST decl; // Either a Declaration or a FieldTypeDenoter

        public Identifier(string spelling, TypeDenoter type, AST decl,
                SourcePosition position) : base(spelling, position)
        {
            this.type = type;
            this.decl = decl;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier identifier && GetSpelling().Equals(identifier.GetSpelling());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(type, decl);
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIdentifier(this, o);
        }

        public TypeDenoter GetTypeDenoter()
        {
            return type;
        }

        public AST GetDecl()
        {
            return decl;
        }
    }
}