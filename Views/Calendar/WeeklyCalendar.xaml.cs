using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for WeeklyCalendar.xaml
    /// </summary>
    public partial class WeeklyCalendar : UserControl, INotifyPropertyChanged, ICalendarDisplay
    {
        public MainWindow _mainWindow;
        public IEnumerable<object> Tasks
        {
            get { return (IEnumerable<object>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register("Tasks", typeof(IEnumerable<object>), typeof(WeeklyCalendar),
                new PropertyMetadata(OnTasksChangedCallBack));


        public SolidColorBrush GridBrush
        {
            get { return (SolidColorBrush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(SolidColorBrush), typeof(WeeklyCalendar), new PropertyMetadata(Brushes.Black));

        public SolidColorBrush Color0
        {
            get { return (SolidColorBrush)GetValue(Color0Property); }
            set { SetValue(Color0Property, value); }
        }

        public static readonly DependencyProperty Color0Property =
            DependencyProperty.Register("Color0", typeof(SolidColorBrush), typeof(WeeklyCalendar),
                new PropertyMetadata(Brushes.LightCyan));


        public SolidColorBrush Color1
        {
            get { return (SolidColorBrush)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(SolidColorBrush), typeof(WeeklyCalendar),
                new PropertyMetadata(Brushes.PaleTurquoise));

        public SolidColorBrush Color2
        {
            get { return (SolidColorBrush)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(SolidColorBrush), typeof(WeeklyCalendar),
                new PropertyMetadata(Brushes.SkyBlue));

        public SolidColorBrush HighlightColor
        {
            get { return (SolidColorBrush)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.Register("HighlightColor", typeof(SolidColorBrush), typeof(WeeklyCalendar),
                new PropertyMetadata(Brushes.DodgerBlue));


        public double GridBorderThickness
        {
            get { return (double)GetValue(GridBorderThicknessProperty); }
            set { SetValue(GridBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridBorderThicknessProperty =
            DependencyProperty.Register("GridBorderThickness", typeof(double), typeof(WeeklyCalendar),
                new PropertyMetadata(0.5));

        private static void OnTasksChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WeeklyCalendar calendar = sender as WeeklyCalendar;
            if (calendar != null)
            {
                calendar.DrawDays();
            }

        }

        //This bool ensures (in theory) that the combo boxes won't set themselves off when the value for current date changes, just becuase they changed programattically
            //previously, going forwards any days using the buttons would (on certain months - I'm guessing due to a combo of day + month combo boxes both changing??? idek) 
            //cause the selectors to change values, causing the corresponding events for them having been fired to fire, so this just tells them not to fire if i've pressed the buttons
        bool fireComboBoxChangeEvent = true;

        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get
            {
                //return _mainWindow.DaySelected; 
                return currentDate;
            }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    OnPropertyChanged(() => currentDate);
                    DrawDaysOnStart();
                    SetDateSelectionComboBoxesByCurrentDate();
                }
            }
        }

        private void SetDateSelectionComboBoxesByCurrentDate()
        {
            MonthsComboBox.SelectedValue = CurrentDate.Month;
            YearsComboBox.SelectedValue = CurrentDate.Year;
            fireComboBoxChangeEvent = true;
            //SetDateSelectionComboBoxesByCurrentDate(); //Want to only fire this method after they have all been changed, to see if this fixes the issue where the setting the combo boxes while setting everything else kind of 
                                                       //breaks everything
            System.Diagnostics.Debug.WriteLine("Set Date Selection Combo Boxes by current date" + CurrentDate.ToString("dd, MM, yyyy"));

        }

        private void SetCurrentDateByDateSelectionComboBoxes()
        {
            //should only fire if FireComboBoxChangeEvent is true
            if (fireComboBoxChangeEvent && YearsComboBox?.SelectedValue != null && MonthsComboBox?.SelectedValue != null)
            {
                CurrentDate = new DateTime((int)YearsComboBox.SelectedValue, (int)MonthsComboBox.SelectedValue, 1);
                
            }

            System.Diagnostics.Debug.WriteLine("Set Current Date by Date Selection Combo Boxes " + CurrentDate.ToString("dd, MM, yyyy"));
        }

        public ObservableCollection<CalendarCell> DaysInCurrentWeek { get; set; }

        public WeeklyCalendar()
        {
            InitializeComponent(); 
            DaysInCurrentWeek = new ObservableCollection<CalendarCell>();
            InitializeDayLabels();
            InitializeDateSelectionComboBoxes();
        }

        public void SetMainWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        private void InitializeDayLabels()
        {
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label();
                dayLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                dayLabel.SetValue(Grid.ColumnProperty, i);
                //Days.Sunday == 0, so i+1 will make monday as first day
                dayLabel.Content = CultureInfo.InstalledUICulture.DateTimeFormat.DayNames[(i + 1) % 7];
                dayLabel.FontWeight = FontWeights.Bold;
                DayLabelsGrid.Children.Add(dayLabel);
            }
        }

        private void InitializeDateSelectionComboBoxes()
        {
            //why i <= 12??? -- OH MONTHS
            for (int i = 1; i <= 12; i++)
            {
                MonthsComboBox.Items.Add(i);
            }

            for (int i = 1950; i <= 2100; i++)
            {
                YearsComboBox.Items.Add(i);
            }
            CurrentDate = DateTime.Today;
        }

        public void HighlightTask(CalendarTaskView taskToSelect)
        {
            //May need to refactor to decouple
            foreach (CalendarCell day in DaysInCurrentWeek)
            {
                foreach (CalendarTaskView e in day.Tasks.Children)
                {
                    if (e.DataContext == taskToSelect.DataContext)
                    {
                        e.BackgroundColor = HighlightColor;
                    }
                    else
                    {
                        e.BackgroundColor = e.DefaultBackgroundColor;
                    }
                }
            }
        }

        /**
         * Draws all days onto screen at the start of the program running 
         */
        public void DrawDaysOnStart()
        {
            DaysGrid.Children.Clear();
            DaysInCurrentWeek.Clear();
            //Since there are only 7 days in a week... I don't need to clear the grid, just need to clear their children's grids.
            DayOfWeek day = CurrentDate.DayOfWeek;
            int daysFromStartOfWeek = 0;
            //figure out how many days from start of the week the current date is. 
            switch (day)
            {
                case DayOfWeek.Monday:
                    daysFromStartOfWeek = 0;
                    break;
                case DayOfWeek.Tuesday:
                    daysFromStartOfWeek = 1;
                    break;
                case DayOfWeek.Wednesday:
                    daysFromStartOfWeek = 2;
                    break;
                case DayOfWeek.Thursday:
                    daysFromStartOfWeek = 3;
                    break;
                case DayOfWeek.Friday:
                    daysFromStartOfWeek = 4;
                    break;
                case DayOfWeek.Saturday:
                    daysFromStartOfWeek = 5;
                    break;
                case DayOfWeek.Sunday:
                    daysFromStartOfWeek = 6;
                    break;
            }
            DateTime firstOfWeek = CurrentDate.AddDays(-1 * daysFromStartOfWeek); 
            DateTime lastOfWeek = firstOfWeek.AddDays(6);

            for (DateTime date = firstOfWeek; date.Date <= lastOfWeek; date = date.AddDays(1))
            {
                CalendarCell newDay = new CalendarCell();
                newDay.BorderThickness = new Thickness((double)GridBorderThickness / 2);
                newDay.BorderBrush = GridBrush;
                newDay.CellDate = date;
                DaysInCurrentWeek.Add(newDay);
            }
            
            for(int i = 0; i < 7; i++ )
            {
                Grid.SetRow(DaysInCurrentWeek[i], 1);
                Grid.SetColumn(DaysInCurrentWeek[i], i);
                DaysGrid.Children.Add(DaysInCurrentWeek[i]);
            }

            //DrawTopBorder();
            //DrawBottomBorder();
            //DrawRightBorder();
            //DrawLeftBorder();

            DrawBorders();

            //set some background today
            CalendarCell today = DaysInCurrentWeek.Where(d => d.CellDate == DateTime.Today).FirstOrDefault();
            if (today != null)
            {
                today.CellDateTextBlock.Background = Color0;
            }

            DrawTasks();
        }

        public void DrawDays()
        {
            //Since there are only 7 days in a week... I don't need to clear the grid, just need to clear their children's grids.
            DayOfWeek day = CurrentDate.DayOfWeek;
            int daysFromStartOfWeek = 0;
            //figure out how many days from start of the week the current date is. 
            switch (day)
            {
                case DayOfWeek.Monday:
                    daysFromStartOfWeek = 0;
                    break;
                case DayOfWeek.Tuesday:
                    daysFromStartOfWeek = 1;
                    break;
                case DayOfWeek.Wednesday:
                    daysFromStartOfWeek = 2;
                    break;
                case DayOfWeek.Thursday:
                    daysFromStartOfWeek = 3;
                    break;
                case DayOfWeek.Friday:
                    daysFromStartOfWeek = 4;
                    break;
                case DayOfWeek.Saturday:
                    daysFromStartOfWeek = 5;
                    break;
                case DayOfWeek.Sunday:
                    daysFromStartOfWeek = 6;
                    break;
            }

            DateTime firstOfWeek = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.AddDays(-1 * daysFromStartOfWeek).Day);
            for(int i =0; i < daysFromStartOfWeek; i++)
            {
                CalendarCell currDay = DaysInCurrentWeek[i];
                currDay.CellDate = firstOfWeek.AddDays(i);
            }

            //need not redraw borders or background

            DrawTasks();
        }
        
        //Since there is only one row, there should only need to be one method for drawing the borders
        private void DrawBorders()
        {
            for(int i = 0; i < 7; i++)
            {
                //first draw top and bottom borders since these are universal
                DaysInCurrentWeek[i].BorderThickness = new Thickness(DaysInCurrentWeek[i].BorderThickness.Left, GridBorderThickness, DaysInCurrentWeek[i].BorderThickness.Right, GridBorderThickness);

                //do left & right borders for both sides
                if (i == 0)
                {
                    DaysInCurrentWeek[i].BorderThickness = new Thickness(GridBorderThickness, 
                        DaysInCurrentWeek[i].BorderThickness.Top, DaysInCurrentWeek[i].BorderThickness.Right, DaysInCurrentWeek[i].BorderThickness.Bottom);
                }
                else if(i == 6)
                {
                    DaysInCurrentWeek[i].BorderThickness = new Thickness(DaysInCurrentWeek[i].BorderThickness.Left,
                        DaysInCurrentWeek[i].BorderThickness.Top, GridBorderThickness, DaysInCurrentWeek[i].BorderThickness.Bottom);
                }
            }
        }

        private void DrawTasks()
        {
            //This method can be called when tasks aren't yet bound, so check that case and return
            if (Tasks == null)
            {
                return;
            }

            //when Tasks is bound, check if it is collection of ICalendarTasks
            if (Tasks is IEnumerable<ICalendarTask> tasks)
            {
                //add colors of tasks to array to pick up them using number
                SolidColorBrush[] colors = { Color0, Color1, Color2 };

                //index to array of colors
                int accentColor = 0;

                //loop all tasks -- ensure all have date starts, and assume if no date end, then just have only one day
                foreach (var t in tasks.OrderBy(t => t.DateStart))
                {
                    if (!t.DateStart.HasValue /*|| !t.DateEnd.HasValue*/)
                    {
                        continue;
                    }

                    //number of row in day, in which event should be displayed
                    int taskRow = 0;
                    var dateFrom = (DateTime)t.DateStart;
                    var dateTo = (DateTime)t.DateEnd;

                    //loop all days of current task
                    for (DateTime date = dateFrom; date <= dateTo; date.AddDays(1))
                    {
                        //get DayOfWeek for current day of currrent task
                        CalendarCell day = DaysInCurrentWeek.Where(d => d.CellDate.Date == date.Date).FirstOrDefault();

                        //day is another month so skip it
                        if (day == null)
                        { continue; }

                        //if day of week is monday, task should be displayed on first row
                        if (day.CellDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            taskRow = 0;
                        }

                        //but if there are some events before, event won't be in first row, but after previous events
                        if (day.Tasks.Children.Count > taskRow)
                        {
                            taskRow = Grid.GetRow(day.Tasks.Children[day.Tasks.Children.Count - 1]) + 1;
                        }

                        //get color for task

                        int accentColorIndex = accentColor % colors.Count();
                        CalendarTaskView calendarTaskView = new CalendarTaskView(colors[accentColorIndex], _mainWindow);

                        calendarTaskView.DataContext = t;
                        Grid.SetRow(calendarTaskView, taskRow);
                        day.Tasks.Children.Add(calendarTaskView);
                    }
                    accentColor++;

                }
            }
            else
            {
                throw new ArgumentException("Tasks must be IEnumerable<ICalendarTask>");
            }
        }

        private void PreviousMonthButtonClicked(object sender, RoutedEventArgs e)
        {
            fireComboBoxChangeEvent = false;
            CurrentDate = CurrentDate.AddDays(-7);
            //WeeksComboBox.SelectedIndex = WeeklyCalendar.GetWeekNumber(CurrentDate) - 1;
        }

        private void NextWeekButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Next week button " + CurrentDate.AddDays(7).ToString("dd, MM, yyyy"));
            fireComboBoxChangeEvent = false;
            CurrentDate = CurrentDate.AddDays(7);
            //WeeksComboBox.SelectedIndex = WeeklyCalendar.GetWeekNumber(CurrentDate) - 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //WHAT IS LINQ??????????????
        public void OnPropertyChanged<T>(Expression<Func<T>> exp)
        {
            //The cast will always succeed
            var memberExpression = (MemberExpression)exp.Body;
            var propertyName = memberExpression.Member.Name;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentDateByDateSelectionComboBoxes();
        }

        private void YearsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentDateByDateSelectionComboBoxes();
        }

        public static int GetWeekNumber(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }
    }
}
