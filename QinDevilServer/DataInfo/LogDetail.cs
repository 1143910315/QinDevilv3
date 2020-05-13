using CommonCode.CCviewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinDevilServer.DataInfo {
    public class LogDetail : CCViewModelBase {
        private string _content;
        public string Content {
            get => _content;
            set => Set(ref _content, value);
        }
        private int _time;
        public int Time {
            get => _time;
            set => Set(ref _time, value);
        }
    }
}
