
namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    public class RecordTypeDenoter : TypeDenoter
    {
        private FieldTypeDenoter fieldTypeDenoter;
        
        public RecordTypeDenoter(FieldTypeDenoter ftAST, SourcePosition position) : base(position)
        {
            fieldTypeDenoter = ftAST;
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
                    fieldTypeDenoter.Equals(denoter.fieldTypeDenoter);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(base.GetHashCode(), fieldTypeDenoter);
        }
    }
}
