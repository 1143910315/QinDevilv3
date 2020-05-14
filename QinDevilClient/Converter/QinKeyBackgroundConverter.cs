using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QinDevilClient.Converter {
    public class QinKeyBackgroundConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            List<int> list = values[0] as List<int>;
            int index = (int)values[1];
            List<int> licence = values[2] as List<int>;
            if (index > 0 && list.Count >= index) {
                if (licence.Contains(list[index - 1])) {
                    return new SolidColorBrush(Color.FromArgb(255, 45, 85, 55));
                } else if (list[index - 1] != 0) {
                    return new SolidColorBrush(Color.FromArgb(255, 230, 33, 41));
                }
            }
            return new SolidColorBrush(Colors.Silver);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}