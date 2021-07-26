
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class MultipleFieldTypeDenoter : FieldTypeDenoter
    {
        public Identifier Identifier { get; }
        public TypeDenoter TypeDenoter { get; internal set; }
        public FieldTypeDenoter FieldTypeDenoter { get; }

        public MultipleFieldTypeDenoter(Identifier identifier, TypeDenoter typeDenoter,
                FieldTypeDenoter fieldTypeDenoter, SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
            FieldTypeDenoter = fieldTypeDenoter;
        }

        public override bool Equals(object obj)
        {
            return obj is MultipleFieldTypeDenoter denoter &&
                   Identifier.Spelling.Equals(denoter.Identifier.Spelling) &&
                   TypeDenoter.Equals(denoter.TypeDenoter) &&
                   FieldTypeDenoter.Equals(denoter.FieldTypeDenoter);;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), Identifier, TypeDenoter, FieldTypeDenoter);
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleFieldTypeDenoter(this, o);
        }
    }
}
