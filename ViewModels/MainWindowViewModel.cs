using PlanningProgramV3.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlanningProgramV3.ViewModels
{
    /// <summary>
    /// Data in this class is stuff that a lot of the other classes will need to inherit from or acquire from the main window anyway
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand GetTasksFromTimePeriodCommand { get; private set; }


        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    OnPropertyChanged(nameof(CurrentDate));
                    //MessageBox.Show("Current Date is " + value); FOR TESTING PURPOSES
                    //technically shouldn't be changing this data in here, but tbh right now I don't care, just want to get a functioning prototype and then I'll refactor later
                    Tasks = new ObservableCollection<CalendarTaskData>(DataAccess.GetTasksFromSandwichMonths(currentDate));
                }
            }
        }

        private int currPlan = 0;
        public int CurrPlanIndex
        {
            get => currPlan;
            set
            {
                if (currPlan != value)
                {
                    currPlan = value;
                    OnPropertyChanged(nameof(CurrPlanIndex));
                }
            }
        }

        public PlannerViewModel CurrentPlan
        {
            get => plans[currPlan];
            set
            {
                if (plans[currPlan] != value)
                {
                    for (int i = 0; i < plans.Count; i++)
                    {
                        if (value == plans[i])
                        {
                            currPlan = i;
                            OnPropertyChanged(nameof(CurrentPlan));
                            return;
                        }
                    }
                    throw new ArgumentException("Value attempted to set CurrentPlan to did not exist in plans collection");
                }
            }
        }

        //list of plans being displayed (ignoring the month/week calendar)
        private ObservableCollection<PlannerViewModel> plans;
        public ObservableCollection<PlannerViewModel> Plans
        {
            get { return plans; }
            set
            {
                if (plans != value)
                {
                    plans = value;
                    OnPropertyChanged(nameof(Plans));
                }
            }
        }


        //list of tasks necessary
        private ObservableCollection<CalendarTaskData> tasks;
        public ObservableCollection<CalendarTaskData> Tasks
        {
            get { return tasks; }
            set
            {
                if (tasks != value)
                {
                    tasks = value;
                    OnPropertyChanged(nameof(Tasks));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            tasks = new ObservableCollection<CalendarTaskData>();
            plans = new ObservableCollection<PlannerViewModel>();
            GetTasksFromTimePeriodCommand = new RelayCommand(GetTasksFromTimePeriod,null);
            plans.Add(new PlannerViewModel());

            //initializecurrentDate
            currentDate = DateTime.Today;

            DataAccess.InitializeDatabase();
            Tasks = new ObservableCollection<CalendarTaskData>(DataAccess.GetTasksFromSandwichMonths(CurrentDate));

            //Initialize tasks
            
        }
        public virtual void GetTasksFromTimePeriod(object timePeriod)
        {
            //have to do things this way for the relay command to work
            if(timePeriod is string stateData)
            {
                switch (stateData)
                {
                    //currently, day shouldn't be used, but maybe someday in the future
                    case "Day":
                        break;
                    case "Week":
                        //in order for this to work, I need to set datetime variables, to ensure the dates are the right dates
                            //if I don't, I might wind up with 8/25/2025 as the start, and 8/1/2025 as the end (random example, totally -- what happened was I was adding seven days to the current date, which if it went into the next month could be lower than the first date, and so no tasks would be returned.
                        DateTime startDate = new DateTime(CurrentDate.Year, CurrentDate.Month, HelperMethods.GetFirstDayOfWeekDate(CurrentDate).Day);
                        DateTime endDate = startDate.AddDays(7);

                        Tasks = new ObservableCollection<CalendarTaskData>(
                            DataAccess.GetTasksFromWeek(startDate,endDate)
                            );
                        break;
                    case "Month":
                        Tasks = new ObservableCollection<CalendarTaskData>(
                            DataAccess.GetTasksFromMonth(
                                new DateTime(CurrentDate.Year, CurrentDate.Month, 1),
                                new DateTime(CurrentDate.Year, CurrentDate.Month, DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month))
                                )
                            );
                        break;
                }
            }
            else
            {
                throw new ArgumentException("Passed in object that was not of type TaskTimePeriodWrapper");
            }
            
        }

    }
}
