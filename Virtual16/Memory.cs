using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual16
{
    static class Memory
    {
        public static unsafe void StoreWord(IntPtr mem, ushort offset, ushort data)
        {
            ushort* ptr = (ushort*)((byte*)mem.ToPointer() + offset);
            *ptr = data;
        }

        public static unsafe void StoreByte(IntPtr mem, ushort offset, byte data)
        {
            byte* ptr = (byte*)((byte*)mem.ToPointer() + offset);
            *ptr = data;
        }

        public static unsafe ushort LoadWord(IntPtr mem, ushort offset)
        {
            ushort* ptr = (ushort*)((byte*)mem.ToPointer() + offset);
            return *ptr;
        }

        public static unsafe byte LoadByte(IntPtr mem, ushort offset)
        {
            byte* ptr = (byte*)((byte*)mem.ToPointer() + offset);
            return *ptr;
        }
    }
}
