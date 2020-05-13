using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonCode.CCsystem {
    public class CCProcessInfo {
        [DllImport("Kernel32.dll", EntryPoint = "QueryFullProcessImageNameA")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryFullProcessImageNameA(IntPtr hProcess, int dwFlags, [Out] StringBuilder lpExeName, ref int lpdwSize);
        public static string GetProcessPath(Process process) {
            if (process != null) {
                int i = 0;
                int length;
                StringBuilder stringBuilder;
                do {
                    i++;
                    length = i * 260;
                    stringBuilder = new StringBuilder(length);
                    _ = QueryFullProcessImageNameA(process.Handle, 0, stringBuilder, ref length);
                    if (length == 0) {
                        return "";
                    }
                } while (i * 260 == length);
                return stringBuilder.ToString();
            } else {
                return "";
            }
        }
    }
}