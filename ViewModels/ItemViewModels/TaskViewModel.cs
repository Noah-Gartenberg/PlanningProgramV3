using PlanningProgramV3.Models;
using PlanningProgramV3.Views.PlanControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{
    /**
     * Noah Gartenberg
     * Last Updated: 8/6/2025
     * This contains the view model for the Task Item
     * Tasks are also the only ones able to contain subitems
     */
    public class TaskViewModel : PlannerItemViewModel
    {

        //create new observable collection for storing the subitem references in the view models
            //initialize in constructor
        private ObservableCollection<PlannerItemViewModel> subItemViewModels;


        #region Properties

        public Point Coordinates
        {
            get => State.coordinates;
            set
            {
                if(State.coordinates != value)
                {
                    State.coordinates = value;
                    OnPropertyChanged(nameof(Coordinates));
                }
            }
        }

        public string UUID
        {
            get => State.uuid;
            set
            {
                if (State.uuid != value)
                {
                    State.uuid = value;
                    OnPropertyChanged(nameof(UUID));
                }
            }
        }

        public virtual double X
        {
            get => State.coordinates.X;
            set
            {
                if (value != State.coordinates.X)
                {
                    State.coordinates.X = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public virtual double Y
        {
            get => State.coordinates.Y;
            set
            {
                if (value != State.coordinates.Y)
                {
                    State.coordinates.Y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }
        public bool IsComplete
        {
            get => State.isCompleted;
            set
            {
                //If there is an error where nothing updates when this is set or unset, look ehre
                //this may not be changing the data at the reference...;
                if (value != State.isCompleted)
                {
                    State.isCompleted = value;
                    OnPropertyChanged(nameof(IsComplete));
                }
            }
        }

        public string Name
        {
            get => State.taskName;
            set
            {
                if (!value.Equals(State.taskName))
                {
                    State.taskName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public ObservableCollection<PlannerItemViewModel> SubItems
        {
            get => subItemViewModels;
            set
            {
                if (subItemViewModels != value)
                {
                    subItemViewModels = value;
                    OnPropertyChanged(nameof(SubItems));
                }
            }
        }

        /**
         * Getter property for state - SHOULD ONLY BE USED BY PLANNER MODEL, and only in this class. SHOULD NOT BE SET
         */
        public TaskModelData State
        {
            get => (TaskModelData)state;
            
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public TaskViewModel() : base(new TaskModelData())
        {
            //startDate = DateTime.MinValue; endDate = DateTime.MinValue;

            //Create observable collection with view models from state
            SubItems = new ObservableCollection<PlannerItemViewModel>();
            if(State.subItems != null)
            {
                foreach (BaseItemModelData item in State.subItems)
                {
                    switch (item.dataType)
                    {
                        case PlannerItemType.Task:
                            SubItems.Add(new TaskViewModel(item as TaskModelData));
                            break;
                        case PlannerItemType.Text:
                            SubItems.Add(new TextViewModel(item as TextModelData));
                            break;
                        case PlannerItemType.Date:
                            SubItems.Add(new DateDurationViewModel(item as DateDurationModelData));
                            break;
                        default:
                            throw new ArgumentException("item in model data subitems was not of type supported by current task view model code");
                            break;
                    }
                }
            }
            AddSubItemCommand = new RelayCommand(AddSubItem,CanMoveTask);
            System.Console.WriteLine("Currently, task view model does not check for a selected object to see if can delete an object");
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem,null);
        }


        //Constructor for making a new task with a specific state
        public TaskViewModel(TaskModelData setState) : base(setState) {    }
        //constructor for making a new task at specific coordinates
        public TaskViewModel(Point coords) : base(new TaskModelData())
        {
            //startDate = DateTime.MinValue; endDate = DateTime.MinValue;
            //((TaskModelData)state).subItems = [];
            //AddSubItemCommand = new RelayCommand(AddSubItem, CanMoveTask);
            //System.Console.WriteLine("Currently, task view model does not check for a selected object to see if can delete an object");
            //RemoveSubItemCommand = new RelayCommand(RemoveSubItem, null);

            State.coordinates = coords;
        }
        #endregion



        #region Commands
        public ICommand AddSubItemCommand { get; private set; }
        public ICommand RemoveSubItemCommand { get; private set; }

        #region Commmand related methods
        //9/17/2025 TO DO!!! CHANGE THIS TO CREATE THE VIEW MODELS FROM STATE. //also, need to figure out how to make this work for copy paste
        public virtual void AddSubItem(object obj)
        {
            PlannerItemViewModel addedItem = obj.ToString() switch
            {
                "Task" => new TaskViewModel(),
                "Text" => new TextViewModel(),
                "Date" => new DateDurationViewModel(),
                //"Image" => new ImageItemViewModel(),
                //"Linker" => new PlanReferenceViewModel(),
                //_ => new TaskItemViewModel(),
            };
            System.Console.WriteLine("Adding object");
            addedItem.SetParent(this.state);
            SubItems.Add(addedItem);
            OnPropertyChanged(nameof(SubItems));
        }

        //Perhaps I should try to move the parent if this is false?? IDK
        //9/17/2025 - actually, waht does this method even do? The way Drag&Drop works rn, it will already move the top level parent anyway. 
        public virtual bool CanMoveTask(object? parameter)
        {
            return Parent == null;
        }

        public virtual void RemoveSubItem(object obj)
        {

            throw new NotImplementedException("The RemoveSubItem method has not been properly implemented -- parameters should not be an object");
            SubItems.Remove(obj as PlannerItemViewModel);
            OnPropertyChanged(nameof(SubItems));
        }
        #endregion
        #endregion

        #region Methods
        
        public BaseItemModelData GetState() { return state; }
        #endregion
    }
}
