using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for MonthCalendar.xaml
    /// </summary>
    public partial class MonthCalendar : UserControl, INotifyPropertyChanged, ICalendarDisplay
    {
        public MainWindow _mainWindow;

        public IEnumerable<object> Tasks
        {
            get { return (IEnumerable<object>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register("Tasks", typeof(IEnumerable<object>), typeof(MonthCalendar),
                new PropertyMetadata(OnTasksChangedCallBack));


        public SolidColorBrush GridBrush
        {
            get { return (SolidColorBrush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(SolidColorBrush), typeof(MonthCalendar), new PropertyMetadata(Brushes.Black));

        public SolidColorBrush Color0
        {
            get { return (SolidColorBrush)GetValue(Color0Property); }
            set { SetValue(Color0Property, value); }
        }

        public static readonly DependencyProperty Color0Property =
            DependencyProperty.Register("Color0", typeof(SolidColorBrush), typeof(MonthCalendar),
                new PropertyMetadata(Brushes.LightCyan));


        public SolidColorBrush Color1
        {
            get { return (SolidColorBrush)GetValue(Color1Property); }
            set { SetValue(Color1Property, value); }
        }

        public static readonly DependencyProperty Color1Property =
            DependencyProperty.Register("Color1", typeof(SolidColorBrush), typeof(MonthCalendar),
                new PropertyMetadata(Brushes.PaleTurquoise));

        public SolidColorBrush Color2
        {
            get { return (SolidColorBrush)GetValue(Color2Property); }
            set { SetValue(Color2Property, value); }
        }

        public static readonly DependencyProperty Color2Property =
            DependencyProperty.Register("Color2", typeof(SolidColorBrush), typeof(MonthCalendar),
                new PropertyMetadata(Brushes.SkyBlue));

        public SolidColorBrush HighlightColor
        {
            get { return (SolidColorBrush)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        public static readonly DependencyProperty HighlightColorProperty =
            DependencyProperty.Register("HighlightColor", typeof(SolidColorBrush), typeof(MonthCalendar),
                new PropertyMetadata(Brushes.DodgerBlue));


        public double GridBorderThickness
        {
            get { return (double)GetValue(GridBorderThicknessProperty); }
            set { SetValue(GridBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridBorderThicknessProperty =
            DependencyProperty.Register("GridBorderThickness", typeof(double), typeof(MonthCalendar),
                new PropertyMetadata(0.5));

        private static void OnTasksChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MonthCalendar calendar = sender as MonthCalendar;
            calendar?.DrawDays();

        }
        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get 
            {
                //return _mainWindow.DateSelected; 
                return currentDate;
            }
            set
            {
                if(CurrentDate != value)
                {
                    currentDate = value;
                    OnPropertyChanged(() => currentDate);
                    DrawDays();
                    SetDateSelectionComboBoxesByCurrentDate();
                }
            }
        }

        private void SetDateSelectionComboBoxesByCurrentDate()
        {
            MonthsComboBox.SelectedValue = CurrentDate.Month;
            YearsComboBox.SelectedValue = CurrentDate.Year;
        }

        private void SetCurrentDateByDateSelectionComboBoxes()
        {
            if (YearsComboBox?.SelectedValue != null && MonthsComboBox?.SelectedValue != null)
            {
                CurrentDate = new DateTime((int)YearsComboBox.SelectedValue, (int)MonthsComboBox.SelectedValue, 1);
            }
        }

        public ObservableCollection<CalendarCell> DaysInCurrentMonth { get; set; }

        public MonthCalendar()
        {
            InitializeComponent();
            DaysInCurrentMonth = [];
            InitializeDayLabels();
            InitializeDateSelectionComboBoxes();
        }

        public void SetMainWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        private void InitializeDayLabels()
        {
            for(int i = 0; i < 7; i++)
            {
                Label dayLabel = new();
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
            for(int i = 1; i <= 12; i++)
            {
                MonthsComboBox.Items.Add(i);
            }

            for(int i = 1950; i <= 2100; i++)
            {
                YearsComboBox.Items.Add(i);
            }
            CurrentDate = DateTime.Now;
        }

        public void HighlightTask(CalendarTaskView eventToSelect)
        {
            //May need to refactor to decouple
            foreach (CalendarCell day in DaysInCurrentMonth)
            {
                foreach (CalendarTaskView e in day.CellTasks.Children)
                {
                    if (e.DataContext == eventToSelect.DataContext)
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

        public void DrawDays()
        {
            DaysGrid.Children.Clear();
            DaysInCurrentMonth.Clear();

            DateTime firstDayOfMonth = new(CurrentDate.Year, CurrentDate.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            for (DateTime date = firstDayOfMonth; date.Date <= lastDayOfMonth; date = date.AddDays(1))
            {
                CalendarCell newDay = new();
                newDay.BorderThickness = new Thickness((double)GridBorderThickness / 2);
                newDay.BorderBrush = GridBrush;
                newDay.CellDate = date;
                DaysInCurrentMonth.Add(newDay);
            }

            int row = 0;
            int column = 0;

            for (int i = 0; i < DaysInCurrentMonth.Count; i++)
            {
                switch (DaysInCurrentMonth[i].CellDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        column = 0;
                        break;
                    case DayOfWeek.Tuesday:
                        column = 1;
                        break;
                    case DayOfWeek.Wednesday:
                        column = 2;
                        break;
                    case DayOfWeek.Thursday:
                        column = 3;
                        break;
                    case DayOfWeek.Friday:
                        column = 4;
                        break;
                    case DayOfWeek.Saturday:
                        column = 5;
                        break;
                    case DayOfWeek.Sunday:
                        column = 6;
                        break;
                }
                Grid.SetRow(DaysInCurrentMonth[i], row);
                Grid.SetColumn(DaysInCurrentMonth[i], column);
                DaysGrid.Children.Add(DaysInCurrentMonth[i]);

                if (column == 6)
                {
                    row++;
                }

                DrawTopBorder();
                DrawBottomBorder();
                DrawRightBorder();
                DrawLeftBorder();

                //set some background today
                CalendarCell today = DaysInCurrentMonth.Where(d => d.CellDate == DateTime.Today).FirstOrDefault();
                if (today != null)
                {
                    today.CellDateTextBlock.Background = Color0;
                }
                

            }
            if (Tasks != null && Tasks.Count() > 0)
                DrawTasks();
        }

        

        private void DrawTopBorder()
        {
            //draw top borderline to be the same as inner lines in calendar
            for(int i = 0; i < 7; i++)
            {
                DaysInCurrentMonth[i].BorderThickness = new Thickness(DaysInCurrentMonth[i].BorderThickness.Left,
                    GridBorderThickness, DaysInCurrentMonth[i].BorderThickness.Right, DaysInCurrentMonth[i].BorderThickness.Bottom);
            }
        }

        private void DrawBottomBorder()
        {
            // draw bottom border line to be the same as inner lines in calendar
            int daysInCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            for (int i = daysInCurrentMonth - 1; i >= daysInCurrentMonth - 7; i--)
            {
                DaysInCurrentMonth[i].BorderThickness = new Thickness(DaysInCurrentMonth[i].BorderThickness.Left, 
                    DaysInCurrentMonth[i].BorderThickness.Top, DaysInCurrentMonth[i].BorderThickness.Right, GridBorderThickness);
            }
        }

        private void DrawRightBorder()
        {
            // draw right border line to be the same as inner lines in calendar
            IEnumerable<CalendarCell> sundays = DaysInCurrentMonth.Where(d => d.CellDate.DayOfWeek == DayOfWeek.Sunday);
            foreach (var sunday in sundays)
            {
                sunday.BorderThickness = new Thickness(sunday.BorderThickness.Left, sunday.BorderThickness.Top, GridBorderThickness, sunday.BorderThickness.Bottom);
            }
            // right border for last day in month
            var lastDay = DaysInCurrentMonth.Last();
            lastDay.BorderThickness = new Thickness(lastDay.BorderThickness.Left, lastDay.BorderThickness.Top, GridBorderThickness, lastDay.BorderThickness.Bottom);

        }

        private void DrawLeftBorder()
        {
            // draw left border line to be the same as inner lines in calendar
            IEnumerable<CalendarCell> mondays = DaysInCurrentMonth.Where(d => d.CellDate.DayOfWeek == DayOfWeek.Monday);
            foreach (var monday in mondays)
            {
                monday.BorderThickness = new Thickness(GridBorderThickness, monday.BorderThickness.Top, monday.BorderThickness.Right, monday.BorderThickness.Bottom);
            }
            // left border for first day in month
            var firstDay = DaysInCurrentMonth.First();
            firstDay.BorderThickness = new Thickness(GridBorderThickness, firstDay.BorderThickness.Top, firstDay.BorderThickness.Right, firstDay.BorderThickness.Bottom);
        }

        private void DrawTasks()
        {
            //this method's getting called a lot
            //This method can be called when tasks aren't yet bound, so check that case and return
            if(Tasks == null || Tasks.Count() == 0)
            {
                return;
            }

            //when Tasks is bound, check if it is collection of ICalendarTasks
            if(Tasks is IEnumerable<ICalendarTask> tasks)
            {
                //add colors of tasks to array to pick up them using number
                SolidColorBrush[] colors = [Color0, Color1, Color2];

                //index to array of colors
                int accentColor = 0;
                
                foreach (var t in tasks.OrderBy(t => t.DateStart))
                {
                    if(!t.DateStart.HasValue || !t.DateEnd.HasValue)
                    {
                        continue;
                    }

                    //number of row in day, in which event should be displayed
                    int taskRow = 0;
                    var dateFrom = (DateTime)t.DateStart;
                    var dateTo = (DateTime)t.DateEnd;

                    //loop all days of current task
                    //youre kidding me... I literally forgot to set the value for date... it was just date.adddays, not date = date.adddays
                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        //get DayOfWeek for current day of currrent task
                        CalendarCell day = DaysInCurrentMonth.Where(d => d.CellDate.Date == date.Date).FirstOrDefault();

                        //day is another month so skip it
                        if (day == null)
                        { continue; }

                        //if day of week is monday, task should be displayed on first row
                        if (day.CellDate.DayOfWeek == DayOfWeek.Monday)
                        {
                            taskRow = 0;
                        }

                        //but if there are some tasks before, tasks won't be in first row, but after previous events
                        if (day.CellTasks.Children.Count > taskRow)
                        {
                            taskRow = Grid.GetRow(day.CellTasks.Children[day.CellTasks.Children.Count - 1]) + 1;
                        }

                        //get color for task

                        int accentColorIndex = accentColor % colors.Count();
                        CalendarTaskView calendarTaskView = new(colors[accentColorIndex], _mainWindow);

                        calendarTaskView.DataContext = t;
                        Grid.SetRow(calendarTaskView, taskRow);
                        day.CellTasks.Children.Add(calendarTaskView);
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
            if (CurrentDate.Month == 1)
            {
                CurrentDate = CurrentDate.AddYears(-1);
            }
            CurrentDate = CurrentDate.AddMonths(-1);
        }

        private void NextMonthButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentDate.Month == 12)
            {
                CurrentDate = CurrentDate.AddYears(1);
            }
            CurrentDate = CurrentDate.AddMonths(1);
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
    }
}
