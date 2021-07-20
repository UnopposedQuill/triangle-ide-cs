
using System;
using System.Collections.Generic;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleFieldTypeDenoter : FieldTypeDenoter
    {
        private Identifier identifier;
        private TypeDenoter typeDenoter;
        private FieldTypeDenoter fieldTypeDenoter;

        public MultipleFieldTypeDenoter(Identifier identifier, TypeDenoter typeDenoter,
                FieldTypeDenoter fieldTypeDenoter, SourcePosition position) : base(position)
        {
            this.identifier = identifier;
            this.typeDenoter = typeDenoter;
            this.fieldTypeDenoter = fieldTypeDenoter;
        }

        public override bool Equals(object obj)
        {
            return obj is MultipleFieldTypeDenoter denoter &&
                   identifier.GetSpelling().Equals(denoter.identifier.GetSpelling()) &&
                   typeDenoter.Equals(denoter.typeDenoter) &&
                   fieldTypeDenoter.Equals(denoter.fieldTypeDenoter);;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(identifier, typeDenoter, fieldTypeDenoter);
        }

        public override object Visit(IVisitor v, object o)
        {
            throw new System.NotImplementedException();
        }
    }
}
