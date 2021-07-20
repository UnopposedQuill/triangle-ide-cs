using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleCompiler.Triangle.AbstractSyntaxTrees
{
    class MultipleArrayAggregate : ArrayAggregate
    {
        private Expression expression;
        private ArrayAggregate arrayAggregate;

        public MultipleArrayAggregate(Expression expression, ArrayAggregate arrayAggregate,
                SourcePosition position) : base(position)
        {
            this.expression = expression;
            this.arrayAggregate = arrayAggregate;
        }

        public override object Visit(IVisitor v, object o)
        {
            return v.VisitMultipleArrayAggregate(this, o);
        }
    }
}
