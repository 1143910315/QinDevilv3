﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommonCode.CCnetwork {
    public class CCSocketServer {
        private struct ClientStruct {
            public byte[] recvBuffer;
            public byte[] sendBuffer;
            public object userToken;
            public List<byte> recvData;
            public List<byte> sendData;
            public Socket s;
            public int id;
            public byte state;
            public SocketAsyncEventArgs[] sendEventArgs;
        }
        public delegate object OnAcceptSuccessEvent(int id);
        public delegate void OnLeaveEvent(int id, object userToken);
        public delegate void OnReceivePackageEvent(int id, int signal, byte[] buffer, object userToken);
        public OnAcceptSuccessEvent onAcceptSuccessEvent = null;
        public OnLeaveEvent onLeaveEvent = null;
        public OnReceivePackageEvent onReceivePackageEvent = null;
        private Socket socket;
        private readonly Hashtable socketHashtable = new Hashtable();
        private int connectNum = 1;
        private bool repeat = false;
        public CCSocketServer() {
        }
        public void Start(int port) {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(10);
            SocketAsyncEventArgs acceptEventArgs = new SocketAsyncEventArgs();
            acceptEventArgs.Completed += AcceptEventArgs_Completed;
            if (!socket.AcceptAsync(acceptEventArgs)) {
                AcceptEventArgs_Completed(socket, acceptEventArgs);
            }
        }
        private void AcceptEventArgs_Completed(object sender, SocketAsyncEventArgs e) {
            SocketAsyncEventArgs acceptEventArgs;
            SocketAsyncEventArgs receiveEventArgs;
            switch (e.SocketError) {
                case SocketError.Success:
                    if (repeat) {
                        lock (socketHashtable) {
                            while (socketHashtable.ContainsKey(connectNum)) {
                                connectNum++;
                            }
                        }
                    } else {
                        if (connectNum < 0) {
                            repeat = true;
                        }
                    }
                    ClientStruct client = new ClientStruct {
                        id = connectNum,
                        s = e.AcceptSocket,
                        sendBuffer = new byte[255],
                        recvBuffer = new byte[255],
                        sendData = new List<byte>(),
                        recvData = new List<byte>(),
                        userToken = onAcceptSuccessEvent?.Invoke(connectNum),
                        state = 0,
                        sendEventArgs = new SocketAsyncEventArgs[2]
                    };
                    for (int i = 0; i < 2; i++) {
                        SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
                        sendEventArgs.Completed += SendEventArgs_Completed;
                        client.sendEventArgs[i] = sendEventArgs;
                    }
                    lock (socketHashtable) {
                        socketHashtable.Add(connectNum++, client);
                    }
                    receiveEventArgs = new SocketAsyncEventArgs();
                    receiveEventArgs.Completed += ReceiveEventArgs_Completed;
                    receiveEventArgs.UserToken = client;
                    receiveEventArgs.SetBuffer(client.recvBuffer, 0, client.recvBuffer.Length);
                    if (!socket.ReceiveAsync(receiveEventArgs)) {
                        ReceiveEventArgs_Completed(socket, receiveEventArgs);
                    }
                    acceptEventArgs = new SocketAsyncEventArgs();
                    acceptEventArgs.Completed += AcceptEventArgs_Completed;
                    if (!socket.AcceptAsync(acceptEventArgs)) {
                        AcceptEventArgs_Completed(socket, acceptEventArgs);
                    }
                    break;
                case SocketError.ConnectionReset:
                    acceptEventArgs = new SocketAsyncEventArgs();
                    acceptEventArgs.Completed += AcceptEventArgs_Completed;
                    if (!socket.AcceptAsync(acceptEventArgs)) {
                        AcceptEventArgs_Completed(socket, acceptEventArgs);
                    }
                    break;
            }
        }
        private void ReceiveEventArgs_Completed(object sender, SocketAsyncEventArgs e) {
            if (e.UserToken is ClientStruct client) {
                try {
                    int len = e.BytesTransferred;
                    if (e.SocketError == SocketError.Success && len > 0) {
                        if (client.recvData.Capacity < client.recvData.Count + len) {
                            client.recvData.Capacity = client.recvData.Count + len;
                        }
                        for (int i = 0; i < len; i++) {
                            client.recvData.Add(client.recvBuffer[i + e.Offset]);
                        }
                        while (client.recvData.Count >= 8) {
                            int dataLen = client.recvData[0] | (client.recvData[1] >> 8) | (client.recvData[2] >> 16) | (client.recvData[3] >> 24);
                            if (client.recvData.Count - 4 >= dataLen) {
                                int signal = client.recvData[4] | (client.recvData[5] >> 8) | (client.recvData[6] >> 16) | (client.recvData[7] >> 24);
                                onReceivePackageEvent?.Invoke(client.id, signal, client.recvData.GetRange(8, dataLen - 4).ToArray(), client.userToken);
                                client.recvData.RemoveRange(0, dataLen + 4);
                            } else {
                                break;
                            }
                        }
                        SocketAsyncEventArgs receiveEventArgs = new SocketAsyncEventArgs();
                        receiveEventArgs.Completed += ReceiveEventArgs_Completed;
                        receiveEventArgs.UserToken = client;
                        receiveEventArgs.SetBuffer(client.recvBuffer, 0, client.recvBuffer.Length);
                        if (!client.s.ReceiveAsync(receiveEventArgs)) {
                            ReceiveEventArgs_Completed(sender, receiveEventArgs);
                        }
                    } else {
                        throw new Exception("套接字发生错误！");
                    }
                } catch (Exception) {
                    lock (socketHashtable) {
                        socketHashtable.Remove(client.id);
                    }
                    client.s.Close();
                    onLeaveEvent?.Invoke(client.id, client.userToken);
                }
            }
        }
        private void SendEventArgs_Completed(object sender, SocketAsyncEventArgs e) {
            if (e.UserToken is ClientStruct client) {
                lock (client.sendData) {
                    try {
                        if (e.SocketError == SocketError.Success) {
                            int len = client.sendData.Count;
                            if (len > 0) {
                                if ((client.state & 0b1) == 0) {
                                    client.state |= 0b1;
                                } else {
                                    client.state &= 0b10;
                                }
                                int count = len > client.sendBuffer.Length ? client.sendBuffer.Length : len;
                                client.sendData.CopyTo(0, client.sendBuffer, 0, count);
                                client.sendData.RemoveRange(0, count);
                                client.sendEventArgs[client.state & 1].SetBuffer(client.sendBuffer, 0, count);
                                if (!socket.SendAsync(client.sendEventArgs[client.state & 1])) {
                                    SendEventArgs_Completed(socket, client.sendEventArgs[client.state & 1]);
                                }
                            } else {
                                client.state &= 1;
                            }
                        } else {
                            throw new Exception("套接字发生错误！");
                        }
                    } catch (Exception) {
                        lock (socketHashtable) {
                            socketHashtable.Remove(client.id);
                        }
                        client.s.Close();
                        onLeaveEvent?.Invoke(client.id, client.userToken);
                    }
                }
            }
        }
        public void SendPackage(int id, int signal, byte[] data) {
            if (data != null) {
                SendPackage(id, signal, data, 0, data.Length);
            } else {
                SendPackage(id, signal, null, 0, 0);
            }
        }
        public void SendPackage(int id, int signal, byte[] data, int offset, int count) {
            object o;
            lock (socketHashtable) {
                o = socketHashtable[id];
            }
            if (o is ClientStruct client) {
                lock (client.sendData) {
                    try {
                        int len = count + 4;
                        if ((client.state & 0b10) == 0) {
                            client.state |= 0b10;
                            client.sendBuffer[0] = (byte)(len & 0xFF);
                            client.sendBuffer[1] = (byte)((len >> 8) & 0xFF);
                            client.sendBuffer[2] = (byte)((len >> 16) & 0xFF);
                            client.sendBuffer[3] = (byte)((len >> 24) & 0xFF);
                            client.sendBuffer[4] = (byte)(signal & 0xFF);
                            client.sendBuffer[5] = (byte)((signal >> 8) & 0xFF);
                            client.sendBuffer[6] = (byte)((signal >> 16) & 0xFF);
                            client.sendBuffer[7] = (byte)((signal >> 24) & 0xFF);
                            if (client.sendData.Capacity < count - client.sendBuffer.Length + 8) {
                                client.sendData.Capacity = count - client.sendBuffer.Length + 8;
                            }
                            for (int i = 0; i < count; i++) {
                                if (i + 8 < client.sendBuffer.Length) {
                                    client.sendBuffer[i + 8] = data[i + offset];
                                } else {
                                    client.sendData.Add(data[i + offset]);
                                }
                            }
                            client.sendEventArgs[client.state & 1].SetBuffer(client.sendBuffer, 0, count);
                            client.sendEventArgs[client.state & 1].UserToken = client;
                            if (!socket.SendAsync(client.sendEventArgs[client.state & 1])) {
                                SendEventArgs_Completed(socket, client.sendEventArgs[client.state & 1]);
                            }
                        } else {
                            if (client.sendData.Capacity < count - client.sendBuffer.Length + 8 + client.sendData.Count) {
                                client.sendData.Capacity = count - client.sendBuffer.Length + 8 + client.sendData.Count;
                            }
                            client.sendData.Add((byte)(len & 0xFF));
                            client.sendData.Add((byte)((len >> 8) & 0xFF));
                            client.sendData.Add((byte)((len >> 16) & 0xFF));
                            client.sendData.Add((byte)((len >> 24) & 0xFF));
                            client.sendData.Add((byte)(signal & 0xFF));
                            client.sendData.Add((byte)((signal >> 8) & 0xFF));
                            client.sendData.Add((byte)((signal >> 16) & 0xFF));
                            client.sendData.Add((byte)((signal >> 24) & 0xFF));
                            for (int i = 0; i < count; i++) {
                                client.sendData.Add(data[i + offset]);
                            }
                        }
                    } catch (Exception) {
                        lock (socketHashtable) {
                            socketHashtable.Remove(client.id);
                        }
                        client.s.Close();
                        onLeaveEvent?.Invoke(client.id, client.userToken);
                    }
                }
            }
        }
    }
}