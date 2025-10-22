using PlanningProgramV3.Models;
using PlanningProgramV3.Views.PlanControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{
    /**
     * Noah Gartenberg
     * Last Updated: 10/10/2025
     * This contains the view model for the Task Item
     * CalendarTasks are also the only ones able to contain subitems
     * 
     * Link to go to to see an example of what Dutton meant when he described when/how to redo the code in the models/viewmodels https://stackoverflow.com/questions/62743207/mvvm-solving-nested-models-and-viewmodels
     * 
     * Removed default constructor and refactored constructors to try and get saving data to work
     */
    public partial class TaskViewModel : PlannerItemViewModel
    {


        //create new observable collection for storing the subitem references in the view models
        //initialize in constructor
        private ObservableCollection<PlannerItemViewModel> subItemViewModels;


        #region Properties

#pragma warning disable CS8603 // Possible null reference return. If it is null, I want it to return as such
        //Task view model is the only object to actually need to expose the parent as a property
        public TaskViewModel Parent
        {
            get => parent;
        }
#pragma warning restore CS8603 // Possible null reference return.

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

        public Guid UUID
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
                    Trace.WriteLine("Printing for name");
                    PrintData();
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
        public new TaskModelData State
        {
            get => (TaskModelData)state;
            
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for creating a task view model from scratch
        /// </summary>
        /// <param name="parent"></param>
        public TaskViewModel(TaskViewModel parent) : base(parent, PlannerItemType.Task)
        {
            SubItems = [];
            AddSubItemCommand = new RelayCommand(AddSubItem, null);
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem, null);
        }

        /// <summary>
        /// Constructor to be called when passing in data that has already been formatted into a model, but doesn't yet have a corresponding view model
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="state"></param>
        public TaskViewModel(TaskViewModel parent, BaseItemModelData state) : base(parent, state, PlannerItemType.Task)
        {
            SubItems = [];
            AddSubItemCommand = new RelayCommand(AddSubItem, null);
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem, null);
        }
        /// <summary>
        /// Constructor for creating a new task view model when loading a plan into memory
        /// </summary>
        /// <param name="state"></param>
        public TaskViewModel(BaseItemModelData state) : base(state,PlannerItemType.Task)
        {
            //load subitems into list
            var temp = state as TaskModelData;
            for (int i = 0; i <temp.subItems.Count; i++)
            {
                subItemViewModels = [];
                var subItem = temp.subItems[i];
                switch(subItem.dataType)
                { 
                    case PlannerItemType.Task:
                        subItemViewModels.Add(new TaskViewModel(this,subItem as TaskModelData));
                        break;
                    case PlannerItemType.Text:
                        subItemViewModels.Add(new TextViewModel(this,subItem as TextModelData));
                        break;
                    case PlannerItemType.Date:
                        subItemViewModels.Add(new DateDurationViewModel(this,subItem as DateDurationModelData));
                        break;
                }
            }
            AddSubItemCommand = new RelayCommand(AddSubItem, null);
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem, null);
        }

        /// <summary>
        /// Default constructor if such a thing is necessary - and I believe it to be for this class and this class only
        /// </summary>
        public TaskViewModel() : base(PlannerItemType.Task)
        {
            SubItems = [];
            AddSubItemCommand = new RelayCommand(AddSubItem, null);
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem, null);
        }
        #endregion



        #region Commands
        public ICommand AddSubItemCommand { get; private set; }
        public ICommand RemoveSubItemCommand { get; private set; }

        #region Commmand related methods
        

        /// <summary>
        /// When inputting an item model, create a view model to represent the item and add it to the subitems collection
        /// do not have to add to the list in the model, because the data will come from the model, and be added to this list
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddSubItem(BaseItemModelData item)
        {
            if(item is TaskModelData)
            {
                subItemViewModels.Add(new TaskViewModel(this,item as TaskModelData));
            }
            else if(item is TextModelData)
            {
                subItemViewModels.Add(new TextViewModel(this,item as TextModelData));
            }
            else if(item is DateDurationModelData)
            {
                subItemViewModels.Add(new DateDurationViewModel(this, item as DateDurationModelData));
            }
            else
            {
                throw new System.NotImplementedException("Tried to add an unimplemented view model/model type to the subitems list");
            }

            OnPropertyChanged(nameof(SubItems));
        }
        public virtual void AddSubItem(object obj)
        {
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            PlannerItemViewModel addedItem = obj.ToString() switch
            {
                "Task" => new TaskViewModel(this),
                "Text" => new TextViewModel(this),
                "Date" => new DateDurationViewModel(this),
                //"Image" => new ImageItemViewModel(),
                //"Linker" => new PlanReferenceViewModel(),
                //_ => new TaskItemViewModel(),
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
            SubItems.Add(addedItem);
            State.AddItem(addedItem.State);
            PrintData();
            OnPropertyChanged(nameof(SubItems));
        }

        //Perhaps I should try to move the parent if this is false?? IDK
        //9/17/2025 - actually, waht does this method even do? The way Drag&Drop works rn, it will already move the top level parent anyway. 
        public virtual bool CanMoveTask(object? parameter)
        {
            return parent == null;
        }

        public virtual void RemoveSubItem(object obj)
        {

            throw new NotImplementedException("The RemoveSubItem method has not been properly implemented -- parameters should not be an object");
        }
        #endregion
        #endregion

        #region Methods
        
        public BaseItemModelData GetState() { return state; }

        public override void PrintData()
        {
            Trace.WriteLine("TaskModel: ");
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("Task Name: " + Name);
            Trace.WriteLine("Task Completion: " + IsComplete);
            Trace.WriteLine("Guid: " + UUID);
            for (int i = 0; i < SubItems.Count; i++)
            {
                SubItems[i].PrintData();
            }
        }
        #endregion
    }
}
