using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonCode.CClog {
    public class LogFile {
        private readonly StreamWriter sw;
        private readonly TextWriter tw;
        public LogFile(string path) {
            sw = File.AppendText(path);
            tw = TextWriter.Synchronized(sw);
        }
        public void Generate(string log) {
            tw.WriteLineAsync(log).Wait();
        }
        public void Flush() {
            tw.Flush();
        }
        ~LogFile() {
            tw.Flush();
        }
    }
}
