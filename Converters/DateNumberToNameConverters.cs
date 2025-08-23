using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlanningProgramV3.Converters
{ 
    /**
     * Noah Gartenberg
     * This file contains the files necessary to take the current/selected date (day/month) and convert it to a name
     */
    public class MonthToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intVal)
            {
                DateTimeFormatInfo MonthFormatInfo = CultureInfo.InstalledUICulture.DateTimeFormat;
                return MonthFormatInfo.GetMonthName(intVal).ToString();
            }
            throw new ArgumentException("a value that was not an integer was passed in to the value parameter");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
