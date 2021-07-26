
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecordTypeDenoter : TypeDenoter
    {
        public FieldTypeDenoter FieldTypeDenoter { get; internal set; }
        
        public RecordTypeDenoter(FieldTypeDenoter ftAST, SourcePosition position) : base(position)
        {
            FieldTypeDenoter = ftAST;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitRecordTypeDenoter(this, o);
        }

        public override bool Equals(object obj)
        {
            //A bypass for error handling
            if (obj is ErrorTypeDenoter)
            {
                return true;
            }
            return obj is RecordTypeDenoter denoter &&
                    FieldTypeDenoter.Equals(denoter.FieldTypeDenoter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), FieldTypeDenoter);
        }
    }
}
