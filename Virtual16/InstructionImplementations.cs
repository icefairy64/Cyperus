using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual16
{
    public partial struct Instruction
    {
        #region Control operations

        static void Nop(V16CPU cpu)
        {
        }

        static void Jmp(V16CPU cpu)
        {
            ushort dest = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.IP = dest;
        }

        static void JmpHL(V16CPU cpu)
        {
            cpu.IP = cpu.HL;
        }

        static void JmpConditional(V16CPU cpu)
        {
            byte cond = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort dest = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 2));
            bool result = false;
            
            switch ((JumpCondition)cond)
            {
                case JumpCondition.Carry:
                    result = cpu.Carry;
                    break;

                case JumpCondition.NonCarry:
                    result = !cpu.Carry;
                    break;

                case JumpCondition.Zero:
                    result = cpu.Zero;
                    break;

                case JumpCondition.NonZero:
                    result = !cpu.Zero;
                    break;
            }

            if (result)
                cpu.IP = dest;
        }

        static void Call(V16CPU cpu)
        {
            ushort dest = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 1));
            Memory.StoreWord(cpu.Memory, cpu.SP, cpu.IP);
            cpu.SP -= 2;
            cpu.IP = dest;
        }

        static void Ret(V16CPU cpu)
        {
            ushort dest = Memory.LoadWord(cpu.Memory, cpu.SP);
            cpu.SP += 2;
            cpu.IP = dest;
        }

        #endregion

        #region Stack operations

        static void Push(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort data = cpu.GetRegister16((RegisterDestination16)src);
            Memory.StoreWord(cpu.Memory, cpu.SP, data);
            cpu.SP -= 2;
        }

        static void Pop(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort data = Memory.LoadWord(cpu.Memory, cpu.SP);
            cpu.SetRegister16((RegisterDestination16)dest, data);
            cpu.SP += 2;
        }

        #endregion

        #region Register-to-register transfer

        static void LoadAFromReg(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.A = cpu.GetRegister8((RegisterDestination8)src);
        }

        static void LoadRegFromA(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.SetRegister8((RegisterDestination8)dest, cpu.A);
        }

        static void LoadAFromHLR(V16CPU cpu)
        {
            cpu.A = Memory.LoadByte(cpu.Memory, cpu.HL);
        }

        static void LoadHLRFromA(V16CPU cpu)
        {
            Memory.StoreByte(cpu.Memory, cpu.HL, cpu.A);
        }

        #endregion

        #region Immediate register loading

        static void LoadA(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.A = data;
        }

        static void LoadReg16(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort data = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 2));
            cpu.SetRegister16((RegisterDestination16)dest, data);
        }

        static void LoadSP(V16CPU cpu)
        {
            ushort data = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.SP = data;
        } 

        #endregion

        #region Direct register loading

        static void LoadReg16FromR(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort src = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 2));
            cpu.SetRegister16((RegisterDestination16)dest, Memory.LoadWord(cpu.Memory, src));
        }

        static void LoadReg8FromR(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort src = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 2));
            cpu.SetRegister8((RegisterDestination8)dest, Memory.LoadByte(cpu.Memory, src));
        }

        static void LoadSPFromR(V16CPU cpu)
        {
            ushort src = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 1));
            cpu.SP = Memory.LoadWord(cpu.Memory, src);
        }
        
        #endregion

        #region Indirect register loading

        static void LoadReg8FromHLR(V16CPU cpu)
        {
            byte dest = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = Memory.LoadByte(cpu.Memory, cpu.HL);
            cpu.SetRegister8((RegisterDestination8)dest, data);
        }

        static void LoadAFromReg16R(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = Memory.LoadByte(cpu.Memory, cpu.GetRegister16((RegisterDestination16)src));
            cpu.A = data;
        }

        #endregion

        #region Memory storing

        static void LoadFromReg8(V16CPU cpu)
        {
            ushort dest = Memory.LoadWord(cpu.Memory, (ushort)(cpu.IP + 1));
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 3));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            Memory.StoreByte(cpu.Memory, dest, data);
        }

        static void LoadHLRFromReg8(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            Memory.StoreByte(cpu.Memory, cpu.HL, data);
        }
        
        #endregion

        #region Addition

        static void Add(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort tmp = (ushort)(cpu.A + data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        static void AddReg(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            ushort tmp = (ushort)(cpu.A + data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        static void AddHLR(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, cpu.HL);
            ushort tmp = (ushort)(cpu.A + data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        #endregion

        #region Addition with carry

        static void AddC(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort tmp = (ushort)(cpu.A + data + (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        static void AddCReg(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            ushort tmp = (ushort)(cpu.A + data + (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        static void AddCHLR(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, cpu.HL);
            ushort tmp = (ushort)(cpu.A + data + (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = false;
        }

        #endregion

        #region Subtraction

        static void Sub(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort tmp = (ushort)(cpu.A - data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        static void SubReg(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            ushort tmp = (ushort)(cpu.A - data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        static void SubHLR(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, cpu.HL);
            ushort tmp = (ushort)(cpu.A - data);
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        #endregion

        #region Subtraction with carry

        static void SubC(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            ushort tmp = (ushort)(cpu.A - data - (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        static void SubCReg(V16CPU cpu)
        {
            byte src = Memory.LoadByte(cpu.Memory, (ushort)(cpu.IP + 1));
            byte data = cpu.GetRegister8((RegisterDestination8)src);
            ushort tmp = (ushort)(cpu.A - data - (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        static void SubCHLR(V16CPU cpu)
        {
            byte data = Memory.LoadByte(cpu.Memory, cpu.HL);
            ushort tmp = (ushort)(cpu.A - data - (cpu.Carry ? 1 : 0));
            cpu.A = (byte)tmp;
            cpu.Carry = tmp > 0xFF;
            cpu.Zero = tmp == 0;
            cpu.Sub = true;
        }

        #endregion
    }
}
