using PlanningProgramV3.Converters;
using PlanningProgramV3.ViewModels;
using PlanningProgramV3.ViewModels.Calendar;
using PlanningProgramV3.ViewModels.ItemViewModels;
using PlanningProgramV3.Views.Calendar;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlanningProgramV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    ///     This is specifically for the "main window" part of the main window, rather than the plan displayer.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {   
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

    /// <summary>
    /// Interaction logic/drag and drop for plan displayer in main window
    ///     tbh, I dont' know why I put the plan displayer in a user control in the first place. This is going to be a bit easier
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
        private void AddTaskItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainWindowViewModel).AddTaskToCurrentPlan.Execute(null);
        }

        private void PlannerDisplayer_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //how ensure that the sender is the list view? 
                if (sender is ListView && (PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && e.LeftButton == MouseButtonState.Pressed)
                {

                    DataObject data = new DataObject(DataFormats.Serializable, PlannerDisplayer.SelectedItem);

                    DragDrop.DoDragDrop(SelectedObject, data, DragDropEffects.Move);
                    if (SelectedObject != null)
                    {
                        SelectedObject.IsHitTestVisible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PlannerDisplayer_DragOver(object sender, DragEventArgs e)
        {
            //For right now, will just not do selection logic if the selected item is not the highest level parent
            if ((PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && viewModel.Parent == null)
            {
                Point dropPosition = e.GetPosition(PlannerDisplayer);

                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);

            }
        }

        private void PlannerDisplayer_Drop(object sender, DragEventArgs e)
        {
            try
            {
                object data = e.Data.GetData(DataFormats.Serializable);

                Point dropPosition = e.GetPosition(PlannerDisplayer);
                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);
                //CurrentPlanner.SetTopTaskPosition(SelectedObject);


                //set the position of the object
                //I know this isn't technically MVVM - I mean I don't think it's a violation per se, but yeah. 
                //couldn't figure out how to do this from the planner
                if (SelectedObject.DataContext is TaskViewModel task)
                {
                    task.X = Convert.ToDouble(PixelToCoordinate.ConvertToCoordinate(dropPosition.X, typeof(int), 1500, null));
                    task.Y = Convert.ToDouble(PixelToCoordinate.ConvertToCoordinate(dropPosition.Y, typeof(int), 1500, null));
                }
                SelectedObject.IsHitTestVisible = true;
                PlannerDisplayer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void PlannerDisplayer_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == PlannerDisplayer)
            {
                object data = e.Data.GetData(DataFormats.Serializable);


                //commented out for right now
                //if (data is UIElement element)
                //{
                //    CurrentPlanner.DeleteItem(data);
                //}
            }
        }

        private void PlannerDisplayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Shout out people in comments explaining how I can fix this so that selected object doesn't throw an error
            //https://stackoverflow.com/questions/610343/wpf-listbox-getting-uielement-instead-of-of-selecteditem
            //in theory at least if I can get it working

            //for right now, only worrying about one item
            if (SelectedObject != null)
            {
                SelectedObject.IsHitTestVisible = true;
            }
            if (PlannerDisplayer.SelectedIndex != -1)
            {

                SelectedObject = PlannerDisplayer.ItemContainerGenerator.ContainerFromItem(PlannerDisplayer.SelectedItem) as FrameworkElement;
            }
            else
                SelectedObject = null;
        }
    }
}