namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class VarDeclaration : Declaration
    {
        public Identifier Identifier { get; }
        public TypeDenoter TypeDenoter { get; internal set; }

        public VarDeclaration(Identifier identifier, TypeDenoter typeDenoter,
                SourcePosition position) : base(position)
        {
            Identifier = identifier;
            TypeDenoter = typeDenoter;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitVarDeclaration(this, o);
        }
    }
}
