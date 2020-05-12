using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QinDevilConsoleTest {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            Test1();
        }
        private static void Test1() {
            List<byte> b = new List<byte>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++) {
                b.Add((byte)i);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds+"ms");
            stopwatch.Restart();
            for (int i = 0; i < 100000; i++) {
                b.RemoveRange(0, 1);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
