using System;
using System.Collections.Generic;
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
using PlanningProgramV3.ViewModels.Calendar;

namespace PlanningProgramV3.Views.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarTaskView.xaml
    /// </summary>
    public partial class CalendarTaskView : UserControl
    {
        private MainWindow _mainWindow;

        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty);}
            set { SetValue(BackgroundColorProperty, value); }
        }

        //So because these won't change too frequently, it needds only be a dependency property?
        //Downside though: can't vary the color for each one I guess...
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(SolidColorBrush), typeof(CalendarTaskView));

        public SolidColorBrush DefaultBackgroundColor;

        public CalendarTaskView()
        {
            InitializeComponent();
        }

        public CalendarTaskView(SolidColorBrush color, MainWindow mainWindow) : this()
        {
            _mainWindow = mainWindow;
            DefaultBackgroundColor = BackgroundColor = color;
        }

        private void TaskTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                _mainWindow.EventCalendarTaskDoubleClicked(this);
            }
            else if(e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
            {
                _mainWindow.EventCalendarTaskClicked(this);
            }
        }
    }
}
