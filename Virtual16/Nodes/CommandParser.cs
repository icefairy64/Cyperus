using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyperus;

namespace Virtual16.Nodes
{
    public class MemoryStoreOperation
    {
        public readonly ushort Address;
        public readonly byte[] Data;

        public MemoryStoreOperation(ushort addr, byte[] data)
        {
            Address = addr;
            Data = data;
        }
    }
    
    public class CommandParser : Processor
    {
        protected Socket<TelnetCommand> In;
        protected Socket<byte[]> CmdOut;
        protected Socket<MemoryStoreOperation> StoreOut;
        
        public CommandParser(string name, Cyperus.Environment env)
            : base(name, env)
        {
            In = AddInput<TelnetCommand>("in");
            CmdOut = AddOutput<byte[]>("cmd");
            StoreOut = AddOutput<MemoryStoreOperation>("storeOp");
        }

        protected override async Task<object> ProcessData(ISender sender, object data)
        {
            var cmd = data as TelnetCommand;
            if (cmd == null)
                return null;

            int d = cmd.Value.IndexOf(' ');
            var str = d < 0 ? cmd.Value : cmd.Value.Substring(0, d);
            var args = d < 0 ? "" : cmd.Value.Substring(d + 1);
            try
            {
                if (str == "st")
                {
                    // Storing data block into memory
                    var addrOffset = args.IndexOf(' ');
                    if (addrOffset < 0)
                        throw new Exception("Memory store address is undefined");
                    var addr = FSMParser.ParseNumber(args.Substring(0, addrOffset), 16);
                    args = args.Substring(addrOffset + 1).ToLower();
                    var buf = "";
                    int i = 0;
                    var list = new List<byte>();
                    foreach (char c in args)
                    {
                        if (c == ' ')
                            continue;
                        if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'))
                        {
                            buf += c;
                            i++;
                            if (i % 2 == 0)
                            {
                                list.Add((byte)FSMParser.ParseNumber(buf, 16));
                                buf = "";
                            }
                        }
                    }
                    var msg = String.Format("Stored {0} bytes\r\n", (i + 1) / 2);
                    cmd.Stream.Write(TelnetServer.StringToByteArray(msg), 0, msg.Length);

                    return new MemoryStoreOperation(addr, list.ToArray());
                }
                else
                {
                    // Executing instruction
                    var res = await Task.Run(() => FSMParser.Parse(args.ToLower(), str));
                    var query =
                        from x in res
                        select String.Format("{0:X2} ", x);
                    var msg = query.Aggregate("", (a, x) => { return a + x; });
                    msg = String.Format("Executed; runnable code: {0}\r\n", msg);
                    cmd.Stream.Write(TelnetServer.StringToByteArray(msg), 0, msg.Length);

                    return res;
                }
            }
            catch (Exception e)
            {
                var msg = String.Format("{0}: {1}\r\n", e.GetType().Name, e.Message);
                cmd.Stream.Write(TelnetServer.StringToByteArray(msg), 0, msg.Length);
                return null;
            }
        }

        protected override async Task DispatchData(ISender sender, object data)
        {
            if (data is byte[])
            {
                var cmd = data as byte[];
                await SendToSocket(CmdOut, data);
                return;
            }
            else if (data is MemoryStoreOperation)
            {
                var cmd = data as MemoryStoreOperation;
                await SendToSocket(StoreOut, data);
                return;
            }
        }
    }

    #region Enums and context

    enum FSMState
    {
        Start,
        Char,
        Digit,
        HexSign,
        OpeningBracket,
        HexDigit,
        RefChar,
        RefDigit,
        RefHexSign,
        RefHexDigit,
        ClosingBracket
    }

    enum FSMInput
    {
        Char,
        Digit,
        HexChar,
        HexSign,
        OpeningBracket,
        ClosingBracket,
        Colon,
        Unknown
    }

    class FSMContext
    {
        public string Buffer = "";
        public List<InstructionArgument> Arguments = new List<InstructionArgument>();
        public List<ushort> Values = new List<ushort>();
    }

    #endregion

    /// <summary>
    /// Represents FSM designed to parse assembly language instructions
    /// </summary>
    class FSMParser
    {
        FSMState State;
        FSMContext Context = new FSMContext();

        static int[,] Matrix = new int[11, 7]
        {
            {  1,  2,  1,  3,  4, -1, -1 }, // start
            {  1, -1,  1, -1, -1, -1,  0 }, // c
            { -1,  2, -1, -1, -1, -1,  0 }, // d
            { -1,  5,  5, -1, -1, -1, -1 }, // $
            {  6,  7,  6,  8, -1, -1, -1 }, // (
            { -1,  5,  5, -1, -1, -1,  0 }, // h
            {  6, -1,  6, -1, -1, 10, -1 }, // rc
            { -1,  7, -1, -1, -1, 10, -1 }, // rd
            { -1,  9,  9, -1, -1, -1, -1 }, // r$
            { -1,  9,  9, -1, -1, 10, -1 }, // rh
            { -1, -1, -1, -1, -1, -1,  0 }  // )
        };

