using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual16
{
    public delegate void InstructionImpl(V16CPU cpu);
    
    public partial struct Instruction
    {
        public readonly InstructionImpl Implementation;
        public readonly InstructionSignature Signature;
        public readonly string Mnemonic;
        public readonly byte OpCode;

        public Instruction(string mnemonic, byte opcode, InstructionSignature signature, InstructionImpl implementation)
            : this()
        {
            Signature = signature;
            Implementation = implementation;
            Mnemonic = mnemonic;
            OpCode = opcode;
        }

        public static Instruction[] Set 
        {
            get { return FSet; }
        }

        static Instruction[] FSet = GenerateInstructionSet();

        static Instruction[] GenerateInstructionSet()
        {
            var tmp = new Instruction[0xA00];

            tmp[0x00] = new Instruction("nop", 0x00, InstructionSignature.Empty, Nop);
            tmp[0x01] = new Instruction("jmp", 0x01, new InstructionSignature(InstructionArgument.D16), Jmp);
            tmp[0x02] = new Instruction("jmp", 0x02, new InstructionSignature(InstructionArgument.D8, InstructionArgument.D16), JmpConditional);
            tmp[0x03] = new Instruction("jmp", 0x03, new InstructionSignature(InstructionArgument.HL), JmpHL);
            tmp[0x04] = new Instruction("call", 0x04, new InstructionSignature(InstructionArgument.D16), Call);
            tmp[0x05] = new Instruction("ret", 0x05, InstructionSignature.Empty, Ret);
            tmp[0x06] = new Instruction("push", 0x06, new InstructionSignature(InstructionArgument.R16), Push);
            tmp[0x07] = new Instruction("pop", 0x07, new InstructionSignature(InstructionArgument.R16), Pop);

            tmp[0x20] = new Instruction("ld", 0x20, new InstructionSignature(InstructionArgument.A, InstructionArgument.R8), LoadAFromReg);
            tmp[0x21] = new Instruction("ld", 0x21, new InstructionSignature(InstructionArgument.R8, InstructionArgument.A), LoadRegFromA);
            tmp[0x22] = new Instruction("ld", 0x22, new InstructionSignature(InstructionArgument.A, InstructionArgument.HLR), LoadAFromHLR);
            tmp[0x23] = new Instruction("ld", 0x23, new InstructionSignature(InstructionArgument.HLR, InstructionArgument.A), LoadHLRFromA);
            tmp[0x24] = new Instruction("ld", 0x24, new InstructionSignature(InstructionArgument.A, InstructionArgument.D8), LoadA);
            tmp[0x25] = new Instruction("ld", 0x25, new InstructionSignature(InstructionArgument.R16, InstructionArgument.D16), LoadReg16);
            tmp[0x26] = new Instruction("ld", 0x26, new InstructionSignature(InstructionArgument.SP, InstructionArgument.D16), LoadSP);
            tmp[0x27] = new Instruction("ld", 0x27, new InstructionSignature(InstructionArgument.R8, InstructionArgument.D16R), LoadReg8FromR);
            tmp[0x28] = new Instruction("ld", 0x28, new InstructionSignature(InstructionArgument.R16, InstructionArgument.D16R), LoadReg16FromR);
            tmp[0x29] = new Instruction("ld", 0x29, new InstructionSignature(InstructionArgument.SP, InstructionArgument.D16R), LoadSPFromR);
            tmp[0x2A] = new Instruction("ld", 0x2A, new InstructionSignature(InstructionArgument.R8, InstructionArgument.HLR), LoadReg8FromHLR);
            tmp[0x2B] = new Instruction("ld", 0x2B, new InstructionSignature(InstructionArgument.A, InstructionArgument.R16R), LoadAFromReg16R);
            tmp[0x2C] = new Instruction("ld", 0x2C, new InstructionSignature(InstructionArgument.D16R, InstructionArgument.R8), LoadFromReg8);
            tmp[0x2D] = new Instruction("ld", 0x2D, new InstructionSignature(InstructionArgument.HLR, InstructionArgument.R8), LoadHLRFromReg8);

            tmp[0x40] = new Instruction("add", 0x40, new InstructionSignature(InstructionArgument.D8), Add);
            tmp[0x41] = new Instruction("add", 0x41, new InstructionSignature(InstructionArgument.R8), AddReg);
            tmp[0x42] = new Instruction("add", 0x42, new InstructionSignature(InstructionArgument.HLR), AddHLR);

            tmp[0x44] = new Instruction("adc", 0x44, new InstructionSignature(InstructionArgument.D8), AddC);
            tmp[0x45] = new Instruction("adc", 0x45, new InstructionSignature(InstructionArgument.R8), AddCReg);
            tmp[0x46] = new Instruction("adc", 0x46, new InstructionSignature(InstructionArgument.HLR), AddCHLR);

            tmp[0x48] = new Instruction("sub", 0x48, new InstructionSignature(InstructionArgument.D8), Sub);
            tmp[0x49] = new Instruction("sub", 0x49, new InstructionSignature(InstructionArgument.R8), SubReg);
            tmp[0x4A] = new Instruction("sub", 0x4A, new InstructionSignature(InstructionArgument.HLR), SubHLR);

            tmp[0x4C] = new Instruction("sbc", 0x4C, new InstructionSignature(InstructionArgument.D8), SubC);
            tmp[0x4D] = new Instruction("sbc", 0x4D, new InstructionSignature(InstructionArgument.R8), SubCReg);
            tmp[0x4E] = new Instruction("sbc", 0x4E, new InstructionSignature(InstructionArgument.HLR), SubCHLR);

            return tmp;
        }
    }

    public struct InstructionSignature
    {
        public readonly byte ArgSize;
        public readonly InstructionArgument[] Arguments;

        public InstructionSignature(params InstructionArgument[] args)
        {
            Arguments = args;
            ArgSize = 0;
            foreach (var arg in args)
            {
                switch (arg.Kind)
                {
                    case InstructionArgumentKind.Accumulator:
                        break;
                    case InstructionArgumentKind.HL:
                        break;
                    case InstructionArgumentKind.StackPointer:
                        break;
                    case InstructionArgumentKind.Constant16:
                        ArgSize += 2;
                        break;
                    case InstructionArgumentKind.Register16:
                        ArgSize += 2;
                        break;
                    case InstructionArgumentKind.Constant8:
                        ArgSize += 1;
                        break;
                    case InstructionArgumentKind.Register8:
                        ArgSize += 1;
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InstructionSignature))
                return base.Equals(obj);

            var sign = (InstructionSignature)obj;
            if (sign.Arguments.Length != Arguments.Length)
                return false;
            if (sign.ArgSize != ArgSize)
                return false;

            for (int i = 0; i < Arguments.Length; i++)
            { 
                if (!sign.Arguments[i].Equals(Arguments[i]))
                    return false;
            }

            return true;
        }

        public bool IsCompatibleWith(InstructionSignature sign)
        {
            if (sign.Arguments.Length != Arguments.Length)
                return false;

            for (int i = 0; i < Arguments.Length; i++)
            {
                if (!(sign.Arguments[i].Equals(Arguments[i]) || InstructionArgument.IsCompatible(Arguments[i], sign.Arguments[i])))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            string tmp = "";

            for (int i = 0; i < Arguments.Length; i++)
            {
                tmp += Arguments[i].ToString();
                if (i < Arguments.Length - 1)
                    tmp += ", ";
            }

            return tmp;
        }

        public static readonly InstructionSignature Empty = new InstructionSignature(new InstructionArgument[0]);
    }

    public struct InstructionArgument
    {
        public readonly InstructionArgumentKind Kind;
        public readonly bool IsReference;

        public InstructionArgument(InstructionArgumentKind kind, bool isRef)
        {
            Kind = kind;
            IsReference = isRef;
        }

        public static readonly InstructionArgument D8 = new InstructionArgument(InstructionArgumentKind.Constant8, false);
        public static readonly InstructionArgument D16 = new InstructionArgument(InstructionArgumentKind.Constant16, false);
        public static readonly InstructionArgument A = new InstructionArgument(InstructionArgumentKind.Accumulator, false);
        public static readonly InstructionArgument HL = new InstructionArgument(InstructionArgumentKind.HL, false);
        public static readonly InstructionArgument R8 = new InstructionArgument(InstructionArgumentKind.Register8, false);
        public static readonly InstructionArgument R16 = new InstructionArgument(InstructionArgumentKind.Register16, false);
        public static readonly InstructionArgument SP = new InstructionArgument(InstructionArgumentKind.StackPointer, false);

        public static readonly InstructionArgument D8R = new InstructionArgument(InstructionArgumentKind.Constant8, true);
        public static readonly InstructionArgument D16R = new InstructionArgument(InstructionArgumentKind.Constant16, true);
        public static readonly InstructionArgument HLR = new InstructionArgument(InstructionArgumentKind.HL, true);
        public static readonly InstructionArgument R8R = new InstructionArgument(InstructionArgumentKind.Register8, true);
        public static readonly InstructionArgument R16R = new InstructionArgument(InstructionArgumentKind.Register16, true);

        public static bool IsCompatible(InstructionArgument target, InstructionArgument src)
        {
            if (target.Equals(src))
                return true;

            if (target.IsReference != src.IsReference)
                return false;

            if (target.Kind == InstructionArgumentKind.Constant16 && src.Kind == InstructionArgumentKind.Constant8)
                return true;

            if (target.Kind == InstructionArgumentKind.Register8 && src.Kind == InstructionArgumentKind.Accumulator)
                return true;

            if (target.Kind == InstructionArgumentKind.Register16 && (src.Kind == InstructionArgumentKind.HL || src.Kind == InstructionArgumentKind.StackPointer))
                return true;

            return false;
        }

        public override string ToString()
        {
            return IsReference ? String.Format("({0})", Kind.ToString()) : Kind.ToString();
        }
    }

    public enum InstructionArgumentKind
    {
        Constant8,
        Constant16,
        Accumulator,
        HL,
        Register8,
        Register16,
        StackPointer
    }
}
