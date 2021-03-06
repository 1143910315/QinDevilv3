﻿using CommonCode.CClog;
using CommonCode.CCnetwork;
using CommonCode.CCserialize;
using CommonCode.CCsystem;
using QinDevilClient.DataInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private readonly List<byte> sendData;
        private bool startPing;
        private int lastPing;
        private readonly Regex QinKeyLessMatch = new Regex("^(?![1-5]*?([1-5])[1-5]*?\\1)[1-5]{0,3}$");
        public MainWindow() {
            string methodMD5 = "43DFFFDA19287556";
            try {
                log = new LogFile(".\\工具日志.log");
                log.Generate(methodMD5 + " 进入");
                InitializeComponent();
                gameData = new GameData();
                GamePanel.DataContext = gameData;
#if DEBUG
                gameData.Line = 0;
#else
                gameData.Line = 0;
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
                        CCSerializeTool.IntToByteList(gameData.Line, sendData);
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
                int ping;
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
                        ping = Environment.TickCount - CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        startPing = false;
                        gameData.Ping = ping > 9999 ? 9999 : (ping < 0 ? 9999 : ping);
                        break;
                    case 2:
                        for (int i = 0; i < gameData.HitQinKey.Length; i++) {
                            gameData.HitQinKey[i] = buffer[startIndex++];
                        }
                        gameData.HitQinKey = gameData.HitQinKey;
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
                        gameData.HitQinKey = gameData.HitQinKey;
                        gameData.HitKeyIndex = 0;
                        break;
                    case 4:
                        ping = Environment.TickCount - CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        startPing = false;
                        gameData.Ping = ping > 9999 ? 9999 : (ping < 0 ? 9999 : ping);
                        lock (sendData) {
                            sendData.Clear();
                            sendData.AddRange(machineIdentity);
                            CCSerializeTool.StringToByteList(CCProcessInfo.GetProcessPath(GetWuXiaProcess()), sendData);
                            client.SendPackage(8, sendData.ToArray());
                        }
                        break;
                    case 5:
                        gameData.No1Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        break;
                    case 6:
                        gameData.No2Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        break;
                    case 7:
                        gameData.No3Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        break;
                    case 8:
                        gameData.No4Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        break;
                    case 9:
                        for (int i = 0; i < 12; i++) {
                            gameData.QinKey[i] = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        }
                        gameData.QinKey = gameData.QinKey;
                        break;
                    case 10:
                        for (int i = 0; i < 12; i++) {
                            gameData.QinKey[i] = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        }
                        gameData.QinKey = gameData.QinKey;
                        int keyIndex = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        if (!gameData.Licence.Contains(gameData.QinKey[keyIndex])) {
                            SystemSounds.Asterisk.Play();
                            Dispatcher.Invoke(() => {
                                Storyboard Storyboard1 = FindResource("Storyboard1") as Storyboard;
                                Storyboard1.Stop();
                                Storyboard.SetTargetName(Storyboard1, "OneKey" + keyIndex.ToString());
                                Storyboard1.Begin();
                            });
                        }
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
        private void Viewbox_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "E8505C9B1743CDD0";
            try {
                log.Generate(methodMD5 + " 进入");
                DragMove();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "3C8338770839B771";
            try {
                log.Generate(methodMD5 + " 进入");
                Close();
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "B566C783C8694E3C";
            try {
                log.Generate(methodMD5 + " 进入");
                if (sender is Image senderImage) {
                    _ = senderImage.CaptureMouse();
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Image_MouseMove(object sender, MouseEventArgs e) {
            string methodMD5 = "E00475FA828DDADD";
            try {
                log.Generate(methodMD5 + " 进入");
                if (sender is Image senderImage && senderImage.IsMouseCaptured) {
                    Width = e.GetPosition(this).X;
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Image_MouseUp(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "889F374E5FBA2E9D";
            try {
                log.Generate(methodMD5 + " 进入");
                if (sender is Image senderImage) {
                    senderImage.ReleaseMouseCapture();
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void Image_MouseMove_1(object sender, MouseEventArgs e) {
            string methodMD5 = "23EE0B7A7656B0F0";
            try {
                log.Generate(methodMD5 + " 进入");
                if (sender is Image senderImage && senderImage.IsMouseCaptured) {
                    Width = (e.GetPosition(this).Y - 1.031746031746) / 0.54263565891473;
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            string methodMD5 = "923DAF4461C60BE5";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.Source is TextBox sourceTextBox) {
                    e.Handled = !QinKeyLessMatch.IsMatch(sourceTextBox.Text.Remove(sourceTextBox.SelectionStart, sourceTextBox.SelectionLength).Insert(sourceTextBox.SelectionStart, e.Text));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            string methodMD5 = "8F6EFD6C25959455";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.Key == Key.Space) {
                    e.Handled = true;
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_SourceUpdated(object sender, DataTransferEventArgs e) {
            string methodMD5 = "131A0AA1D2AA4882";
            try {
                log.Generate(methodMD5 + " 进入");
                client.SendPackage(2, CCSerializeTool.StringToByte(gameData.No1Qin));
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_SourceUpdated_1(object sender, DataTransferEventArgs e) {
            string methodMD5 = "CE8B5465A949746A";
            try {
                log.Generate(methodMD5 + " 进入");
                client.SendPackage(3, CCSerializeTool.StringToByte(gameData.No2Qin));
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_SourceUpdated_2(object sender, DataTransferEventArgs e) {
            string methodMD5 = "5B7E6DC97151E091";
            try {
                log.Generate(methodMD5 + " 进入");
                client.SendPackage(4, CCSerializeTool.StringToByte(gameData.No3Qin));
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void TextBox_SourceUpdated_3(object sender, DataTransferEventArgs e) {
            string methodMD5 = "7CF22B3EA1783029";
            try {
                log.Generate(methodMD5 + " 进入");
                client.SendPackage(5, CCSerializeTool.StringToByte(gameData.No4Qin));
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey0_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "852F45F93618118A";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(0));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(0));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey1_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "C66B1C1776E5BEA9";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(1));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(1));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey2_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "5E1D49BFFBC79375";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(2));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(2));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey3_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "801271A76D3D610A";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(3));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(3));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey4_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "6FBF7CC482818493";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(4));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(4));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey5_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "91D8F535D340C7B1";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(5));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(5));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey6_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "8291DA2FF7D206A9";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(6));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(6));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey7_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "125B3D709A1E300F";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(7));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(7));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey8_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "6A81F5ED1960E9E2";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(8));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(8));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey9_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "1A61030A49535EA8";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(9));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(9));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey10_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "1CCA1BE7455CD689";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(10));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(10));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
        }
        private void OneKey11_MouseDown(object sender, MouseButtonEventArgs e) {
            string methodMD5 = "78A7758259355E5D";
            try {
                log.Generate(methodMD5 + " 进入");
                if (e.ChangedButton == MouseButton.Left) {
                    client.SendPackage(6, CCSerializeTool.IntToByte(11));
                } else if (e.ChangedButton == MouseButton.Right) {
                    client.SendPackage(7, CCSerializeTool.IntToByte(11));
                }
            } catch (Exception exception) {
                log.Generate(methodMD5 + " 异常，异常信息：" + exception.Message);
                log.Flush();
                throw;
            } finally {
                log.Generate(methodMD5 + " 退出");
            }
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
