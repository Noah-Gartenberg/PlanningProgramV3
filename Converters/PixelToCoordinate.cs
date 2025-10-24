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
    //https://stackoverflow.com/questions/62714559/wpf-canvas-zoom-and-children-position/62715838#62715838 zoom behavior/scroll behavior
    //https://stackoverflow.com/questions/64894795/how-to-map-world-coordinates-to-screen-coordinates-while-separating-view-model
    public class PixelToCoordinate : IMultiValueConverter, IValueConverter
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
            if (parameter != null)
            {
                return (int)((double)value * (int)parameter);
            }
            else
                return (int)((double)value / (float)(int)parameter);
        }
        public static object ConvertToCoordinate(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (parameter != null)
            {
                return (int)((double)value * (int)parameter);
            }
            else
                return (int)((double)value /(float)(int)parameter);
        }
        public object ConvertBackToPixel(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            return ((int)value) / (int)parameter;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Assuming 'values' contains the x/y 

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //makign these static so can access in scripts and see if works
    }
}
