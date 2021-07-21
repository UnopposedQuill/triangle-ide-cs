
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class SingleFieldTypeDenoter : FieldTypeDenoter
    {
        private Identifier Identifier { get; set; }
        private TypeDenoter TypeDenoter { get; set; }

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
                    Identifier.GetSpelling().Equals(denoter.Identifier.GetSpelling()) &&
                    TypeDenoter.Equals(denoter.TypeDenoter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), Identifier, TypeDenoter);
        }
    }
}
