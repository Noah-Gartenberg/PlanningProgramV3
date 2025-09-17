using PlanningProgramV3.Models;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlanningProgramV3.ViewModels
{
    
    public class PlannerViewModel : INotifyPropertyChanged
    {
        
        

        //public List<PlannerItemViewModel> selectedTasks; // this is a placeholder list for the selected objects and stuff...
                                                         // should only have top level objects selected, but unsure how to do that rn, so won't
        private PlannerModelData data;

        //public RelayCommand SetPosition { get; }
        public RelayCommand AddTask { get; }


        /**
         * 
         * TO DO: MAKE A COMMAND AND FUNCTIONALITY FOR DELETING ITEMS!!! Will probably need to use events
         * 
         */
        public string FileName
        {
            get => data.fileName;
            set
            {
                if (data.fileName != value)
                {
                    data.fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }
        //add custom observable collection, and when add to it, add to the inheriting model too
        private ObservableCollection<TaskViewModel> highestTasks;
        public ObservableCollection<TaskViewModel> HighestTasks
        {
            get => highestTasks;
            set
            {
                if (highestTasks != value)
                {
                    highestTasks = value;
                    OnPropertyChanged(nameof(HighestTasks));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddHighestTask(object task)
        {
            if(task is TaskViewModel temp)
            {
                HighestTasks.Add(temp);
                //I know this is technically bad practice, but I needed a way to access state so that I could add it to the model data
                    //which I needed to do to ensure that the planner view model stored only view models, whereas I needed the models to contain only models so they could be serialized
                data.topPlanItems.Add(temp.State);
                OnPropertyChanged(nameof(HighestTasks));
            }
            else 
            {
                throw new ArgumentException("Should have been a TaskViewModel");
            }
            
        }

        //pass in the view model, to set its position
        public void SetTopTaskPosition(object InputViewModel)
        {
            //if I can ensure this is called on drop by binding to it...
            //MessageBox.Show("SetTopTaskPosition has not yet been implemented");
            //in theory should also check to see if can move objects, but this is good for now
        }

        /**
         * Adds a task to the top of the list at the user's mouse's position
         *      Unsure as of yet what the input will be - maybe mouse position
         *      may be passing in a reference to the canvas?
         */
        public void AddTopTask(object input)
        {
            TaskViewModel tempVar = new TaskViewModel();
            //not setting coordinates to mouse position because can't figure out how to make it work right now
            HighestTasks.Add(tempVar);
            OnPropertyChanged(nameof(HighestTasks));
        }

        public void SetTaskPosition(TaskViewModel task, Point position)
        {
            
        }


        /**
         * This method will delete an item
         * 
         */
        public void DeleteItem(object item)
        {

        }

        public PlannerViewModel()
        {
            data = new PlannerModelData();
            //SetPosition = new RelayCommand(SetTopTaskPosition, null);
            AddTask = new RelayCommand(AddTopTask, null);
            HighestTasks = new ObservableCollection<TaskViewModel>();
        }
    }
}
