using System.Text;
using System.Windows;
using PlanningProgramV3.Views.PlanControls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanningProgramV3.ViewModels;
using PlanningProgramV3.Views;
using PlanningProgramV3.Views.Calendar;
using PlanningProgramV3.ViewModels.Calendar;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System.ComponentModel;

namespace PlanningProgramV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {


        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(FrameworkElement), typeof(ListView));

        public FrameworkElement SelectedObject
        {
            get => (FrameworkElement)GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        

        public ICalendarDisplay CalendarInView;
        public MainWindow()
        {
            InitializeComponent();
            MonthlyCalendarControl.SetMainWindow(this);
            WeeklyCalendarControl.SetMainWindow(this);
            

        }

        public event EventHandler<CalendarTaskView> EventCalendarTaskDoubleClickedEvent;


        /**
         * this event (in my head) should open up the plan that contains the actual task
         */
        public void EventCalendarTaskDoubleClicked(CalendarTaskView calendarTaskView)
        {
            throw new NotImplementedException("Ideally, this method should open the actual plan");
            EventCalendarTaskDoubleClickedEvent?.Invoke(this, calendarTaskView);
        }

        public event EventHandler<CalendarTaskView> EventCalendarTaskClickedEvent;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void EventCalendarTaskClicked(CalendarTaskView taskToSelect)
        {
            CalendarInView.HighlightTask(taskToSelect);
        }

       
    }
}