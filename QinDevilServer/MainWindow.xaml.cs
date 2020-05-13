using CommonCode.CCkeyboard;
using CommonCode.CCnetwork;
using CommonCode.CCserialize;
using QinDevilServer.DataInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QinDevilServer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly CCSocketServer server;
        private readonly GameData gameData;
        private readonly List<byte> sendData = new List<byte>();
        private readonly CCKeyboardHook hook;
        private bool ctrlState;

        public MainWindow() {
            InitializeComponent();
            gameData = new GameData();
            hook = new CCKeyboardHook();
            hook.KeyDownEvent += Hook_KeyDownEvent;
            server = new CCSocketServer();
            server.OnAcceptSuccessEvent += Server_OnAcceptSuccessEvent;
            server.OnReceivePackageEvent += Server_OnReceivePackageEvent;
            server.OnLeaveEvent += Server_OnLeaveEvent;
            server.Start(11248);
        }
        private void Hook_KeyDownEvent(CCKeyCode keyCode) {
            switch (keyCode) {
                case CCKeyCode.VK_LCONTROL:
                    ctrlState = true;
                    break;
                case CCKeyCode.Numeric1:
                    gameData.LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog();
                        node.Value.Content = "数字键 1被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData.Log.AddLast(node);
                    } finally {
                        gameData.LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData.HitQinKeyLength < gameData.HitQinKey.Length) {
                            gameData.HitQinKey[gameData.HitQinKeyLength++] = 1;
                        }
                        gameData.ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData.ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData.HitQinKey);
                            }
                        } finally {
                            gameData.ClientInfoLock.EnterReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric2:
                    gameData.LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog();
                        node.Value.Content = "数字键 2被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData.Log.AddLast(node);
                    } finally {
                        gameData.LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData.HitQinKeyLength < gameData.HitQinKey.Length) {
                            gameData.HitQinKey[gameData.HitQinKeyLength++] = 2;
                        }
                        gameData.ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData.ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData.HitQinKey);
                            }
                        } finally {
                            gameData.ClientInfoLock.EnterReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric3:
                    gameData.LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog();
                        node.Value.Content = "数字键 3被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData.Log.AddLast(node);
                    } finally {
                        gameData.LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData.HitQinKeyLength < gameData.HitQinKey.Length) {
                            gameData.HitQinKey[gameData.HitQinKeyLength++] = 3;
                        }
                        gameData.ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData.ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData.HitQinKey);
                            }
                        } finally {
                            gameData.ClientInfoLock.EnterReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric4:
                    gameData.LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog();
                        node.Value.Content = "数字键 4被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData.Log.AddLast(node);
                    } finally {
                        gameData.LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData.HitQinKeyLength < gameData.HitQinKey.Length) {
                            gameData.HitQinKey[gameData.HitQinKeyLength++] = 4;
                        }
                        gameData.ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData.ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData.HitQinKey);
                            }
                        } finally {
                            gameData.ClientInfoLock.EnterReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric5:
                    gameData.LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog();
                        node.Value.Content = "数字键 5被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData.Log.AddLast(node);
                    } finally {
                        gameData.LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData.HitQinKeyLength < gameData.HitQinKey.Length) {
                            gameData.HitQinKey[gameData.HitQinKeyLength++] = 5;
                        }
                        gameData.ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData.ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData.HitQinKey);
                            }
                        } finally {
                            gameData.ClientInfoLock.EnterReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric7: {
                        if (ctrlState) {
                            gameData.LogLock.EnterWriteLock();
                            try {
                                LinkedListNode<LogDetail> node = PopLog();
                                node.Value.Content = "补弦清屏---------------------");
                                node.Value.Time = Environment.TickCount;
                                gameData.Log.AddLast(node);
                            } finally {
                                gameData.LogLock.ExitWriteLock();
                            }
                            gameData.No1Qin = gameData.No2Qin = gameData.No3Qin = gameData.No4Qin = "";
                            for (int i = 0; i < gameData.HitQinKey.Length; i++) {
                                gameData.HitQinKey[i] = 0;
                            }
                            gameData.HitQinKeyLength = 0;
                            for (int i = 0; i < 12; i++) {
                                gameData.QinKey[i] = 0;
                            }
                            gameData.ClientInfoLock.EnterReadLock();
                            try {
                                byte[] intByte = new byte[4];
                                foreach (UserInfo userInfo in gameData.ClientInfo) {
                                    CCSerializeTool.IntToByte(userInfo.Id, intByte, 0);
                                    server.SendPackage(userInfo.Id, 3, intByte);
                                }
                            } finally {
                                gameData.ClientInfoLock.EnterReadLock();
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        private object Server_OnAcceptSuccessEvent(int id) {
            return Dispatcher.Invoke(() => {
                gameData.LogLock.EnterWriteLock();
                try {
                    LinkedListNode<LogDetail> node = ExpandLog();
                    node.Value.Content = "客户 " + id.ToString() + "进入。";
                    node.Value.Time = Environment.TickCount;
                    gameData.Log.AddLast(node);
                } finally {
                    gameData.LogLock.ExitWriteLock();
                }
                gameData.ClientInfoLock.EnterWriteLock();
                try {
                    return gameData.ClientInfo.AddLast(new UserInfo() {
                        Id = id,
                        LastReceiveTime = DateTime.Now
                    });
                } finally {
                    gameData.ClientInfoLock.ExitWriteLock();
                }
            });
        }
        private void Server_OnReceivePackageEvent(int id, int signal, byte[] buffer, object userToken) {
            if (userToken is LinkedListNode<UserInfo> userInfoNode) {
                UserInfo userInfo = userInfoNode.Value;
                userInfo.LastReceiveTime = DateTime.Now;
                int startIndex = 0;
                switch (signal) {
                    case 0:
                        userInfo.Line = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        userInfo.MachineIdentity = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        userInfo.GamePath = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        lock (sendData) {
                            sendData.Clear();
                            CCSerializeTool.IntToByteList(userInfo.Id, sendData);
                            CCSerializeTool.StringToByteList(gameData.No1Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData.No2Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData.No3Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData.No4Qin, sendData);
                            for (int i = 0; i < gameData.QinKey.Count; i++) {
                                CCSerializeTool.IntToByteList(gameData.QinKey[i], sendData);
                            }
                            sendData.AddRange(gameData.HitQinKey);
                            server.SendPackage(id, 0, sendData.ToArray());
                        }
                        break;
                    case 1:
                        server.SendPackage(id, 1, buffer);
                        break;
                    default:
                        break;
                }
            }
        }
        private void Server_OnLeaveEvent(int id, object userToken) {
            if (userToken is LinkedListNode<UserInfo> user) {
                Dispatcher.Invoke(() => {
                    gameData.ClientInfoLock.EnterWriteLock();
                    try {
                        gameData.ClientInfo.Remove(user);
                    } finally {
                        gameData.ClientInfoLock.ExitWriteLock();
                    }
                });
            }
        }
        private LinkedListNode<LogDetail> ExpandLog() {
            if (gameData.LogBack.Count < 15) {
                for (int i = 0; i < 15; i++) {
                    gameData.LogBack.Push(new LinkedListNode<LogDetail>(new LogDetail()));
                }
            }
            return gameData.LogBack.Pop();
        }
        private LinkedListNode<LogDetail> PopLog() {
            return gameData.LogBack.Count > 0 ? gameData.LogBack.Pop() : new LinkedListNode<LogDetail>(new LogDetail());
        }
    }
}
