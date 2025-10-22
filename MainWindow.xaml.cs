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


            //On initialization, check if there exists a programconfig file in the path where the program is installed.
            //if not, create one



            //afterwards/if so, check if the first-open boolean is false, and if there exists a file path where plans should be saved
            //if the former is not true, then display the program config window, and have them select a file path (default to documents folder, and create a "Planner" folder within it)
            //make it clear that a folder will be created there
            //then, set the value in the program config for the filepath to that new folder that was created

            #region Not current priority but how to use section
            //if the first-open boolean is false, display a how to use window
            /**
             * Sections of how to use:
             *      first section would be how to create a plan, and the various options you'd have for creating one
             *      second section would be the calendar, and how it works and how they communicate
             */

            //when it's completed or x-ed out, set the how to use boolean to true
            #endregion




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


        #region Can't put these into the view model, so for time being they're going in here.
        private void CommandBinding_OpenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Trying to execute load command");
            (DataContext as MainWindowViewModel).LoadPlan.Execute(null);
            

        }

        /// <summary>
        /// Don't have any reason why this would not be true right now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_CanExecute_OpenCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Trying to execute save command");
            (DataContext as MainWindowViewModel).SaveCurrentPlan.Execute(null);
            MessageBox.Show("Saving executed?");

        }

        private void CommandBinding_CanExecute_SaveCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (DataContext as MainWindowViewModel).SaveCurrentPlan.CanExecute(null);
        }
        #endregion
    }
}