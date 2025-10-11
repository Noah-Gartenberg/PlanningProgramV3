using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlanningProgramV3.Models;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{

    /**
     * Noah Gartenberg
     * Last Updated: 10/10/2025
     * The goal of this class is to provide an abstract class with commmon methods for the views to display
     * 
     * also, realized this class didn't inherit from INotifyPropertyChanged so added it here
     */
    public abstract class PlannerItemViewModel : INotifyPropertyChanged
    {
        public PlannerItemType Type { get; private set; }


        #region Fields
        protected BaseItemModelData state;

        public BaseItemModelData State
        {
            get => state;
            //10/10/2025 - added an "onpropertychanged" call to this, in case that might change things
            private set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }


        //Storing parent in the base planner itemm class to ensure that date duration and linker (planned but not yet implemented) items can be handled correctly
        //made parent nullable
        protected TaskViewModel? parent;
        //Removed the Parent property, as parent isn't used in the views, and also, I have the set parent method



        #endregion

        #region Property Changed Event
        public virtual event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructors

        //10/10/2025 - removed other two constructors, and reworked to take two arguments, and then reworked them. Added a data type parameter to store the data type. 
        /// <summary>
        /// This will be my preffered constructor to call in the view models, for when a state object is being passed in. 
        /// HOWEVER, THE PREFERENCE WOULD BE TO AVOID USING THIS ONE AND TO INSTEAD USE ONE THAT INSTANTIATES THE STATE DATA WITHIN ITSELF
        /// </summary>
        /// <param name="parent">The object that the item is the child of</param>
        /// <param name="state">Assume that the state's parenting is set up correctly too</param>
        /// <param name="type">The type of planner item that the object is</param>
        public PlannerItemViewModel(TaskViewModel parent, BaseItemModelData state, PlannerItemType type)
        {
            this.parent = parent;
            this.state = state;
            Type = type;
        }

        /// <summary>
        /// Constructor for when there are task view models that don't have parents (such as when they are the top-most calendarTasks) are instantiated
        /// HOWEVER, THE PREFERENCE WOULD BE TO AVOID USING THIS ONE AND TO INSTEAD USE ONE THAT INSTANTIATES THE STATE DATA WITHIN ITSELF
        /// </summary>
        /// <param name="state">Assumes state's parent data is already set up</param>
        /// <param name="type"></param>
        public PlannerItemViewModel(BaseItemModelData state, PlannerItemType type)
        {
            this.state = state;
            Type = type;
        }

        public PlannerItemViewModel(TaskViewModel parent, PlannerItemType type)
        {
            this.parent = parent;
            Type = type;


            BaseItemModelData newState;

#warning Ensure that the states of the view models are set up correctly, to maintain a synced state between the view models and the models' states
            //The way I've refactored the code right now should allow me to bypass any mess ups with parenting data, as soon as I figure out how to refactor this code such that the parent can be passed in by reference instead of by value
            switch (Type)
            {
                case PlannerItemType.Task:
                    newState = new TaskModelData(parent.State);
                    break;
                case PlannerItemType.Text:
                    newState = new TextModelData(parent.State);
                    break;
                case PlannerItemType.Date:
                    newState = new DateDurationModelData(parent.State);
                    break;
                default:
                    throw new NotImplementedException("The item view model you tried to create of type " + Type + " is not yet implemented, or is of generic type and will not have functionality");
                    break;
            }
            this.state = newState;
        }

        /// <summary>
        /// This constructor should only be called if a task model is being added, without a parent
        /// OKAY BUT I WAS WRONG - 11:27 PM NOAH HERE, I WAS WRONG: CONSTRUCTOR GETS CALLED AUTOMATICALLY BY THE USER CONTROLS!!!
        /// </summary>
        /// <param name="type"></param>
        public PlannerItemViewModel(PlannerItemType type)
        {
            Type = type;
            BaseItemModelData newState;
            switch (Type)
            {
                case PlannerItemType.Task:
                    parent = null;
                    newState = new TaskModelData();
                    break;
                case PlannerItemType.Text:
                    newState = new TextModelData();
                    break;
                case PlannerItemType.Date:
                    newState = new DateDurationModelData();
                    break;
                default:
                    throw new NotImplementedException("The item view model you tried to create of type " + Type + " is not yet implemented, or is of generic type and will not have functionality");
                    break;
            }
            state = newState;


            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Made a method to print data to see if models and view models were out of sink with each other
        /// </summary>
        public abstract void PrintData();

        /// <summary>
        /// Class that is called to change the parent of an object. Required because apparently the user controls call default constructors
        /// </summary>
        /// <param name="parent"></param>
        public virtual void SetParent(TaskViewModel parent)
        {
            this.parent = parent;
            this.state.parent = parent.State;

        }
        #endregion
    }
}
