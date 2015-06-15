using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Virtual16
{
    public delegate void Interrupt(V16CPU sender);
    
    public class V16CPU
    {
        public IntPtr Memory { get; internal set; }
        public byte A { get; internal set; }
        public ushort BC { get; internal set; }
        public ushort DE { get; internal set; }
        public ushort HL { get; internal set; }
        public ushort SP { get; internal set; }
        public ushort IP { get; internal set; }
        public byte F { get; internal set; }

        public Interrupt ResetHandler;
        public Interrupt ExternalHWMemUpdateHandler;

        public byte B
        {
            get { return (byte)(BC & ByteMask); }
            internal set { BC = (ushort)((BC & ~ByteMask) | value); }
        }

        public byte C
        {
            get { return (byte)(BC >> 8); }
            internal set { BC = (ushort)((BC & ByteMask) | (value << 8)); }
        }

        public byte D
        {
            get { return (byte)(DE & ByteMask); }
            internal set { DE = (ushort)((DE & ~ByteMask) | value); }
        }

        public byte E
        {
            get { return (byte)(DE >> 8); }
            internal set { DE = (ushort)((DE & ByteMask) | (value << 8)); }
        }

        public byte H
        {
            get { return (byte)(HL & ByteMask); }
            internal set { HL = (ushort)((HL & ~ByteMask) | value); }
        }

        public byte L
        {
            get { return (byte)(HL >> 8); }
            internal set { HL = (ushort)((HL & ByteMask) | (value << 8)); }
        }

        public bool Carry
        {
            get { return (F & CarryMask) > 0; }
            set { F = (byte)((F & ~CarryMask) | (value ? CarryMask : 0)); }
        }

        public bool Zero
        {
            get { return (F & ZeroMask) > 0; }
            set { F = (byte)((F & ~ZeroMask) | (value ? ZeroMask : 0)); }
        }

        public bool Sub
        {
            get { return (F & SubMask) > 0; }
            set { F = (byte)((F & ~SubMask) | (value ? SubMask : 0)); }
        }

        public bool HalfCarry
        {
            get { return (F & HalfCarryMask) > 0; }
            set { F = (byte)((F & ~HalfCarryMask) | (value ? HalfCarryMask : 0)); }
        }

        public V16CPU()
        {
            Memory = AllocMem();
            A = 0x00;
            SP = 0xEFFF;
            IP = 0x0000;
        }

        public void Tick(bool execCodeFromNonExecArea = false)
        {
            if (IP < TempCodeBlockEnd && !execCodeFromNonExecArea)
                return;
            
            // Loading instruction
            byte opCode = Virtual16.Memory.LoadByte(Memory, IP);
            Instruction inst = Instruction.Set[opCode];
            ushort sIP = IP;
            byte sHWStatus = Virtual16.Memory.LoadByte(Memory, HWStatusOffset);
            
            // Executing
            inst.Implementation(this);

            // Moving instruction pointer if neccessary
            if (IP == sIP)
                IP += (ushort)(inst.Signature.ArgSize + 1);

            // Checking external HW byte
            byte cHWStatus = Virtual16.Memory.LoadByte(Memory, HWStatusOffset);
            if (sHWStatus != cHWStatus && ExternalHWMemUpdateHandler != null)
                ExternalHWMemUpdateHandler(this);
        }

        public void Exec(params byte[] cmd)
        {
            for (ushort i = 0; i < cmd.Length; i++)
                Virtual16.Memory.StoreByte(Memory, (ushort)(i + TempCodeOffset), cmd[i]);

            ushort sIP = IP;
            IP = TempCodeOffset;
            Tick(true);
            if (IP < TempCodeBlockEnd)
                IP = sIP;
        }

        public void SetRegister16(RegisterDestination16 dest, ushort data)
        {
            switch (dest)
            {
                case RegisterDestination16.BC:
                    BC = data;
                    break;

                case RegisterDestination16.DE:
                    DE = data;
                    break;

                case RegisterDestination16.HL:
                    HL = data;
                    break;
            }
        }

        public void SetRegister8(RegisterDestination8 dest, byte data)
        {
            switch (dest)
            {
                case RegisterDestination8.A:
                    A = data;
                    break;

                case RegisterDestination8.B:
                    B = data;
                    break;

                case RegisterDestination8.C:
                    C = data;
                    break;

                case RegisterDestination8.D:
                    D = data;
                    break;

                case RegisterDestination8.E:
                    E = data;
                    break;

                case RegisterDestination8.H:
                    H = data;
                    break;

                case RegisterDestination8.L:
                    L = data;
                    break;
            }
        }

        public ushort GetRegister16(RegisterDestination16 src)
        {
            switch (src)
            {
                case RegisterDestination16.BC:
                    return BC;

                case RegisterDestination16.DE:
                    return DE;

                case RegisterDestination16.HL:
                    return HL;

                default:
                    return 0;
            }
        }

        public byte GetRegister8(RegisterDestination8 src)
        {
            switch (src)
            {
                case RegisterDestination8.A:
                    return A;

                case RegisterDestination8.B:
                    return B;

                case RegisterDestination8.C:
                    return C;

                case RegisterDestination8.D:
                    return D;

                case RegisterDestination8.E:
                    return E;

                case RegisterDestination8.H:
                    return H;

                case RegisterDestination8.L:
                    return L;

                default:
                    return 0;
            }
        }

        ~V16CPU()
        {
            FreeMem(Memory);
        }

        public static readonly ushort HWStatusOffset = 0xFFFF;
        public static readonly ushort SharedMemOffset = 0xF000;

        static readonly ushort TempCodeOffset = 0x0000;
        static readonly ushort TempCodeBlockEnd = 0x1000;

        static readonly ushort ByteMask = 0x00FF;
        static readonly byte CarryMask = 0x01;
        static readonly byte ZeroMask = 0x02;
        static readonly byte SubMask = 0x04;
        static readonly byte HalfCarryMask = 0x08;

        static IntPtr AllocMem()
        {
            return Marshal.AllocHGlobal(0xA0000);
        }

        static void FreeMem(IntPtr mem)
        {
            if (mem != IntPtr.Zero)
                Marshal.FreeHGlobal(mem);

            mem = IntPtr.Zero;
        }
    }

    public enum RegisterDestination16
    {
        BC = 0,
        DE = 1,
        HL = 2
    }

    public enum RegisterDestination8
    {
        A = 8,
        B = 9,
        C = 10,
        D = 11,
        E = 12,
        F = 13,
        H = 14,
        L = 15
    }

    public enum JumpCondition
    {
        Carry = 0,
        NonCarry = 1,
        Zero = 2,
        NonZero = 3
    }
}
