using CommonCode.CCviewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinDevilClient.DataInfo {
    public class GameData : CCViewModelBase {
        private bool _sendInfoSuccess;
        public bool SendInfoSuccess {
            get => _sendInfoSuccess;
            set => Set(ref _sendInfoSuccess, value);
        }
        private List<int> _licence = new List<int>();
        public List<int> Licence {
            get => _licence;
            set => Set(ref _licence, value);
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
        private readonly List<int> _qinKey = new List<int>(12);
        public List<int> QinKey {
            get => _qinKey;
            set => Update();
        }
        private readonly byte[] _hitQinKey = new byte[9];
        public byte[] HitQinKey {
            get => _hitQinKey;
            set => Update();
        }
        private int _hitKeyIndex = 0;
        public int HitKeyIndex {
            get => _hitKeyIndex;
            set => Set(ref _hitKeyIndex, value);
        }
        private int _ping = 9999;
        public int Ping {
            get => _ping;
            set => Set(ref _ping, value);
        }
    }
}