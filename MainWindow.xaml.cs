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

        //private List<ICalendarTask> _tasks;
        //public List<ICalendarTask> Tasks
        //{
        //    get { return _tasks; }
        //    set
        //    {
        //        if (_tasks != value)
        //        {
        //            _tasks = value;
        //            OnPropertyChanged(nameof(Tasks));
        //
        //            WeeklyCalendarControl.DrawDays();
        //            MonthlyCalendarControl.DrawDays();
        //        }
        //    }
        //}

        

        public DateTime DaySelected { get; set; }
        public ICalendarDisplay CalendarInView;
        public MainWindow()
        {
            InitializeComponent();
            DaySelected = DateTime.Today;
            MonthlyCalendarControl.SetMainWindow(this);
            //WeeklyCalendarControl.SetMainWindow(this);
            //_tasks = new List<ICalendarTask>();
            DataAccess.InitializeDatabase();

            //hard setting this for sake of testing purposes - will change when can
            //MonthlyCalendarControl.Tasks = DataAccess.GetTasksFromMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
            //MonthlyCalendarControl.Tasks = DataAccess.GetAllTasks();
            //WeeklyCalendarControl.Tasks = DataAccess.GetTasksFromMonth(
            //    new DateTime(DateTime.Today.Year, DateTime.Today.Month, WeeklyCalendar.GetFirstDayOfWeekDate(DateTime.Today).Day),
            //    new DateTime(DateTime.Today.Year, DateTime.Today.Month, WeeklyCalendar.GetFirstDayOfWeekDate(DateTime.Today).Day).AddDays(7)
            //    );

            //why is the app not running (at least in debug mode)???? Previously, this was an issue with the sqlite connection. There are no errors as of right now. 
            //it loads when things are commented out, so lets try setting the tasks ienumerable to a new ienumerable with a few dummy objects???

            //temp.Add(new CalendarTaskData("GUID1", true, true, "TASK", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //temp.Add(new CalendarTaskData("GUID1", true, true, "TASK2", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //temp.Add(new CalendarTaskData("GUID1", true, true, "TASK3", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //
            //WeeklyCalendarControl.Tasks = temp;
            //SO THAT ENCOUNTERED THE SAME ISSUE!!!!!!! HOLY CRAP I must just be doing something wrong with this!
            //Let's go check as to how the person did their calendar events, with the IEnumerable interface
            //okay, so let's set this up in xaml maybe? Maybe I just have to bind them?

            //Okay test two, data binding ought to be set up
            //Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK2", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK3", true, DateTime.Today, DateTime.Today.AddDays(1)));
            //
            //MonthlyCalendarControl.DrawDays();
            //WeeklyCalendarControl.DrawDays();
            //AND THE TASKS STILL DON'T DISPLAY... BUT THEY'RE IN THE LISTS!!!!!!
            //Okay, so the tasks seem to be going out of scope, or not binding properly???

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