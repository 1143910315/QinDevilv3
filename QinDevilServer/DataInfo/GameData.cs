using CommonCode.CCviewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinDevilServer.DataInfo {
    public class GameData : CCViewModelBase {
        public readonly ReaderWriterLockSlim ClientInfoLock = new ReaderWriterLockSlim();
        private LinkedList<UserInfo> _clientInfo = new LinkedList<UserInfo>();
        public LinkedList<UserInfo> ClientInfo {
            get => _clientInfo;
            set => Set(ref _clientInfo, value);
        }
        private int _line;
        public int Line {
            get => _line;
            set => Set(ref _line, value);
        }
        public readonly ReaderWriterLockSlim LogLock = new ReaderWriterLockSlim();
        private LinkedList<LogDetail> _log = new LinkedList<LogDetail>();
        public LinkedList<LogDetail> Log {
            get => _log;
            set => Set(ref _log, value);
        }
        private Stack<LinkedListNode<LogDetail>> _logBack = new Stack<LinkedListNode<LogDetail>>();
        public Stack<LinkedListNode<LogDetail>> LogBack {
            get => _logBack;
            set => Set(ref _logBack, value);
        }
        private string _no1Qin = "";
        public string No1Qin {
            get => _no1Qin;
            set => Set(ref _no1Qin, value);
        }
        private string _no2Qin = "";
        public string No2Qin {
            get => _no2Qin;
            set => Set(ref _no2Qin, value);
        }
        private string _no3Qin = "";
        public string No3Qin {
            get => _no3Qin;
            set => Set(ref _no3Qin, value);
        }
        private string _no4Qin = "";
        public string No4Qin {
            get => _no4Qin;
            set => Set(ref _no4Qin, value);
        }
        private readonly List<int> _qinKey = new List<int>(new int[12]);
        public List<int> QinKey {
            get => _qinKey;
            set => Update();
        }
        private byte[] _hitQinKey = new byte[9];
        public byte[] HitQinKey {
            get => _hitQinKey;
            set => Set(ref _hitQinKey, value);
        }
        private int _hitQinKeyLength = 0;
        public int HitQinKeyLength {
            get => _hitQinKeyLength;
            set => Set(ref _hitQinKeyLength, value);
        }
    }
}
