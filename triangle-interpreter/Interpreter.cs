using System;

namespace TriangleInterpreter
{
    public class Interpreter
    {
        private String objectName;
        public static int CurrentChar;

        #region Data Store

        private static readonly int[] data = new int[1024];

        // DATA STORE REGISTERS AND OTHER REGISTERS
        private static int CB = 0,
                           SB = 0,
                           HB = 1024; // = upper bound of data array + 1

        private static int CT, CP, ST, HT, LB, status;

        // status values
        private const int RUNNING = 0, HALTED = 1, FAILEDDATASTOREFULL = 2, FAILEDINVALIDCODEADDRESS = 3,
                FAILEDINVALIDINSTRUCTION = 4, FAILEDOVERFLOW = 5, FAILEDZERODIVIDE = 6,
                FAILEDIOERROR = 7, FAILEDARRAYINDEXOUTOFBOUNDS = 8;

        private static long accumulator;

        //Returns the current content of register r,
        //even if r is onhe of the pseudo-registers L1..L6
        public int Content(int r)
        {
            return r switch
            {
                Machine.CBr => CB,
                Machine.CTr => CT,
                Machine.PBr => Machine.PB,
                Machine.PTr => Machine.PT,
                Machine.SBr => SB,
                Machine.STr => ST,
                Machine.HBr => HB,
                Machine.HTr => HT,
                Machine.LBr => LB,
                Machine.L1r => data[LB],
                Machine.L2r => data[data[LB]],
                Machine.L3r => data[data[data[LB]]],
                Machine.L4r => data[data[data[data[LB]]]],
                Machine.L5r => data[data[data[data[data[LB]]]]],
                Machine.L6r => data[data[data[data[data[data[LB]]]]]],
                Machine.CPr => CP,
                _ => 0,
            };
        }

        #endregion

        #region Program Status

        //Writes a summary of the machine's state
        public static void Dump()
        {
            int address, staticLink, dynamicLink, localRegisterNumber;
            Console.WriteLine("");
            Console.WriteLine("State of data store and register:");
            Console.WriteLine("");
            if (HT == HB)
            {
                Console.WriteLine("            |--------|          (heap is empty)");
            }
            else
            {
                Console.WriteLine("       HB-->");
                Console.WriteLine("            |--------|");
                for (address = HB - 1; address >= HT; address--)
                {
                    Console.Write(address + ":");
                    if (address == HT)
                    {
                        Console.Write(" HT-->");
                    }
                    else
                    {
                        Console.Write("      ");
                    }
                    Console.WriteLine("|" + data[address] + "|");
                }
                Console.WriteLine("            |--------|");
            }
            Console.WriteLine("            |////////|");
            Console.WriteLine("            |////////|");
            if (ST == SB)
            {
                Console.WriteLine("            |--------|          (stack is empty)");
            }
            else
            {
                dynamicLink = LB;
                staticLink = LB;
                localRegisterNumber = Machine.LBr;
                Console.WriteLine("      ST--> |////////|");
                Console.WriteLine("            |--------|");
                for (address = ST - 1; address >= SB; address--)
                {
                    Console.Write(address + ":");
                    if (address == SB)
                    {
                        Console.Write(" SB-->");
                    }
                    else if (address == staticLink)
                    {
                        switch (localRegisterNumber)
                        {
                            case Machine.LBr:
                                Console.Write(" LB-->");
                                break;
                            case Machine.L1r:
                                Console.Write(" L1-->");
                                break;
                            case Machine.L2r:
                                Console.Write(" L2-->");
                                break;
                            case Machine.L3r:
                                Console.Write(" L3-->");
                                break;
                            case Machine.L4r:
                                Console.Write(" L4-->");
                                break;
                            case Machine.L5r:
                                Console.Write(" L5-->");
                                break;
                            case Machine.L6r:
                                Console.Write(" L6-->");
                                break;
                        }
                        staticLink = data[address];
                        localRegisterNumber++;
                    }
                    else
                    {
                        Console.Write("      ");
                    }
                    if ((address == dynamicLink) && (dynamicLink != SB))
                    {
                        Console.Write("|SL=" + data[address] + "|");
                    }
                    else if ((address == dynamicLink + 1) && (dynamicLink != SB))
                    {
                        Console.Write("|DL=" + data[address] + "|");
                    }
                    else if ((address == dynamicLink + 2) && (dynamicLink != SB))
                    {
                        Console.Write("|RA=" + data[address] + "|");
                    }
                    else
                    {
                        Console.Write("|" + data[address] + "|");
                    }
                    Console.WriteLine("");
                    if (address == dynamicLink)
                    {
                        Console.WriteLine("            |--------|");
                        dynamicLink = data[address + 1];
                    }
                }
            }
            Console.WriteLine("");
        }

