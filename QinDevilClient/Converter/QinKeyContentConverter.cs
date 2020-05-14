using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace QinDevilClient.Converter {
    public class QinKeyContentConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string str = (string)values[0];
            int index = (int)values[1];
            if (str.Length < index) {
                return "";
            }
            return (str[index - 1]) switch
            {
                '1' => "宫",
                '2' => "商",
                '3' => "角",
                '4' => "徵",
                '5' => "羽",
                _ => "",
            };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}