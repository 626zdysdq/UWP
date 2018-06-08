using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AccountBook.Converters
{
    class ForMoney : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double money = (double)value;
            string str = money.ToString("0.00");
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
