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
        private readonly List<GameData> gameData;
        private readonly List<byte> sendData = new List<byte>();
        private readonly CCKeyboardHook hook;
        private bool ctrlState;

        public MainWindow() {
            InitializeComponent();
            gameData = new List<GameData> {
                new GameData()
            };
            hook = new CCKeyboardHook();
            hook.KeyDownEvent += Hook_KeyDownEvent;
            hook.KeyUpEvent += Hook_KeyUpEvent;
            server = new CCSocketServer();
            server.OnAcceptSuccessEvent += Server_OnAcceptSuccessEvent;
            server.OnReceivePackageEvent += Server_OnReceivePackageEvent;
            server.OnLeaveEvent += Server_OnLeaveEvent;
            server.Start(11248);
        }
        private void Hook_KeyUpEvent(CCKeyCode keyCode) {
            switch (keyCode) {
                case CCKeyCode.VK_LCONTROL:
                    ctrlState = false;
                    break;
                default:
                    break;
            }
        }
        private void Hook_KeyDownEvent(CCKeyCode keyCode) {
            switch (keyCode) {
                case CCKeyCode.VK_LCONTROL:
                    ctrlState = true;
                    break;
                case CCKeyCode.Numeric1:
                    gameData[0].LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog(0);
                        node.Value.Content = "数字键 1被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData[0].Log.AddLast(node);
                    } finally {
                        gameData[0].LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData[0].HitQinKeyLength < gameData[0].HitQinKey.Length) {
                            gameData[0].HitQinKey[gameData[0].HitQinKeyLength++] = 1;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData[0].HitQinKey);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric2:
                    gameData[0].LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog(0);
                        node.Value.Content = "数字键 2被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData[0].Log.AddLast(node);
                    } finally {
                        gameData[0].LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData[0].HitQinKeyLength < gameData[0].HitQinKey.Length) {
                            gameData[0].HitQinKey[gameData[0].HitQinKeyLength++] = 2;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData[0].HitQinKey);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric3:
                    gameData[0].LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog(0);
                        node.Value.Content = "数字键 3被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData[0].Log.AddLast(node);
                    } finally {
                        gameData[0].LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData[0].HitQinKeyLength < gameData[0].HitQinKey.Length) {
                            gameData[0].HitQinKey[gameData[0].HitQinKeyLength++] = 3;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData[0].HitQinKey);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric4:
                    gameData[0].LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog(0);
                        node.Value.Content = "数字键 4被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData[0].Log.AddLast(node);
                    } finally {
                        gameData[0].LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData[0].HitQinKeyLength < gameData[0].HitQinKey.Length) {
                            gameData[0].HitQinKey[gameData[0].HitQinKeyLength++] = 4;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData[0].HitQinKey);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric5:
                    gameData[0].LogLock.EnterWriteLock();
                    try {
                        LinkedListNode<LogDetail> node = PopLog(0);
                        node.Value.Content = "数字键 5被按下，" + (ctrlState ? "按下了ctrl。" : "没按ctrl。");
                        node.Value.Time = Environment.TickCount;
                        gameData[0].Log.AddLast(node);
                    } finally {
                        gameData[0].LogLock.ExitWriteLock();
                    }
                    if (ctrlState) {
                        if (gameData[0].HitQinKeyLength < gameData[0].HitQinKey.Length) {
                            gameData[0].HitQinKey[gameData[0].HitQinKeyLength++] = 5;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                server.SendPackage(userInfo.Id, 2, gameData[0].HitQinKey);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                case CCKeyCode.Numeric7:
                    if (ctrlState) {
                        gameData[0].LogLock.EnterWriteLock();
                        try {
                            LinkedListNode<LogDetail> node = PopLog(0);
                            node.Value.Content = "补弦清屏---------------------";
                            node.Value.Time = Environment.TickCount;
                            gameData[0].Log.AddLast(node);
                        } finally {
                            gameData[0].LogLock.ExitWriteLock();
                        }
                        gameData[0].No1Qin = gameData[0].No2Qin = gameData[0].No3Qin = gameData[0].No4Qin = "";
                        for (int i = 0; i < gameData[0].HitQinKey.Length; i++) {
                            gameData[0].HitQinKey[i] = 0;
                        }
                        gameData[0].HitQinKeyLength = 0;
                        for (int i = 0; i < 12; i++) {
                            gameData[0].QinKey[i] = 0;
                        }
                        gameData[0].ClientInfoLock.EnterReadLock();
                        try {
                            byte[] intByte = new byte[4];
                            foreach (UserInfo userInfo in gameData[0].ClientInfo) {
                                CCSerializeTool.IntToByte(userInfo.Id, intByte, 0);
                                server.SendPackage(userInfo.Id, 3, intByte);
                            }
                        } finally {
                            gameData[0].ClientInfoLock.ExitReadLock();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private object Server_OnAcceptSuccessEvent(int id) {
            return Dispatcher.Invoke(() => {
                gameData[0].LogLock.EnterWriteLock();
                try {
                    LinkedListNode<LogDetail> node = ExpandLog(0);
                    node.Value.Content = "客户 " + id.ToString() + "进入。";
                    node.Value.Time = Environment.TickCount;
                    gameData[0].Log.AddLast(node);
                } finally {
                    gameData[0].LogLock.ExitWriteLock();
                }
                gameData[0].ClientInfoLock.EnterWriteLock();
                try {
                    return gameData[0].ClientInfo.AddLast(new UserInfo() {
                        Id = id,
                        LastReceiveTime = DateTime.Now
                    });
                } finally {
                    gameData[0].ClientInfoLock.ExitWriteLock();
                }
            });
        }
        private void Server_OnReceivePackageEvent(int id, int signal, byte[] buffer, object userToken) {
            if (userToken is LinkedListNode<UserInfo> userInfoNode) {
                UserInfo userInfo = userInfoNode.Value;
                userInfo.LastReceiveTime = DateTime.Now;
                int startIndex = 0;
                byte[] data;
                switch (signal) {
                    case 0:
                        int line = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                        lock (gameData) {
                            int i = 0;
                            for (; i < gameData.Count; i++) {
                                if (line == gameData[i].Line) {
                                    if (userInfo.Line != i) {
                                        gameData[userInfo.Line].ClientInfoLock.EnterWriteLock();
                                        try {
                                            gameData[userInfo.Line].ClientInfo.Remove(userInfoNode);
                                        } finally {
                                            gameData[userInfo.Line].ClientInfoLock.ExitWriteLock();
                                        }
                                        userInfo.Line = i;
                                        gameData[userInfo.Line].ClientInfoLock.EnterWriteLock();
                                        try {
                                            gameData[userInfo.Line].ClientInfo.AddLast(userInfoNode);
                                        } finally {
                                            gameData[userInfo.Line].ClientInfoLock.ExitWriteLock();
                                        }
                                    }
                                    break;
                                }
                            }
                            if (i == gameData.Count) {
                                gameData.Add(new GameData() {
                                    Line = line
                                });
                                gameData[userInfo.Line].ClientInfoLock.EnterWriteLock();
                                try {
                                    gameData[userInfo.Line].ClientInfo.Remove(userInfoNode);
                                } finally {
                                    gameData[userInfo.Line].ClientInfoLock.ExitWriteLock();
                                }
                                userInfo.Line = i;
                                gameData[userInfo.Line].ClientInfoLock.EnterWriteLock();
                                try {
                                    gameData[userInfo.Line].ClientInfo.AddLast(userInfoNode);
                                } finally {
                                    gameData[userInfo.Line].ClientInfoLock.ExitWriteLock();
                                }
                            }
                        }
                        userInfo.MachineIdentity = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        userInfo.GamePath = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        lock (sendData) {
                            sendData.Clear();
                            CCSerializeTool.IntToByteList(userInfo.Id, sendData);
                            CCSerializeTool.StringToByteList(gameData[userInfo.Line].No1Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData[userInfo.Line].No2Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData[userInfo.Line].No3Qin, sendData);
                            CCSerializeTool.StringToByteList(gameData[userInfo.Line].No4Qin, sendData);
                            for (int i = 0; i < gameData[userInfo.Line].QinKey.Count; i++) {
                                CCSerializeTool.IntToByteList(gameData[userInfo.Line].QinKey[i], sendData);
                            }
                            sendData.AddRange(gameData[userInfo.Line].HitQinKey);
                            server.SendPackage(id, 0, sendData.ToArray());
                        }
                        break;
                    case 1:
                        if (userInfo.GamePath.Equals("") || userInfo.MachineIdentity.Equals("")) {
                            server.SendPackage(id, 4, buffer);
                        } else {
                            server.SendPackage(id, 1, buffer);
                        }
                        break;
                    case 2:
                        gameData[userInfo.Line].No1Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 5, buffer);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        GenerateLog(userInfo.Line, userInfo.Remark + " 修改一号琴缺弦为：" + gameData[userInfo.Line].No1Qin);
                        break;
                    case 3:
                        gameData[userInfo.Line].No2Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 6, buffer);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        GenerateLog(userInfo.Line, userInfo.Remark + " 修改二号琴缺弦为：" + gameData[userInfo.Line].No2Qin);
                        break;
                    case 4:
                        gameData[userInfo.Line].No3Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 7, buffer);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        GenerateLog(userInfo.Line, userInfo.Remark + " 修改三号琴缺弦为：" + gameData[userInfo.Line].No3Qin);
                        break;
                    case 5:
                        gameData[userInfo.Line].No4Qin = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 8, buffer);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        GenerateLog(userInfo.Line, userInfo.Remark + " 修改四号琴缺弦为：" + gameData[userInfo.Line].No4Qin);
                        break;
                    case 6:
                        lock (sendData) {
                            sendData.Clear();
                            int keyIndex = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                            for (int i = 0; i < gameData[userInfo.Line].QinKey.Count; i++) {
                                if (i != keyIndex) {
                                    if (gameData[userInfo.Line].QinKey[i] == userInfo.Id) {
                                        gameData[userInfo.Line].QinKey[i] = 0;
                                    }
                                } else {
                                    if (gameData[userInfo.Line].QinKey[i] == 0) {
                                        gameData[userInfo.Line].QinKey[i] = userInfo.Id;
                                        GenerateLog(userInfo.Line, userInfo.Remark + " 补 " + keyIndex.ToString() + " 琴弦。");
                                    } else if (gameData[userInfo.Line].QinKey[i] == userInfo.Id) {
                                        gameData[userInfo.Line].QinKey[i] = 0;
                                        GenerateLog(userInfo.Line, userInfo.Remark + " 放弃补 " + keyIndex.ToString() + " 琴弦。");
                                    } else {
                                        GenerateLog(userInfo.Line, userInfo.Remark + " 尝试补 " + keyIndex.ToString() + " 琴弦但冲突。");
                                    }
                                }
                                CCSerializeTool.IntToByteList(gameData[userInfo.Line].QinKey[i], sendData);
                            }
                            CCSerializeTool.IntToByteList(keyIndex, sendData);
                            data = sendData.ToArray();
                        }
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 9, data, 0, 48);
                                } else {
                                    server.SendPackage(tempUserInfo.Id, 10, data);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        break;
                    case 7:
                        lock (sendData) {
                            sendData.Clear();
                            int keyIndex = CCSerializeTool.ByteToInt(buffer, ref startIndex);
                            for (int i = 0; i < gameData[userInfo.Line].QinKey.Count; i++) {
                                if (i == keyIndex) {
                                    if (gameData[userInfo.Line].QinKey[i] == 0) {
                                        gameData[userInfo.Line].QinKey[i] = userInfo.Id;
                                        GenerateLog(userInfo.Line, userInfo.Remark + " 强制补 " + keyIndex.ToString() + " 琴弦。");
                                    } else {
                                        gameData[userInfo.Line].QinKey[i] = 0;
                                        GenerateLog(userInfo.Line, userInfo.Remark + " 强制取消补 " + keyIndex.ToString() + " 琴弦。");
                                    }
                                }
                                CCSerializeTool.IntToByteList(gameData[userInfo.Line].QinKey[i], sendData);
                            }
                            CCSerializeTool.IntToByteList(keyIndex, sendData);
                            data = sendData.ToArray();
                        }
                        gameData[userInfo.Line].ClientInfoLock.EnterReadLock();
                        try {
                            foreach (UserInfo tempUserInfo in gameData[userInfo.Line].ClientInfo) {
                                if (tempUserInfo.Id != id) {
                                    server.SendPackage(tempUserInfo.Id, 9, data, 0, 48);
                                } else {
                                    server.SendPackage(tempUserInfo.Id, 10, data);
                                }
                            }
                        } finally {
                            gameData[userInfo.Line].ClientInfoLock.ExitReadLock();
                        }
                        break;
                    case 8:
                        userInfo.MachineIdentity = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        userInfo.GamePath = CCSerializeTool.ByteToString(buffer, ref startIndex);
                        break;
                    default:
                        break;
                }
            }
        }
        private void Server_OnLeaveEvent(int id, object userToken) {
            if (userToken is LinkedListNode<UserInfo> user) {
                Dispatcher.Invoke(() => {
                    gameData[user.Value.Line].ClientInfoLock.EnterWriteLock();
                    try {
                        gameData[user.Value.Line].ClientInfo.Remove(user);
                    } finally {
                        gameData[user.Value.Line].ClientInfoLock.ExitWriteLock();
                    }
                });
            }
        }
        private LinkedListNode<LogDetail> ExpandLog(int i) {
            if (gameData[i].LogBack.Count < 15) {
                for (int j = 0; j < 15; j++) {
                    gameData[i].LogBack.Push(new LinkedListNode<LogDetail>(new LogDetail()));
                }
            }
            return gameData[i].LogBack.Pop();
        }
        private LinkedListNode<LogDetail> PopLog(int i) {
            return gameData[i].LogBack.Count > 0 ? gameData[i].LogBack.Pop() : new LinkedListNode<LogDetail>(new LogDetail());
        }
        private void GenerateLog(int i, string content) {
            gameData[i].LogLock.EnterWriteLock();
            try {
                LinkedListNode<LogDetail> node = ExpandLog(i);
                node.Value.Content = content;
                node.Value.Time = Environment.TickCount;
                gameData[i].Log.AddLast(node);
            } finally {
                gameData[i].LogLock.ExitWriteLock();
            }
        }
    }
}