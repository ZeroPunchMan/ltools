using System;
namespace Helper
{
    class Serialize
    {
        public enum Endian
        {
            Big,
            Little
        }

        public static UInt32 BytesToUint32(byte[] arr, int offset, Endian endian)
        {
            UInt32 number = 0;
            if (endian == Endian.Big)
            {
                number = (UInt32)((UInt32)arr[offset] << 24);
                number |= (UInt32)((UInt32)arr[offset + 1] << 16);
                number |= (UInt32)((UInt32)arr[offset + 2] << 8);
                number |= (UInt32)((UInt32)arr[offset + 3]);
            }
            else if (endian == Endian.Little)
            {
                number = (UInt32)((UInt32)arr[offset + 3] << 24);
                number |= (UInt32)((UInt32)arr[offset + 2] << 16);
                number |= (UInt32)((UInt32)arr[offset + 1] << 8);
                number |= (UInt32)((UInt32)arr[offset]);
            }
            return number;
        }

        public static UInt16 BytesToUint16(byte[] arr, int offset, Endian endian)
        {
            UInt16 number = 0;
            if (endian == Endian.Big)
            {
                number = (UInt16)((UInt16)arr[offset] << 8);
                number |= (UInt16)((UInt16)arr[offset + 1]);
            }
            else if (endian == Endian.Little)
            {
                number = (UInt16)((UInt16)arr[offset + 1] << 8);
                number |= (UInt16)((UInt16)arr[offset]);
            }
            return number;
        }

        public static void Uint32ToBytes(UInt32 number, byte[] arr, int offset, Endian endian)
        {
            if (endian == Endian.Big)
            {
                arr[offset] = (byte)((number >> 24) & 0xff);
                arr[offset + 1] = (byte)((number >> 16) & 0xff);
                arr[offset + 2] = (byte)((number >> 8) & 0xff);
                arr[offset + 3] = (byte)((number >> 0) & 0xff);
            }
            else if (endian == Endian.Little)
            {
                arr[offset + 3] = (byte)((number >> 24) & 0xff);
                arr[offset + 2] = (byte)((number >> 16) & 0xff);
                arr[offset + 1] = (byte)((number >> 8) & 0xff);
                arr[offset] = (byte)((number >> 0) & 0xff);
            }
        }

        public static void Uint16ToBytes(UInt16 number, byte[] arr, int offset, Endian endian)
        {
            if (endian == Endian.Big)
            {
                arr[offset] = (byte)((number >> 8) & 0xff);
                arr[offset + 1] = (byte)((number) & 0xff);
            }
            else if (endian == Endian.Little)
            {
                arr[offset + 1] = (byte)((number >> 8) & 0xff);
                arr[offset] = (byte)((number) & 0xff);
            }
        }
    }
}