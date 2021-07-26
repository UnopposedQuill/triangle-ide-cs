
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleFieldTypeDenoter : FieldTypeDenoter
    {
        public Identifier Identifier { get; }
        public TypeDenoter TypeDenoter { get; internal set; }

        public SingleFieldTypeDenoter(Identifier identifier, TypeDenoter typeDenoter,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitSingleFieldTypeDenoter(this, o);
        }

        public override bool Equals(object obj)
        {
            return obj is SingleFieldTypeDenoter denoter &&
                    Identifier.Spelling.Equals(denoter.Identifier.Spelling) &&
                    TypeDenoter.Equals(denoter.TypeDenoter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), Identifier, TypeDenoter);
        }
    }
}
