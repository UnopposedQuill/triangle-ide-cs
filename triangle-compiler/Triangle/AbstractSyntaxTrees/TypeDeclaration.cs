
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class TypeDeclaration : Declaration
    {
        public Identifier Identifier { get; }
        public TypeDenoter TypeDenoter { get; internal set; }

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
