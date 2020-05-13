using CommonCode.CCviewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinDevilServer.DataInfo {
    public class UserInfo : CCViewModelBase {
        private int _id;
        public int Id {
            get => _id;
            set => Set(ref _id, value);
        }
        private DateTime _lastReceiveTime;
        public DateTime LastReceiveTime {
            get => _lastReceiveTime;
            set => Set(ref _lastReceiveTime, value);
        }
        private int _line;
        public int Line {
            get => _line;
            set => Set(ref _line, value);
        }
        private string _machineIdentity = "";
        public string MachineIdentity {
            get => _machineIdentity;
            set => Set(ref _machineIdentity, value);
        }
        private string _gamePath = "";
        public string GamePath {
            get => _gamePath;
            set => Set(ref _gamePath, value);
        }
    }
}