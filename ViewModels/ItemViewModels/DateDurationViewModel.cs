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
    public class TaskDurationViewModel : PlannerItemViewModel
    {
        #region Properties
        public string ParentFilePath
        {
            get => ((TaskDurationModelData)state).parentPlanFile;
            set
            {
                if (((TaskDurationModelData)state).parentPlanFile != value)
                {
                    ((TaskDurationModelData)state).parentPlanFile = value;
                    OnPropertyChanged(nameof(ParentFilePath));
                }
            }
        }
        public string ParentUUID
        {
            get => ((TaskDurationModelData)state).parentTaskUUID;
            set
            {
                if (((TaskDurationModelData)state).parentTaskUUID != value)
                {
                    ((TaskDurationModelData)state).parentTaskUUID = value;
                    OnPropertyChanged(nameof(ParentUUID));
                }
            }
        }

        public DateTime StartDate
        {
            get => ((TaskDurationModelData)state).startDate;
            set
            {
                //ensure that the new start date is not more than the end date - if it is, don't set the start date? Set it to the end date? Going with the former for now
                if (((TaskDurationModelData)state).startDate != value && value > EndDate)
                {
                    ((TaskDurationModelData)state).startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => ((TaskDurationModelData)state).endDate;
            set
            {
                //ensure that new end date is not less than the start date - if it is, same rule as start date, and just don't set
                if (((TaskDurationModelData)state).endDate != value && value < StartDate)
                {
                    ((TaskDurationModelData)state).endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }
        #endregion

        #region Constructors
        public TaskDurationViewModel() : base(new TaskDurationModelData())
        {

        }
        #endregion
    }
}
