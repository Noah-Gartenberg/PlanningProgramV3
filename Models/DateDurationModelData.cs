using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.Models
{
    /**
     * 8/17/2025
     * This class stores date information for a task that is time sensetive
     * 
     */
    [Serializable()]
    public class DateDurationModelData : BaseItemModelData
    {
        public string parentPlanFile = "";
        public string parentTaskUUID = "";
        public DateTime startDate;
        public DateTime endDate;

        public DateDurationModelData() : base(PlannerItemType.Date)
        {
            startDate = DateTime.Today;
            endDate = DateTime.Today;
        }
    }
}
