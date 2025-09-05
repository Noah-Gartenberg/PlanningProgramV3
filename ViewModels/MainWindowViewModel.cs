using PlanningProgramV3.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.ViewModels
{
    /// <summary>
    /// Data in this class is stuff that a lot of the other classes will need to inherit from or acquire from the main window anyway
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //list of plans being displayed (ignoring the month/week calendar)
        private ObservableCollection<PlannerViewModel> plans;
        public ObservableCollection<PlannerViewModel> Plans
        {
            get {  return plans; }
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

        //date occurring
        private DateTime dateSelected;
        public DateTime DateSelected
        {
            get { return dateSelected; }
            set
            {
                if (dateSelected != value)
                {
                    dateSelected = value;
                    OnPropertyChanged(nameof(DateSelected));
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

            DataAccess.InitializeDatabase();

            Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK", true, DateTime.Today, DateTime.Today.AddDays(1)));
            Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK2", true, DateTime.Today, DateTime.Today.AddDays(3)));
            Tasks.Add(new CalendarTaskData("GUID1", true, true, "TASK3", true, DateTime.Today.AddDays(-6), DateTime.Today.AddDays(1)));
        }

        
    }
}
