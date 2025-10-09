using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlanningProgramV3.Models
{
    /**
     * 8/17/2025
     * This class stores date information for a task that is time sensetive
     * 
     */
    [XmlInclude(typeof(BaseItemModelData))]
    public class DateDurationModelData : BaseItemModelData
    {
        //wait, for one of these, the parent plan would need to be in the same file, so this isn't necessary
        [XmlIgnore]
        public string parentPlanFile = "";

        [XmlElement(ElementName = "ParentTaskGUID", Type = typeof(Guid), IsNullable = false)]
        public Guid parentTaskUUID;

        [XmlElement(ElementName = "StartDate", Type = typeof(DateTime), IsNullable = false)]
        public DateTime startDate;

        [XmlElement(ElementName = "EndDate", Type = typeof(DateTime), IsNullable = false)]
        public DateTime endDate;

        public DateDurationModelData(TaskModelData? parent) : base(PlannerItemType.Date,parent)
        {
            startDate = DateTime.Today;
            endDate = DateTime.Today;
            parentTaskUUID = parent.uuid;
        }

        //need parameterless constructor for serialization...
        public DateDurationModelData() : base(PlannerItemType.Date) 
        {
            startDate = DateTime.Today;
            endDate = DateTime.Today;
        }

        public DateDurationModelData(Guid parentTaskUUID, DateTime startDate, DateTime endDate) : base(PlannerItemType.Date)
        {
            this.parentTaskUUID = parentTaskUUID;
            this.startDate = startDate;
            this.endDate = endDate;
        }

    }
}