        public static void ShowStatus()
        {
            // Writes an indication of whether and why the program has terminated.
            Console.WriteLine("");
            switch (status)
            {
                case RUNNING:
                    Console.WriteLine("Program is running.");
                    break;
                case HALTED:
                    Console.WriteLine("Program has halted normally.");
                    break;
                case FAILEDDATASTOREFULL:
                    Console.WriteLine("Program has failed due to exhaustion of Data Store.");
                    break;
                case FAILEDINVALIDCODEADDRESS:
                    Console.WriteLine("Program has failed due to an invalid code address.");
                    break;
                case FAILEDINVALIDINSTRUCTION:
                    Console.WriteLine("Program has failed due to an invalid instruction.");
                    break;
                case FAILEDOVERFLOW:
                    Console.WriteLine("Program has failed due to overflow.");
                    break;
                case FAILEDZERODIVIDE:
                    Console.WriteLine("Program has failed due to division by zero.");
                    break;
                case FAILEDIOERROR:
                    Console.WriteLine("Program has failed due to an IO error.");
                    break;
                case FAILEDARRAYINDEXOUTOFBOUNDS:
                    Console.WriteLine("Program has failed due to an Index Out of Bounds");
                    break;
            }
            if (status != HALTED)
            {
                Dump();
            }
        }

        #endregion


        #region Interpretation

        static void CheckSpace(int spaceNeeded)
        {
            // Signals failure if there is not enough space to expand the stack or
            // heap by spaceNeeded.

            if (HT - ST < spaceNeeded)
            {
                status = FAILEDDATASTOREFULL;
            }
        }

        public static bool IsTrue(int datum)
        {
            // Tests whether the given datum represents true.
            return (datum == Machine.trueRep);
        }

        public static bool Equal(int size, int addr1, int addr2)
        {
            // Tests whether two multi-word objects are equal, given their common
            // size and their base addresses.

            bool eq;
            int index;

            eq = true;
            index = 0;
            while (eq && (index < size))
            {
                if (data[addr1 + index] == data[addr2 + index])
                {
                    index++;
                }
                else
                {
                    eq = false;
                }
            }
            return eq;
        }

        public static int OverflowChecked(long datum)
        {
            // Signals failure if the datum is too large to fit into a single word,
            // otherwise returns the datum as a single word.

            if ((-Machine.maxintRep <= datum) && (datum <= Machine.maxintRep))
            {
                return (int)datum;
            }
            else
            {
                status = FAILEDOVERFLOW;
                return 0;
            }
        }

        public static int ToInt(bool b)
        {
            return b ? Machine.trueRep : Machine.falseRep;
        }

        public static int ReadInt()
        {
            int temp = 0;
            int sign = 1;

            do
            {
                CurrentChar = Console.Read();
            } while (char.IsWhiteSpace((char)CurrentChar));

            if ((CurrentChar == '-') || (CurrentChar == '+'))
            {
                do
                {
                    sign = (CurrentChar == '-') ? -1 : 1;
                    CurrentChar = Console.Read();
                } while ((CurrentChar == '-') || CurrentChar == '+');
            }

            if (char.IsDigit((char)CurrentChar))
            {
                do
                {
                    temp = temp * 10 + (CurrentChar - '0');
                    CurrentChar = Console.Read();
                } while (char.IsDigit((char)CurrentChar));
            }

            return sign * temp;
        }

