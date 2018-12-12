using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp1
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culter)
        {
            DateTime date = (DateTime)value;
            return date.ToString("d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culter)
        {
            string strValue = value as string;
            DateTime dt;
            if (DateTime.TryParse(strValue, out dt))
            {
                return dt;
            }
            return DateTime.Now;
        }
    }
}
