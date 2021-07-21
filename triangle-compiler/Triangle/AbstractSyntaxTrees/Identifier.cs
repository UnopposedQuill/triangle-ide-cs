
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class Identifier : Terminal
    {
        private TypeDenoter Type { get; set; }
        private AST Declaration { get; set; } // Either a Declaration or a FieldTypeDenoter

        public Identifier(string spelling, SourcePosition position) : base(spelling, position)
        {
            Type = null;
            Declaration = null;
        }

        public override bool Equals(object obj)
        {
            
            return obj is Identifier identifier && GetSpelling().Equals(identifier.GetSpelling());
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Type, Declaration);
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitIdentifier(this, o);
        }
    }
}