using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanningProgramV3.Models;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{

    /**
     * Noah Gartenberg
     * Last Updated: 7/11/2025
     * The goal of this class is to provide an abstract class with commmon methods for the views to display
     */
    public class PlannerItemViewModel
    {

        #region Fields
        protected BaseItemModelData state;
        //public TaskViewModel? HighestLevelParent { get; set; }
        private TaskViewModel parent;
        public TaskViewModel? Parent 
        {
            get => parent;
            set
            {
                if(parent != value)
                {
                    parent = value;
                    //do this to maintain mvvm
                    state.parent = parent.state as TaskModelData;
                    OnPropertyChanged(nameof(Parent));
                }
            }
        }

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
            if (Parent != null)
            {
                Parent = parent;
                state.parent = parent.state as TaskModelData;
            }

        }

        
        //public void SetHighestParent(TaskViewModel? parent)
        //{
        //    if (Parent != null)
        //    {
        //        HighestLevelParent = parent;
        //    }
        //}
        #endregion
    }
}
