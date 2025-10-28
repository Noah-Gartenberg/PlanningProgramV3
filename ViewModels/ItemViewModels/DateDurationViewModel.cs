using PlanningProgramV3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{
    /**
     * Noah Gartenberg
     * Last Edited:10/10/2025
     * This class contains the view model data for the TimeSensetive Controls
     * 
     * Refactored constructors, added State property, and thought about removing parent file path
     */
    public partial class DateDurationViewModel : PlannerItemViewModel
    {
        #region Properties
        public new DateDurationModelData State
        {
            get => (DateDurationModelData)state;
        }

#warning Honestly, I'm not sure if this property/field is necessary - I may be better off just having an event that I can call in the main window view model to get the plan, but then do I need to store the plan's file name? 
        public string ParentFilePath
        {
            get => State.parentPlanFile;
            set
            {
                if (State.parentPlanFile != value)
                {
                    State.parentPlanFile = value;
                    OnPropertyChanged(nameof(ParentFilePath));
                }
            }
        }
        public Guid ParentUUID
        {
            get => State.parentTaskUUID;
            set
            {
                if (State.parentTaskUUID != value)
                {
                    State.parentTaskUUID = value;
                    OnPropertyChanged(nameof(ParentUUID));
                }
            }
        }

        public DateTime StartDate
        {
            get => State.startDate;
            set
            {
                //ensure that the new start date is not more than the end date - if it is, don't set the start date? Set it to the end date? Going with the former for now
                if (State.startDate != value && value <= EndDate)
                {
                    State.startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => State.endDate;
            set
            {
                //ensure that new end date is not less than the start date - if it is, same rule as start date, and just don't set
                if (State.endDate != value && value >= StartDate)
                {
                    State.endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor to call when creating a new date duration sub-item
        /// </summary>
        /// <param name="parent"></param>
        public DateDurationViewModel(TaskViewModel parent) : base(parent, PlannerItemType.Date)
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ParentUUID = parent.UUID;
        }
        /// <summary>
        /// Constructor to call when creating a new date duration sub-item from pre-existing data or state
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="state"></param>
        public DateDurationViewModel(TaskViewModel parent, BaseItemModelData state) : base(parent, state, PlannerItemType.Date)
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            ParentUUID = parent.UUID;
        }

        /// <summary>
        /// Default constructor for controls
        /// </summary>
        public DateDurationViewModel() : base(PlannerItemType.Date) 
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
        #endregion

        #region Methods
        public override void PrintData()
        {
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("StartDate: " + StartDate);
            Trace.WriteLine("EndDate: " + EndDate);
        }
        #endregion
    }
}
