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

namespace PlanningProgramV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(FrameworkElement), typeof(ListView));

        public FrameworkElement SelectedObject
        {
            get => (FrameworkElement)GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        public PlannerViewModel CurrPlanner { get; set; }

        public DateTime DaySelected { get; set; }
        public ICalendarDisplay CalendarInView;
        public MainWindow()
        {
            InitializeComponent();
            DaySelected = DateTime.Today;
            CurrPlanner = new();
            //MonthCalendarControl.SetMainWindow(this);
            //WeeklyCalendarControl.SetMainWindow(this);
            CurrPlanner.HighestTasks.Add(new TaskViewModel());
            PlannerDisplayer.ItemsSource = CurrPlanner.HighestTasks;
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
        
        public void EventCalendarTaskClicked(CalendarTaskView taskToSelect)
        {
            CalendarInView.HighlightTask(taskToSelect);
        }

        private void PlannerDisplayer_MouseMove(object sender, MouseEventArgs e)
        {
            //For right now, will just not do selection logic if the selected item is not the highest level parent -- need to figure out how to ensure that selected view model is at top of its stack though
            if ((PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && viewModel.Parent == null && e.LeftButton == MouseButtonState.Pressed)
            {
                
                DataObject data = new DataObject(DataFormats.Serializable, PlannerDisplayer.SelectedItem);

                DragDrop.DoDragDrop(SelectedObject, data, DragDropEffects.Move);
                if(SelectedObject != null)
                {
                    SelectedObject.IsHitTestVisible = false;
                }
                
            }
        }
        //protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        //{
        //    base.OnGiveFeedback(e);
        //    //these effects values are set in the drop target's drago over event handler
        //    if(e.Effects.HasFlag(DragDropEffects.Move))
        //    {
        //        Mouse.SetCursor(Cursors.Hand);
        //    }
        //    else
        //    {
        //        Mouse.SetCursor(Cursors.No);
        //    }
        //        e.Handled = true;
        //}

        private void PlannerDisplayer_DragOver(object sender, DragEventArgs e)
        {
            //For right now, will just not do selection logic if the selected item is not the highest level parent
            if((PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && viewModel.Parent == null)
            {
                Point dropPosition = e.GetPosition(PlannerDisplayer);

                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);

            }

        }

        private void PlannerDisplayer_Drop(object sender, DragEventArgs e)
        {


            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {

                Point dropPosition = e.GetPosition(PlannerDisplayer);
                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);

                CurrPlanner.AddHighestTask(element);
                
            }
            SelectedObject.IsHitTestVisible = true;
            PlannerDisplayer.SelectedIndex = -1;

        }

        private void PlannerDisplayer_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == PlannerDisplayer)
            {
                object data = e.Data.GetData(DataFormats.Serializable);



                if (data is UIElement element)
                {
                    CurrPlanner.RemoveTopTask(data);
                }
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