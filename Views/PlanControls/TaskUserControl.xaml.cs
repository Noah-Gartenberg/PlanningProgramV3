using PlanningProgramV3.ViewModels;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanningProgramV3.Views.PlanControls
{
    /// <summary>
    /// Interaction logic for TaskUserControl.xaml
    /// </summary>
    public partial class TaskUserControl : UserControl
    {

        public TaskUserControl()
        {
            InitializeComponent();
        }

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    if(e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        DragDrop.DoDragDrop(this, this, DragDropEffects.Move);
        //    }
        //}

        //protected override void OnDrop(DragEventArgs e)
        //{
        //    base.OnDrop(e);

        //    //if data object contains string data, extract it
        //    if(e.Data.GetDataPresent(DataFormats.StringFormat))
        //    {
        //        string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

        //        // If the string can be converted into a Brush,
        //        // convert it and apply it to the ellipse.
        //        BrushConverter converter = new BrushConverter();
        //        if (converter.IsValid(dataString))
        //        {

        //            //Brush newFill = (Brush)converter.ConvertFromString(dataString);
        //            //circleUI.Fill = newFill;

        //            // Set Effects to notify the drag source what effect
        //            // the drag-and-drop operation had.
        //            // (Copy if CTRL is pressed; otherwise, move.)
        //            if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
        //            {
        //                e.Effects = DragDropEffects.Copy;
        //            }
        //            else
        //            {
        //                e.Effects = DragDropEffects.Move;
        //            }
        //        }
        //    }
        //    e.Handled = true;
        //}
    }
}