        static void CallPrimitive(int primitiveDisplacement)
        {
            // Invokes the given primitive routine.

            int addr, size;
            char ch;

            switch (primitiveDisplacement)
            {
                case Machine.idDisplacement:
                    break; // nothing to be done
                case Machine.notDisplacement:
                    data[ST - 1] = ToInt(!IsTrue(data[ST - 1]));
                    break;
                case Machine.andDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(IsTrue(data[ST - 1]) & IsTrue(data[ST]));
                    break;
                case Machine.orDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(IsTrue(data[ST - 1]) | IsTrue(data[ST]));
                    break;
                case Machine.succDisplacement:
                    data[ST - 1] = OverflowChecked(data[ST - 1] + 1);
                    break;
                case Machine.predDisplacement:
                    data[ST - 1] = OverflowChecked(data[ST - 1] - 1);
                    break;
                case Machine.negDisplacement:
                    data[ST - 1] = -data[ST - 1];
                    break;
                case Machine.addDisplacement:
                    ST--;
                    accumulator = data[ST - 1];
                    data[ST - 1] = OverflowChecked(accumulator + data[ST]);
                    break;
                case Machine.subDisplacement:
                    ST--;
                    accumulator = data[ST - 1];
                    data[ST - 1] = OverflowChecked(accumulator - data[ST]);
                    break;
                case Machine.multDisplacement:
                    ST--;
                    accumulator = data[ST - 1];
                    data[ST - 1] = OverflowChecked(accumulator * data[ST]);
                    break;
                case Machine.divDisplacement:
                    ST--;
                    accumulator = data[ST - 1];
                    if (data[ST] != 0)
                    {
                        data[ST - 1] = (int)(accumulator / data[ST]);
                    }
                    else
                    {
                        status = FAILEDZERODIVIDE;
                    }
                    break;
                case Machine.modDisplacement:
                    ST--;
                    accumulator = data[ST - 1];
                    if (data[ST] != 0)
                    {
                        data[ST - 1] = (int)(accumulator % data[ST]);
                    }
                    else
                    {
                        status = FAILEDZERODIVIDE;
                    }
                    break;
                case Machine.ltDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(data[ST - 1] < data[ST]);
                    break;
                case Machine.leDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(data[ST - 1] <= data[ST]);
                    break;
                case Machine.geDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(data[ST - 1] >= data[ST]);
                    break;
                case Machine.gtDisplacement:
                    ST--;
                    data[ST - 1] = ToInt(data[ST - 1] > data[ST]);
                    break;
                case Machine.eqDisplacement:
                    size = data[ST - 1]; // size of each comparand
                    ST -= 2 * size;
                    data[ST - 1] = ToInt(Equal(size, ST - 1, ST - 1 + size));
                    break;
                case Machine.neDisplacement:
                    size = data[ST - 1]; // size of each comparand
                    ST -= 2 * size;
                    data[ST - 1] = ToInt(!Equal(size, ST - 1, ST - 1 + size));
                    break;
                case Machine.eolDisplacement:
                    data[ST] = ToInt(CurrentChar == '\n');
                    ST++;
                    break;
                case Machine.eofDisplacement:
                    data[ST] = ToInt(CurrentChar == -1);
                    ST++;
                    break;
                case Machine.getDisplacement:
                    ST--;
                    addr = data[ST];
                    try
                    {
                        CurrentChar = Console.Read();
                    }
                    catch (System.IO.IOException)
                    {
                        status = FAILEDIOERROR;
                    }
                    data[addr] = (int)CurrentChar;
                    break;
                case Machine.putDisplacement:
                    ST--;
                    ch = (char)data[ST];
                    Console.Write(ch);
                    break;
                case Machine.geteolDisplacement:
                    try
                    {
                        while ((CurrentChar = Console.Read()) != '\n');
                    }
                    catch (System.IO.IOException)
                    {
                        status = FAILEDIOERROR;
                    }
                    break;
                case Machine.puteolDisplacement:
                    Console.WriteLine("");
                    break;
                case Machine.getintDisplacement:
                    ST--;
                    addr = data[ST];
                    try
                    {
                        accumulator = ReadInt();
                    }
                    catch (System.IO.IOException)
                    {
                        status = FAILEDIOERROR;
                    }
                    data[addr] = (int)accumulator;
                    break;
                case Machine.putintDisplacement:
                    ST--;
                    accumulator = data[ST];
                    Console.Write(accumulator);
                    break;
                case Machine.newDisplacement:
                    size = data[ST - 1];
                    CheckSpace(size);
                    HT -= size;
                    data[ST - 1] = HT;
                    break;
                case Machine.disposeDisplacement:
                    ST--; // no action taken at present
                    break;
                case Machine.indexCheckDisplacement:
                    // Upper bound, then lower bound, then index to check
                    if (data[ST - 1] <= data[ST - 3] || data[ST - 2] > data[ST - 3])
                    {
                        status = FAILEDARRAYINDEXOUTOFBOUNDS;// It's out of bounds
                    }
                    else
                    {
                        ST -= 3;
                    }
                    break;
            }
        }

