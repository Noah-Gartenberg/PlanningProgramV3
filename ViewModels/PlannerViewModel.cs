using PlanningProgramV3.Models;
using PlanningProgramV3.ViewModels.ItemViewModels;
using PlanningProgramV3.Views.PlanControls;
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
    public class PlannerViewModel : INotifyPropertyChanged
    {
        //public List<PlannerItemViewModel> selectedTasks; // this is a placeholder list for the selected objects and stuff...
                                                         // should only have top level objects selected, but unsure how to do that rn, so won't
        
        private PlannerModelData data;

        public RelayCommand SetPosition { get; }

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
            if(task is TaskUserControl temp)
            {
                data.topPlanItems.Add(temp.DataContext as TaskViewModel);
                OnPropertyChanged(nameof(HighestTasks));
            }
            else
            {
                throw new ArgumentException("Should have been a TaskUserControl");
            }
            
        }

        //pass in the view model, to set its position
        public void SetTopTaskPosition(object InputViewModel)
        {
            MessageBox.Show("SetTopTaskPosition has not yet been implemented");
            //in theory should also check to see if can move objects, but this is good for now
        }

        //removes view model from list

        public void RemoveTopTask(object task)
        {

            if (task is TaskUserControl temp)
            {
                data.topPlanItems.Remove(temp.DataContext as TaskViewModel);
                OnPropertyChanged(nameof(HighestTasks));
            }
            else
            {
                throw new ArgumentException("Should have been a TaskUserControl");
            }
        }

        public PlannerViewModel()
        {
            data = new PlannerModelData();
            SetPosition = new RelayCommand(SetTopTaskPosition, null);
        }
    }
}
