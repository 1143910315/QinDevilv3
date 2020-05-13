using CommonCode.CClog;
using CommonCode.CCnetwork;
using CommonCode.CCserialize;
using CommonCode.CCsystem;
using QinDevilClient.DataInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QinDevilClient {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly LogFile log;
        private readonly CCSocketClient client;
        private readonly Timer connectTimer;
        private readonly Timer sendBaseInfoTimer;
        private readonly Timer pingTimer;
        private readonly GameData gameData;
        private readonly byte[] machineIdentity;
        private readonly int line;
        private readonly List<byte> sendData;
        private bool startPing;
        private int lastPing;

        public MainWindow() {
            string methodMD5 = "43DFFFDA19287556";
            try {
                log = new LogFile(".\\工具日志.log");
                log.Generate(methodMD5 + " 进入");
                InitializeComponent();
                gameData = new GameData();
#if DEBUG
                line = 0;
#endif
                sendData = new List<byte>();
                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(CCSystemInfo.GetMacAddress() + CCSystemInfo.GetCpuID()));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) {
                    _ = sb.Append(b.ToString("X2"));
                }
                machineIdentity = CCSerializeTool.StringToByte(sb.ToString());
                client = new CCSocketClient();
                client.OnConnectedEvent += Client_OnConnectedEvent;
                client.OnConnectionBreakEvent += Client_OnConnectionBreakEvent;
                client.OnReceivePackageEvent += Client_OnReceivePackageEvent;
                client.OnSocketExceptionEvent += Client_OnSocketExceptionEvent;
                connectTimer = new Timer();
                connectTimer.Elapsed += ConnectTimer_Elapsed;
                connectTimer.AutoReset = false;
                sendBaseInfoTimer = new Timer();
                sendBaseInfoTimer.Elapsed += SendBaseInfoTimer_Elapsed; ;
                sendBaseInfoTimer.AutoReset = false;
                sendBaseInfoTimer.Interval = 3000;
                pingTimer = new Timer();
                pingTimer.Elapsed += PingTimer_Elapsed;
                pingTimer.Interval = 150;
                pingTimer.AutoReset = true;
                Connect();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void PingTimer_Elapsed(object sender, ElapsedEventArgs e) {
            string methodMD5 = "3B8C60148B2D1D2F";
            try {
                log.Generate(methodMD5 + " 进入");
                if (startPing) {
                    int ping = Environment.TickCount - lastPing;
                    if (ping > gameData.Ping) {
                        gameData.Ping = ping > 9999 ? 9999 : (ping < 0 ? 9999 : ping);
                        if (gameData.Ping == 9999) {
                            Connect();
                        }
                    }
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void SendBaseInfoTimer_Elapsed(object sender, ElapsedEventArgs e) {
            string methodMD5 = "B5BE9A7B29C08AD2";
            try {
                log.Generate(methodMD5 + " 进入");
                if (gameData.SendInfoSuccess) {
                    lock (sendData) {
                        sendData.Clear();
                        if (startPing == false) {
                            lastPing = Environment.TickCount;
                            startPing = true;
                            CCSerializeTool.IntToByteList(lastPing, sendData);
                        } else {
                            CCSerializeTool.IntToByteList(Environment.TickCount, sendData);
                        }
                        client.SendPackage(1, sendData.ToArray());
                    }
                } else {
                    lock (sendData) {
                        sendData.Clear();
                        CCSerializeTool.IntToByteList(line, sendData);
                        sendData.AddRange(machineIdentity);
                        CCSerializeTool.StringToByteList(CCProcessInfo.GetProcessPath(GetWuXiaProcess()), sendData);
                        client.SendPackage(0, sendData.ToArray());
                    }
                }
                sendBaseInfoTimer.Start();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void ConnectTimer_Elapsed(object sender, ElapsedEventArgs e) {
            string methodMD5 = "E294EBA3D8F45A16";
            try {
                log.Generate(methodMD5 + " 进入");
                Connect();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Connect() {
            client.Connect("127.0.0.1", 11248);
        }
        private void Client_OnConnectedEvent(bool connected) {
            string methodMD5 = "6187DF87D0840A5C";
            try {
                log.Generate(methodMD5 + " 进入");
                if (connected) {
                    sendBaseInfoTimer.Start();
                } else {
                    connectTimer.Interval = 3000;
                    connectTimer.Start();
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Client_OnConnectionBreakEvent() {
            string methodMD5 = "229D8B1ED98E6D7B";
            try {
                log.Generate(methodMD5 + " 进入");
                sendBaseInfoTimer.Stop();
                pingTimer.Stop();
                if (gameData.SendInfoSuccess) {
                    connectTimer.Interval = 100;
                } else {
                    connectTimer.Interval = 3000;
                }
                gameData.SendInfoSuccess = false;
                connectTimer.Start();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Client_OnReceivePackageEvent(int signal, byte[] buffer) {
            string methodMD5 = "3C8905B3EE89BB1A";
            try {
                log.Generate(methodMD5 + " 进入");
                int startIndex = 0;
                switch (signal) {
                    case 0:
                        gameData.SendInfoSuccess = true;
                        int licence = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        if (!gameData.Licence.Contains(licence)) {
                            gameData.Licence.Add(licence);
                        }
                        gameData.No1Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData.No2Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData.No3Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData.No4Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        for (int i = 0; i < 12; i++) {
                            gameData.QinKey[i] = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        }
                        gameData.QinKey = gameData.QinKey;
                        for (int i = 0; i < gameData.HitQinKey.Length; i++) {
                            gameData.HitQinKey[i] = buffer[startIndex++];
                        }
                        gameData.HitQinKey = gameData.HitQinKey;
                        pingTimer.Start();
                        break;
                    case 1:
                        int ping = Environment.TickCount - CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        startPing = false;
                        gameData.Ping = ping > 9999 ? 9999 : (ping < 0 ? 9999 : ping);
                        break;
                    case 2:
                        for (int i = 0; i < gameData.HitQinKey.Length; i++) {
                            gameData.HitQinKey[i] = buffer[startIndex++];
                        }
                        break;
                    case 3:
                        gameData.Licence.Clear();
                        gameData.Licence.Add(CCSerializeTool.ByteToInt(buffer, ref startIndex));
                        gameData.No1Qin = gameData.No2Qin = gameData.No3Qin = gameData.No4Qin = "";
                        for (int i = 0; i < gameData.QinKey.Count; i++) {
                            gameData.QinKey[i] = 0;
                        }
                        gameData.QinKey = gameData.QinKey;
                        for (int i = 0; i < gameData.HitQinKey.Length; i++) {
                            gameData.HitQinKey[i] = 0;
                        }
                        gameData.HitKeyIndex = 0;
                        break;
                    default:
                        break;
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }

        private void Client_OnSocketExceptionEvent(System.Net.Sockets.SocketException socketException) {
            log.Generate("网络异常，异常信息：" + socketException.Message + "；错误代码：" + socketException.ErrorCode.ToString());
            log.Flush();
        }
        private Process GetWuXiaProcess() {
            Process[] process = Process.GetProcessesByName("WuXia_Client_x64");
            if (process.Length == 0) {
                process = Process.GetProcessesByName("WuXia_Client");
            }
            if (process.Length > 0) {
                Process temp = process[0];
                if (process.Length > 0) {
                    IntPtr topWindow = CCWindowInfo.GetTopWindow();
                    while (!topWindow.Equals(IntPtr.Zero)) {
                        for (int i = 0; i < process.Length; i++) {
                            if (topWindow.Equals(process[i].MainWindowHandle)) {
                                return process[i];
                            }
                        }
                        topWindow = CCWindowInfo.GetWindow(topWindow, CCWindowInfo.GettingType.GW_HWNDNEXT);
                    }
                }
                return temp;
            }
            return null;
        }
    }
}
/*
string methodMD5="";
try{
log.Generate(methodMD5+" 进入");

}catch(Exception exception){
log.Generate(methodMD5+" 异常，异常信息："+exception.Message);
log.Flush();
throw;
} finally {
log.Generate(methodMD5+" 退出");
}
*/
