using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace PlanningProgramV3.Converters
{
    /**
     * For right now, all this class does is convert the pixels from the left and top of the canvas to coordinates
     */
    public class PixelToCoordinate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (parameter != null)
            {
                return (int)((double)value * int.Parse((string)parameter));
            }
            else
                return (int)((double)value * (int)parameter);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return (double)value / (int)parameter;
        }
        public static object ConvertToCoordinate(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (parameter != null)
            {
                return (int)((double)value * (int)parameter);
            }
            else
                return (int)((double)value * 1000);
        }
        public object ConvertBackToPixel(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return ((int)value) / (int)parameter;
        }

        //makign these static so can access in scripts and see if works
    }
}
