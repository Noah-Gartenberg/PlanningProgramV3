using Microsoft.UI.Input;
using PlanningProgramV3.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PlanningProgramV3.Converters
{
    /**
     * For right now, all this class does is convert the pixels from the left and top of the canvas to coordinates
     */
    //https://stackoverflow.com/questions/62714559/wpf-canvas-zoom-and-children-position/62715838#62715838 zoom behavior/scroll behavior
    //https://stackoverflow.com/questions/64894795/how-to-map-world-coordinates-to-screen-coordinates-while-separating-view-model

    //Drag and drop: https://github.com/microsoft/WPF-Samples/blob/main/Drag%20and%20Drop/DragDropObjects/MainWindow.cs
    [ValueConversion(typeof(double), typeof(double))]
    public class PixelToCoordinate : IMultiValueConverter
    {
        //objects passed in include:
            //coordinate value
            //camera location
            //canvas width/height
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //coordinate - camera location == screen
            //world coord --> canvas == (worldCoord - cameraCoord) + 0.5 * canvas width/height
                //don't need scale factor because loading data/setting data. 
            return (double)values[0] - (double)values[1] + (0.5f * (double)values[2]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
