using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCode.CCserialize {
    public class CCSerializeTool {
        public static void IntToByte(int i, byte[] rawdatas, int startIndex) {
            rawdatas[startIndex++] = (byte)i;
            rawdatas[startIndex++] = (byte)(i >> 8);
            rawdatas[startIndex++] = (byte)(i >> 16);
            rawdatas[startIndex++] = (byte)(i >> 24);
        }
        public static byte[] IntToByte(int i) {
            byte[] rawdatas = new byte[4];
            rawdatas[0] = (byte)i;
            rawdatas[1] = (byte)(i >> 8);
            rawdatas[2] = (byte)(i >> 16);
            rawdatas[3] = (byte)(i >> 24);
            return rawdatas;
        }
        public static void IntToByteList(int i, List<byte> list) {
            list.Add((byte)i);
            list.Add((byte)(i >> 8));
            list.Add((byte)(i >> 16));
            list.Add((byte)(i >> 24));
        }
        public static int ByteToInt(byte[] rawdatas, ref int startIndex) {
            return rawdatas[startIndex++] | (rawdatas[startIndex++] << 8) | (rawdatas[startIndex++] << 16) | (rawdatas[startIndex++] << 24);
        }
        public static void StringToByteList(string str, List<byte> list) {
            byte[] rawdatas = Encoding.UTF8.GetBytes(str);
            list.Add((byte)rawdatas.Length);
            list.Add((byte)(rawdatas.Length >> 8));
            list.Add((byte)(rawdatas.Length >> 16));
            list.Add((byte)(rawdatas.Length >> 24));
            list.AddRange(rawdatas);
        }
        public static byte[] StringToByte(string str) {
            byte[] rawdatas = new byte[4 + Encoding.UTF8.GetByteCount(str)];
            int len = rawdatas.Length - 4;
            rawdatas[0] = (byte)len;
            rawdatas[1] = (byte)(len >> 8);
            rawdatas[2] = (byte)(len >> 16);
            rawdatas[3] = (byte)(len >> 24);
            _ = Encoding.UTF8.GetBytes(str, 0, str.Length, rawdatas, 4);
            return rawdatas;
        }
        public static string ByteToString(byte[] rawdatas, ref int startIndex) {
            int length = rawdatas[startIndex++]
                | (rawdatas[startIndex++] >> 8)
                | (rawdatas[startIndex++] >> 16)
                | (rawdatas[startIndex++] >> 24);
            string str = Encoding.UTF8.GetString(rawdatas, startIndex, length);
            startIndex += length;
            return str;
        }
    }
}
