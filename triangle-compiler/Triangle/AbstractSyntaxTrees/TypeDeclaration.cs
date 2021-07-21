
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class TypeDeclaration : Declaration
    {
        private Identifier Identifier { get; set; }
        private TypeDenoter TypeDenoter { get; set; }

        public TypeDeclaration(Identifier identifier, TypeDenoter typeDenoter,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitTypeDeclaration(this, o);
        }
    }
}
