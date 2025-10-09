using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanningProgramV3.Models;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{
    /**
     * This class contains the view model data for the TimeSensetive Controls
     */
    public class DateDurationViewModel : PlannerItemViewModel
    {
        #region Properties
        public string ParentFilePath
        {
            get => ((DateDurationModelData)state).parentPlanFile;
            set
            {
                if (((DateDurationModelData)state).parentPlanFile != value)
                {
                    ((DateDurationModelData)state).parentPlanFile = value;
                    OnPropertyChanged(nameof(ParentFilePath));
                }
            }
        }
        public Guid ParentUUID
        {
            get => ((DateDurationModelData)state).parentTaskUUID;
            set
            {
                if (((DateDurationModelData)state).parentTaskUUID != value)
                {
                    ((DateDurationModelData)state).parentTaskUUID = value;
                    OnPropertyChanged(nameof(ParentUUID));
                }
            }
        }

        public DateTime StartDate
        {
            get => ((DateDurationModelData)state).startDate;
            set
            {
                //ensure that the new start date is not more than the end date - if it is, don't set the start date? Set it to the end date? Going with the former for now
                if (((DateDurationModelData)state).startDate != value && value <= EndDate)
                {
                    ((DateDurationModelData)state).startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => ((DateDurationModelData)state).endDate;
            set
            {
                //ensure that new end date is not less than the start date - if it is, same rule as start date, and just don't set
                if (((DateDurationModelData)state).endDate != value && value >= StartDate)
                {
                    ((DateDurationModelData)state).endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        #endregion

        #region Constructors
        public DateDurationViewModel(DateDurationModelData setState) : base(setState) { }

        public DateDurationViewModel(TaskViewModel? parent) : base(parent.State)
        {

        }

        public DateDurationViewModel() : base(new DateDurationModelData())
        {

        }
        #endregion
    }
}