        public static void InterpretProgram()
        {
            // Runs the program in code store.

            Instruction currentInstr;
            int op, r, n, d, addr, index;

            // Initialize registers ...
            ST = SB;
            HT = HB;
            LB = SB;
            CP = CB;
            status = RUNNING;
            do
            {
                // Fetch instruction ...
                currentInstr = Machine.code[CP];
                // Decode instruction ...
                op = currentInstr.op;
                r = currentInstr.r;
                n = currentInstr.n;
                d = currentInstr.d;
                // Execute instruction ...
                //dump();//Debugging
                switch (op)
                {
                    case Machine.LOADop:
                        addr = d + Content(r);
                        CheckSpace(n);
                        for (index = 0; index < n; index++)
                        {
                            data[ST + index] = data[addr + index];
                        }
                        ST += n;
                        CP++;
                        break;
                    case Machine.LOADAop:
                        addr = d + content(r);
                        checkSpace(1);
                        data[ST] = addr;
                        ST = ST + 1;
                        CP = CP + 1;
                        break;
                    case Machine.LOADIop:
                        ST = ST - 1;
                        addr = data[ST];
                        checkSpace(n);
                        for (index = 0; index < n; index++)
                        {
                            data[ST + index] = data[addr + index];
                        }
                        ST = ST + n;
                        CP = CP + 1;
                        break;
                    case Machine.LOADLop:
                        checkSpace(1);
                        data[ST] = d;
                        ST = ST + 1;
                        CP = CP + 1;
                        break;
                    case Machine.STOREop:
                        addr = d + content(r);
                        ST = ST - n;
                        for (index = 0; index < n; index++)
                        {
                            data[addr + index] = data[ST + index];
                        }
                        CP = CP + 1;
                        break;
                    case Machine.STOREIop:
                        ST = ST - 1;
                        addr = data[ST];
                        ST = ST - n;
                        for (index = 0; index < n; index++)
                        {
                            data[addr + index] = data[ST + index];
                        }
                        CP = CP + 1;
                        break;
                    case Machine.CALLop:
                        addr = d + content(r);
                        if (addr >= Machine.PB)
                        {
                            callPrimitive(addr - Machine.PB);
                            CP = CP + 1;
                        }
                        else
                        {
                            checkSpace(3);
                            if ((0 <= n) && (n <= 15))
                            {
                                data[ST] = content(n); // static link
                            }
                            else
                            {
                                status = FAILEDINVALIDINSTRUCTION;
                            }
                            data[ST + 1] = LB; // dynamic link
                            data[ST + 2] = CP + 1; // return address
                            LB = ST;
                            ST = ST + 3;
                            CP = addr;
                        }
                        break;
                    case Machine.CALLIop:
                        ST = ST - 2;
                        addr = data[ST + 1];
                        if (addr >= Machine.PB)
                        {
                            callPrimitive(addr - Machine.PB);
                            CP = CP + 1;
                        }
                        else
                        {
                            // data[ST] = static link already
                            data[ST + 1] = LB; // dynamic link
                            data[ST + 2] = CP + 1; // return address
                            LB = ST;
                            ST = ST + 3;
                            CP = addr;
                        }
                        break;
                    case Machine.RETURNop:
                        addr = LB - d;
                        CP = data[LB + 2];
                        LB = data[LB + 1];
                        ST = ST - n;
                        for (index = 0; index < n; index++)
                        {
                            data[addr + index] = data[ST + index];
                        }
                        ST = addr + n;
                        break;
                    case Machine.PUSHop:
                        checkSpace(d);
                        ST = ST + d;
                        CP = CP + 1;
                        break;
                    case Machine.POPop:
                        addr = ST - n - d;
                        ST = ST - n;
                        for (index = 0; index < n; index++)
                        {
                            data[addr + index] = data[ST + index];
                        }
                        ST = addr + n;
                        CP = CP + 1;
                        break;
                    case Machine.JUMPop:
                        CP = d + content(r);
                        break;
                    case Machine.JUMPIop:
                        ST = ST - 1;
                        CP = data[ST];
                        break;
                    case Machine.JUMPIFop:
                        ST = ST - 1;
                        if (data[ST] == n)
                        {
                            CP = d + content(r);
                        }
                        else
                        {
                            CP = CP + 1;
                        }
                        break;
                    case Machine.HALTop:
                        status = HALTED;
                        break;
                }
                if ((CP < CB) || (CP >= CT))
                {
                    status = FAILEDINVALIDCODEADDRESS;
                }
            } while (status == RUNNING);
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
