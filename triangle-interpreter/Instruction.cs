
using System.IO;

namespace TriangleInterpreter
{
    public class Instruction
    {
        // C# has no type synonyms, so the following representations are
        // assumed:
        //
        //  type
        //    OpCode = 0..15;  {4 bits unsigned}
        //    Length = 0..255;  {8 bits unsigned}
        //    Operand = -32767..+32767;  {16 bits signed}
        // Represents TAM instructions.
        public int op { get; set; } // OpCode
        public int r { get; set; }  // RegisterNumber
        public int n { get; set; }  // Length
        public int d { get; set; }  // Operand

        public Instruction()
        {
            op = 0;
            r = 0;
            n = 0;
            d = 0;
        }

        public void Write(StreamWriter output)
        {
            output.Write(op);
            output.Write(r);
            output.Write(n);
            output.Write(d);
        }

        public static Instruction Read(StreamReader input)
        {
            Instruction inst = new();
            try
            {
                inst.op = input.Read();
                inst.r = input.Read();
                inst.n = input.Read();
                inst.d = input.Read();
                return inst;
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
}