        static FSMState[] FinalStates = new FSMState[] { FSMState.Start, FSMState.Digit, FSMState.Char, FSMState.HexDigit, FSMState.RefChar, FSMState.RefDigit, FSMState.RefHexDigit, FSMState.ClosingBracket };

        /// <summary>
        /// Processes given character
        /// </summary>
        /// <param name="c">Character</param>
        public void Input(char c)
        {
            FSMInput input = FSMInput.Unknown;
            if (c >= 'a' && c <= 'f')
            {
                input = FSMInput.HexChar;
                Context.Buffer += c;
            }
            else if (c >= 'e' && c <= 'z')
            {
                input = FSMInput.Char;
                Context.Buffer += c;
            }
            else if (c >= '0' && c <= '9')
            {
                input = FSMInput.Digit;
                Context.Buffer += c;
            }
            else if (c == '(')
                input = FSMInput.OpeningBracket;
            else if (c == ')')
                input = FSMInput.ClosingBracket;
            else if (c == '$')
                input = FSMInput.HexSign;
            else if (c == ',')
                input = FSMInput.Colon;

            if (input == FSMInput.Unknown)
                throw new Exception(String.Format("Error on char {0}", c));

            int nextState = Matrix[(int)State, (int)input];
            if (nextState < 0)
                throw new Exception(String.Format("Error on char {0}", c));

            State = (FSMState)nextState;
        }

        /// <summary>
        /// Converts given string to managed register destination or instruction argument kind type
        /// </summary>
        /// <param name="reg">Register name</param>
        /// <returns>RegisterDestination8, RegisterDestination16 or InstructionArgumentKind</returns>
        /// <exception cref="Exception"></exception>
        static object GuessRegister(string reg)
        {
            switch (reg)
            {
                case "a":
                    return RegisterDestination8.A;
                case "b":
                    return RegisterDestination8.B;
                case "c":
                    return RegisterDestination8.C;
                case "d":
                    return RegisterDestination8.D;
                case "e":
                    return RegisterDestination8.E;
                case "h":
                    return RegisterDestination8.H;
                case "l":
                    return RegisterDestination8.L;
                case "bc":
                    return RegisterDestination16.BC;
                case "de":
                    return RegisterDestination16.DE;
                case "hl":
                    return RegisterDestination16.HL;
                case "sp":
                    return InstructionArgumentKind.StackPointer;
                default:
                    throw new Exception(String.Format("Unknown sequence: {0}", reg));
            }
        }

        /// <summary>
        /// Parses string representation of number in given base
        /// </summary>
        /// <param name="str">String representation of number</param>
        /// <param name="bs">Base</param>
        /// <returns></returns>
        public static ushort ParseNumber(string str, int bs)
        {
            ushort tmp = 0;
            int i = 1;

            for (int j = str.Length - 1; j >= 0; j--)
            {
                char c = str[j];
                if (c < 'a')
                    tmp += (ushort)(i * (c - '0'));
                else
                    tmp += (ushort)(i * ((c - 'a') + 10));

                i *= bs;
            }

            return tmp;
        }

        /// <summary>
        /// Adds an argument to parser's context argument list
        /// </summary>
        /// <param name="parser">Parser</param>
        private static void AddArgument(FSMParser parser, FSMState prevState)
        {
            var buf = parser.Context.Buffer;
            parser.Context.Buffer = "";

            var kind = InstructionArgumentKind.Constant8;
            var isRef = false;
            ushort value = 0;
            switch (prevState)
            {
                case FSMState.RefChar:
                case FSMState.Char:
                    // Register
                    if (prevState == FSMState.RefChar)
                        isRef = true;

                    var reg = GuessRegister(buf);
                    if (reg is RegisterDestination8)
                    {
                        kind = InstructionArgumentKind.Register8;
                        if ((RegisterDestination8)reg == RegisterDestination8.A)
                            kind = InstructionArgumentKind.Accumulator;

                        value = (ushort)((RegisterDestination16)reg);
                    }
                    else if (reg is RegisterDestination16)
                    {
                        kind = InstructionArgumentKind.Register16;
                        if ((RegisterDestination16)reg == RegisterDestination16.HL)
                            kind = InstructionArgumentKind.HL;

                        value = (ushort)((RegisterDestination16)reg);
                    }
                    else if (reg is InstructionArgumentKind)
                        kind = (InstructionArgumentKind)reg;

                    break;

                case FSMState.Digit:
                case FSMState.HexDigit:
                case FSMState.RefDigit:
                case FSMState.RefHexDigit:
                    // Data
                    if (prevState == FSMState.RefDigit || prevState == FSMState.RefHexDigit)
                        isRef = true;

                    bool isHex = prevState == FSMState.RefHexDigit || prevState == FSMState.HexDigit;
                    kind = isHex && buf.Length > 2 ? InstructionArgumentKind.Constant16 : InstructionArgumentKind.Constant8;

                    try
                    {
                        value = ParseNumber(buf, isHex ? 16 : 10);
                        if (!isHex && value > 0xff)
                            kind = InstructionArgumentKind.Constant16;
                    }
                    catch (OverflowException e)
                    {
                        throw new Exception(String.Format("An error occured when parsing number {0}: resulting value is too big"));
                    }

                    break;
            }

            parser.Context.Arguments.Add(new InstructionArgument(kind, isRef));
            parser.Context.Values.Add(value);
        }

