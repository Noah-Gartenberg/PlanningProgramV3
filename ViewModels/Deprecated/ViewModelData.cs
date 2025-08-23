using PlanningProgramV3.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PlanningProgramV3.ViewModels.Deprecated
{
    
    /**
     * Noah Gartenberg
     * Last Updated: 7/11/2025
     * Struct to handle coordinate and any other such information
     *  which must be common between structs the items, but is not tied to state
     *  and therefore does not need to be inherited from like the state does.
     */
    public struct EssentialInfo
    {
        public float x;
        public float y;
        
    }

    /**
     * Noah Gartenberg
     * Last Updated: 7/11/2025
     * The goal of this class is to provide an abstract class with commmon methods for the views to display
     */
    public class PlannerItemViewModel
    {

        #region Fields
        protected BaseItemModelData state;
        public TaskViewModel? Parent { get; set; }

        #endregion

        #region Property Changed Event
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructors
        public PlannerItemViewModel(BaseItemModelData data)
        {
            state = data;
        }
        #endregion

        #region Methods

        /**
         * For setting the parent of the subitems of a task -- Parent should only be set if null, otherwise it shouldn't be touched.
         */
        public void SetParent(TaskViewModel? parent)
        {
            if(Parent != null)
            {
                Parent = parent;
            }
            
        }
        #endregion
    }

    /**
     * Noah Gartenberg
     * Last Updated: 8/6/2025
     * This contains the view model for the Task Item
     * Tasks are also the only ones able to contain subitems
     */
    public class TaskViewModel : PlannerItemViewModel
    {


        #region Fields
        protected ObservableCollection<PlannerItemViewModel> subItems;

        protected EssentialInfo coords;
        protected string uuid;
        #endregion

        #region Properties

        public string UUID
        {
            get => uuid;
            set
            {
                if (uuid != value)
                {
                    uuid = value;
                    OnPropertyChanged(nameof(UUID));
                }
            }
        }

        public virtual float XCoord
        {
            get => coords.x;
            set
            {
                if (value != coords.x)
                {
                    coords.x = value;
                    OnPropertyChanged(nameof(XCoord));
                }
            }
        }

        public virtual float YCoord
        {
            get => coords.y;
            set
            {
                if (value != coords.y)
                {
                    coords.y = value;
                    OnPropertyChanged(nameof(YCoord));
                }
            }
        }
        public bool IsComplete
        {
            get => ((TaskModelData)state).isCompleted;
            set
            {
                //If there is an error where nothing updates when this is set or unset, look ehre
                //this may not be changing the data at the reference...
                TaskModelData theData = ((TaskModelData)state);
                if (value != theData.isCompleted)
                {
                    theData.isCompleted = value;
                    OnPropertyChanged(nameof(IsComplete));
                }
            }
        }

        public string Name
        {
            get => ((TaskModelData)state).taskName;
            set
            {
                TaskModelData theData = ((TaskModelData)state);
                if (!value.Equals(theData.taskName))
                {
                    theData.taskName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public ObservableCollection<PlannerItemViewModel> SubItems
        {
            get => subItems;
            set
            {
                subItems = value;
                OnPropertyChanged(nameof(SubItems));
            }
        }

        #endregion

        #region Constructors
        public TaskViewModel() : base(new TaskModelData()) 
        {
            //startDate = DateTime.MinValue; endDate = DateTime.MinValue;
            this.subItems = [];
            AddSubItemCommand = new RelayCommand(AddSubItem);
            RemoveSubItemCommand = new RelayCommand(RemoveSubItem);
        }
        #endregion



        #region Commands
        public ICommand AddSubItemCommand { get; private set; }
        public ICommand RemoveSubItemCommand { get; private set; }

        #region Commmand related methods
        public virtual void AddSubItem(object obj)
        {
            PlannerItemViewModel addedItem = obj.ToString() switch
            {
                "Task" => new TaskViewModel(),
                "Text" => new TextViewModel(),
                //"Image" => new ImageItemViewModel(),
                //"Linker" => new PlanReferenceViewModel(),
                //_ => new TaskItemViewModel(),
            };
            System.Console.WriteLine("Adding object");
            addedItem.SetParent(this);
            subItems.Add(addedItem);
            OnPropertyChanged(nameof(SubItems));
        }

        public virtual void RemoveSubItem(object obj)
        {

            throw new NotImplementedException("The RemoveSubItem method has not been properly implemented -- parameters should not be an object");
            subItems.Remove(obj as PlannerItemViewModel);
            OnPropertyChanged(nameof(SubItems));
        }
        #endregion
        #endregion
    }

    /** 
     * Noah Gartenberg
     * Last Updated: 7/11/2025
     * This will contain the view model for the Text Item
     */
    public class TextViewModel : PlannerItemViewModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Text
        {
            //If there is an error where nothing updates when this is set or unset, look ehre
            //this may not be changing the data at the reference...
            get => ((TextModelData)state).text;
            set
            {
                if(!value.Equals((((TextModelData)state).text)))
                {
                    ((TextModelData)state).text = value; 
                    OnPropertyChanged(nameof(Text));
                }
            }
        }
        #endregion

        #region Constructors
        public TextViewModel() : base(new TextModelData()) { }
        #endregion

    }

    /**
     * This class contains the view model data for the TimeSensetive Controls
     */
    public class TaskDurationViewModel : PlannerItemViewModel
    {
        #region Properties
        public string ParentFilePath
        {
            get => ((TaskDurationData)state).parentPlanFile;
            set
            {
                if (((TaskDurationData)state).parentPlanFile != value)
                {
                    ((TaskDurationData)state).parentPlanFile = value;
                    OnPropertyChanged(nameof(ParentFilePath));
                }
            }
        }
        public string ParentUUID
        {
            get => ((TaskDurationData)state).parentTaskUUID;
            set
            {
                if(((TaskDurationData)state).parentTaskUUID != value)
                {
                    ((TaskDurationData)state).parentTaskUUID = value;
                    OnPropertyChanged(nameof(ParentUUID));
                }
            }
        }

        public DateTime StartDate
        {
            get => ((TaskDurationData)state).startDate;
            set
            {
                if (((TaskDurationData)state).startDate != value)
                {
                    ((TaskDurationData)state).startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => ((TaskDurationData)state).endDate;
            set
            {
                if (((TaskDurationData)state).endDate != value)
                {
                    ((TaskDurationData)state).endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        #endregion

        #region Constructors
        public TaskDurationViewModel() : base(new TaskDurationData()) 
        { 
            
        }
        #endregion
    }
}
