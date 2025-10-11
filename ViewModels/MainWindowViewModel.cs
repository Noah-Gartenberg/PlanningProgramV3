using Microsoft.Win32;
using PlanningProgramV3.Models;
using PlanningProgramV3.ViewModels.Calendar;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace PlanningProgramV3.ViewModels
{
    /// <summary>
    /// Data in this class is stuff that a lot of the other classes will need to inherit from or acquire from the main window anyway
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public RelayCommand GetTasksFromTimePeriodCommand { get; private set; }
        public RelayCommand SaveCurrentPlan { get; private set; }
        public RelayCommand LoadPlan { get; private set; }


        #region State Data
        public ProgramConfig Config { get; private set; }

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
                    CalendarTasks = new ObservableCollection<CalendarTaskData>(DataAccess.GetTasksFromSandwichMonths(currentDate));
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


        //list of calendarTasks necessary
        private ObservableCollection<CalendarTaskData> calendarTasks;
        public ObservableCollection<CalendarTaskData> CalendarTasks
        {
            get { return calendarTasks; }
            set
            {
                if (calendarTasks != value)
                {
                    calendarTasks = value;
                    OnPropertyChanged(nameof(CalendarTasks));
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            //Make sure that there is a program config in existence, and that we have a reference to wherever files ought to be stored
            //I just realized this will cause issues with the way I've handled versions, but we'll deal with that later I guess

            //to accomplish this, check if there exists a file in bin called ProgramConfig that is of .xml file type
            //when I ship this eventaully, what will be in the files? NEED TO FIGURE THIS OUT
            string path = AppDomain.CurrentDomain.BaseDirectory + "/ProgramConfig.xml";
            if(File.Exists(path))
            {
                //if the file exists, then pull data from it, instead of creating a new one/overwriting it
                FileStream fsout = new FileStream(path, FileMode.Open, FileAccess.Read);
                XmlSerializer serializer = new XmlSerializer(typeof(ProgramConfig));
                Config = (ProgramConfig)serializer.Deserialize(fsout);

                fsout.Close();
            }
            else
            {
                //IF THIS IS THE CASE, THEN USERS HAVE NEVER OPENED THE PROGRAM BEFORE AND SO SOME THINGS NEED TO HAPPEN - TUTORIAL TO USE

                //NOTE TO SELF, WE SEEM TO BE DOING THIS BIT HERE WRONG - MIGHT NEED TO USE ANOTHER METHOD TO ACCESS THE BIN, BUT I BELIEVE THE BIN WILL BE ACCESSIBLE WHEN SHIPPED
                FileStream fsout = new FileStream(path, FileMode.OpenOrCreate);
                XmlSerializer serializer = new XmlSerializer(typeof(ProgramConfig));

                Config = new ProgramConfig(AppDomain.CurrentDomain.BaseDirectory);

                serializer.Serialize(fsout, Config);
                fsout.Close();
            }

            #region TESTING PURPOSES
            //override path to be in documents folder for stuff
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlannerProgramDocs");

            //directory.createdirectory won't create directory if it already exists.
            Directory.CreateDirectory(path);

            Config.SetFileStoragePath(path);




            #endregion


            calendarTasks = new ObservableCollection<CalendarTaskData>();
            plans = new ObservableCollection<PlannerViewModel>();


            GetTasksFromTimePeriodCommand = new RelayCommand(GetTasksFromTimePeriod, null);
            SaveCurrentPlan = new RelayCommand(SavePlan_Command, CanSavePlan);
            LoadPlan = new RelayCommand(LoadPlan_Command, null);

            plans.Add(new PlannerViewModel());

            //initializecurrentDate
            currentDate = DateTime.Today;

            DataAccess.InitializeDatabase();
            CalendarTasks = new ObservableCollection<CalendarTaskData>(DataAccess.GetTasksFromSandwichMonths(CurrentDate));

        }
        #region Methods
        public virtual void GetTasksFromTimePeriod(object timePeriod)
        {
            //have to do things this way for the relay command to work
            if (timePeriod is string stateData)
            {
                switch (stateData)
                {
                    //currently, day shouldn't be used, but maybe someday in the future
                    case "Day":
                        break;
                    case "Week":
                        //in order for this to work, I need to set datetime variables, to ensure the dates are the right dates
                        //if I don't, I might wind up with 8/25/2025 as the start, and 8/1/2025 as the end (random example, totally -- what happened was I was adding seven days to the current date, which if it went into the next month could be lower than the first date, and so no calendarTasks would be returned.
                        DateTime startDate = new DateTime(CurrentDate.Year, CurrentDate.Month, HelperMethods.GetFirstDayOfWeekDate(CurrentDate).Day);
                        DateTime endDate = startDate.AddDays(7);

                        CalendarTasks = new ObservableCollection<CalendarTaskData>(
                            DataAccess.GetTasksFromWeek(startDate, endDate)
                            );
                        break;
                    case "Month":
                        CalendarTasks = new ObservableCollection<CalendarTaskData>(
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

        #endregion


        /**
         * Goal is going to be to move a lot of the code in the code behind for the main windows into the below region
         */

        #region Main Window Interaction Commands





        /// <summary>
        /// This method will load a plan from an xml file into memory
        /// It will need to throw an error if a file that's not an xml file, or if it's not the correct format of xml file
        /// </summary>
        /// <param name="source">Unsure if this will be a string of the filename (if I'm doing the file dialogue in this class or in the code-behind)</param>
        protected virtual void LoadPlan_Command(object source)
        {

        }

        /// <summary>
        /// This method will save a plan into an xml file
        /// It will need to throw an error if a file that's not an xml file is saved
        /// </summary>
        /// <param name="source">Unsure if this will be a string of the filename to save to (if I'm doing the file dialogue in this class or in the code-behind)</param>

        protected virtual void SavePlan_Command(object source)
        {
            Plans[currPlan].TrySaveToFile(Config.fileStoragePath);
            
            
        }

        /// <summary>
        /// This method will check if the current plan a) needs to be saved, and b) if it has been saved before
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual bool CanSavePlan(object source)
        {
            //return true, if the dirty flag is true, or if the current plan does not yet have a file name
            //added true for testing purposes
            return Plans[currPlan].DirtyFlag || Plans[currPlan].FileName.Equals("") || true;
        }



        #endregion

    }
}