        /// <summary>
        /// Returns set of instructions that can accept given context's arguments
        /// </summary>
        /// <param name="context">Parser context</param>
        /// <param name="mnemonic">Instruction mnemonic</param>
        /// <returns></returns>
        private static Instruction[] LookupSignature(FSMContext context, string mnemonic)
        {
            var list = new List<Instruction>();
            var query =
                from inst in Instruction.Set
                where inst.Mnemonic == mnemonic
                select inst;

            var signList = query.ToList();
            var sign = new InstructionSignature(context.Arguments.ToArray());

            foreach (var s in signList)
            {
                // Make sure that most fitting instruction goes first
                if (s.Signature.Equals(sign))
                {
                    list.Insert(0, s);
                    break;
                }
                else if (s.Signature.IsCompatibleWith(sign))
                    list.Add(s);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Generates runnable code based on arguments and its values of given parser context
        /// </summary>
        /// <param name="context">Parser context</param>
        /// <param name="mnemonic">Instruction mnemonic</param>
        /// <returns>Runnable code</returns>
        private static byte[] GenerateCode(FSMContext context, string mnemonic)
        {
            var list = new List<byte>();
            var inst = LookupSignature(context, mnemonic)[0];
            int pos = 0;

            list.Add(inst.OpCode);

            for (int i = 0; i < inst.Signature.Arguments.Length; i++)
            {
                switch (inst.Signature.Arguments[i].Kind)
                {
                    case InstructionArgumentKind.Register16:
                        if (inst.Signature.Arguments[i].Kind != context.Arguments[i].Kind)
                        {
                            byte val = (byte)(RegisterDestination16.HL);
                            list.Add(val);
                            pos++;
                            break;
                        }
                        goto case InstructionArgumentKind.Constant8;
                    case InstructionArgumentKind.Constant16:
                        list.Add((byte)(context.Values[pos] & 0xff));
                        list.Add((byte)((context.Values[pos] >> 8) & 0xff));
                        pos++;
                        break;
                    case InstructionArgumentKind.Register8:
                        if (inst.Signature.Arguments[i].Kind != context.Arguments[i].Kind)
                        {
                            byte val = (byte)(RegisterDestination8.A);
                            list.Add(val);
                            pos++;
                            break;
                        }
                        goto case InstructionArgumentKind.Constant8;
                    case InstructionArgumentKind.Constant8:
                        list.Add((byte)(context.Values[pos] & 0xff));
                        pos++;
                        break;
                    case InstructionArgumentKind.Accumulator:
                    case InstructionArgumentKind.HL:
                    case InstructionArgumentKind.StackPointer:
                        pos++;
                        break;
                }
            }

            return list.ToArray();
        }


        /// <summary>
        /// Parses given assembler instruction
        /// </summary>
        /// <param name="cmd">Instruction arguments</param>
        /// <param name="mnemonic">Mnemonic of instruction</param>
        /// <returns>Runnable code</returns>
        /// <exception cref="Exception"></exception>
        public static byte[] Parse(string cmd, string mnemonic)
        {
            FSMParser parser = new FSMParser();
            var prevState = parser.State;
            var pprevState = prevState;

            // Parsing
            foreach (var ch in cmd)
            {
                pprevState = prevState;
                prevState = parser.State;
                if (ch == ' ')
                    continue;

                parser.Input(ch);

                if (parser.State == FSMState.Start)
                {
                    if (prevState == FSMState.ClosingBracket)
                        AddArgument(parser, pprevState);
                    else
                        AddArgument(parser, prevState);
                }
            }

            // Adding last argument
            if (parser.State != FSMState.Start && parser.State != FSMState.ClosingBracket)
                AddArgument(parser, parser.State);
            else if (parser.State == FSMState.ClosingBracket)
                AddArgument(parser, prevState);

            // Generating runnable code
            try
            {
                return GenerateCode(parser.Context, mnemonic);
            }
            catch (IndexOutOfRangeException e)
            {
                var msg = "";

                for (int i = 0; i < parser.Context.Arguments.Count; i++)
                {
                    msg += parser.Context.Arguments[i].ToString();
                    if (i < parser.Context.Arguments.Count - 1)
                        msg += ", ";
                }

                throw new Exception(String.Format("Could not find instruction {0} with parameters {1}", mnemonic, msg));
            }
        }
    }
}
