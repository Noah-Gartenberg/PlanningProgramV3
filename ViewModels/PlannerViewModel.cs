using Microsoft.UI.Xaml.Controls;
using Microsoft.Win32;
using PlanningProgramV3.Models;
using PlanningProgramV3.ViewModels.ItemViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace PlanningProgramV3.ViewModels
{
    public partial class PlannerViewModel : INotifyPropertyChanged
    {

        #region Fields and Properties
        /**
         * 
         * Information for storing the viewport and camera data
         * Need to store the camera location (center of screen), camera pan amount, and camera zoom amount
         */
        private Point CameraLoc;
        private int cameraPanAmount;

        private PlannerModelData data;

        //Dirty flag - if true, needs to be saved
        private bool dirtyFlag;
        public bool DirtyFlag 
        { 
            get => dirtyFlag;
            set
            {
                if (dirtyFlag != value)
                {
                    dirtyFlag = value;
                    OnPropertyChanged(nameof(DirtyFlag));
                }
            }
        }

        //public RelayCommand SetPosition { get; }
        public RelayCommand AddTask { get; }
        public RelayCommand RemoveTask { get; }

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
                    foreach (var task in highestTasks)
                    {
                        data.planTasks.Add(task.State);
                    }

                    OnPropertyChanged(nameof(HighestTasks));
                }
            }
        }

        /**
         * Selected index item meanings:
         *         selectedTaskIndex < -1: subitem has been selected (parent-item sub-item is within is amount less than -1?)
         *         selectedIndex = -1: no item has been selected
         *         selectedIndex >= 0: parent task item selected
         * 
         * 
         * 
         * 
         */
        private int selectedTaskIndex = -1;
        public int SelectedTaskIndex
        {
            get { return selectedTaskIndex; }
            set
            {
                if(selectedTaskIndex != value)
                {
                    selectedTaskIndex = value;
                    OnPropertyChanged(nameof(SelectedTaskIndex));
                }
            }
        }
        
        #region Property Changed

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor a plan that has been created
        /// will need to create another constructor to handle creating other data
        /// </summary>
        public PlannerViewModel()
        {
            data = new PlannerModelData();
            //SetPosition = new RelayCommand(SetTopTaskPosition, null);
            AddTask = new RelayCommand(AddNewTask, null);
            RemoveTask = new RelayCommand(DeleteHighestTask, null);
            HighestTasks = [];
        }

        #endregion

        #region Methods
        public void TrySaveToFile(string filepath)
        {
            PrintViewModels();
            data.PrintPlannerDataMethod();
            //save data
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = filepath;
            saveFileDialog.Filter = "xml file (*.xml)|*.xml";
            if(FileName == "")
            {
                FileName = "Untitled";
                
            }
            saveFileDialog.FileName = FileName;
            if (saveFileDialog.ShowDialog() == true)
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    File.Delete(saveFileDialog.FileName);
                }
                FileStream fsout = new FileStream(saveFileDialog.FileName, FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(PlannerModelData));
                serializer.Serialize(fsout, data);
                fsout.Close();
            }
        }

        public void TryLoadFromFile(string filepath)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog();
            openFileDialogue.InitialDirectory = filepath;
            openFileDialogue.Filter = "xml file (*.xml)|*.xml";
            if(openFileDialogue.ShowDialog() == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PlannerModelData));
                using(XmlReader reader = XmlReader.Create(openFileDialogue.FileName))
                {
                    data = (PlannerModelData)serializer.Deserialize(reader);
                }
                
            }
            //name has been updated
            OnPropertyChanged(nameof(FileName));

            //need to loop through highest tasks, and add it to the list, based on what is in the plan 
            //highest tasks must be manually updated, add to the field, not the property, so that in theory, the property doesn't update the model behind it
            for (int i = 0; i < data.planTasks.Count; i++)
            {
                
                highestTasks.Add(new TaskViewModel(data.planTasks[i]));
                
            }
            OnPropertyChanged(nameof(HighestTasks));
        }

        /// <summary>
        /// This method will add a new task to the plan
        /// </summary>
        /// <param name="shouldBeNull">Name is self-explanatory. It should literally be null, as there is no use for the variable, but I am using it anyway for some reason... 
        ///                                 I should probably create no-parameter, and multi parameter (use a stack) version of the command.</param>
        public void AddNewTask(object shouldBeNull)
        {
            //create the task
            TaskViewModel temp = new TaskViewModel();

            HighestTasks.Add(temp);

            data.AddTask(temp.State);

            OnPropertyChanged(nameof(HighestTasks));
        }


        /// <summary>
        /// Deletes a task known to be in the list of top tasks,
        ///         should not be used for tasks that may be lower level items
        /// </summary>
        /// <param name="item"></param>
        public void DeleteHighestTask(object item)
        {
            if(item is TaskViewModel itemToDelete)
            {
                for (int i = 0; i < HighestTasks.Count; i++)
                {
                    //shoudl do a reference equals - if I am correct
                    if (HighestTasks[i] == itemToDelete);
                    HighestTasks.RemoveAt(i);
                    data.RemoveTask(i);
                    OnPropertyChanged(nameof(HighestTasks));
                    return; //don't even need to break out of loop, can just return
                }
            }
            else if(item is int itemIndex)
            {
                HighestTasks.RemoveAt(itemIndex);
                data.RemoveTask(itemIndex);
                OnPropertyChanged(nameof(HighestTasks));
            }
                
                
                throw new ArgumentException("item passed in was not of type TaskViewModel or integer");
        }

        /// <summary>
        /// Deletes an item from somewhere in the list of the parent item
        /// </summary>
        /// <param name="itemToDelete"></param>
        /// <param name="itemParent">Technically not necesssary, but screw it</param>
        public void DeleteItem(PlannerItemViewModel itemToDelete, TaskViewModel itemParent)
        {

        }


        /// <summary>
        /// Method for testing what is inside the view model
        /// </summary>
        public void PrintViewModels()
        {
            Trace.WriteLine("File: " + FileName);
            for (int i = 0; i < highestTasks.Count; i++)
            {
                highestTasks[i].PrintData();
            }
        }

        /// <summary>
        /// Method for testing what data is inside the model
        /// </summary>
        public void PrintModels()
        {
            Trace.WriteLine("File: " + FileName);
            for(int i = 0; i < highestTasks.Count; i++)
            {
                highestTasks[i].State.PrintData();
            }
        }
        #endregion
    }
}
