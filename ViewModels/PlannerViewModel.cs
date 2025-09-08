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

        public ObservableCollection<TaskViewModel> HighestTasks
        {
            get => data.topPlanItems;
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
                data.topPlanItems.Add(temp);
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
            MessageBox.Show("SetTopTaskPosition has not yet been implemented");
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
            HighestTasks.Add(new TaskViewModel());
        }
    }
}
