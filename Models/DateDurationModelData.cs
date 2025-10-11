using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

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

        #region Constructors
        /// <summary>
        /// Constructor to call when creating a new date duration sub-item from pre-existing data that hasn't been organized into state yet
        /// </summary>
        /// <param name="parentTaskUUID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="parent"></param>
        public DateDurationModelData(Guid parentTaskUUID, DateTime startDate, DateTime endDate, TaskModelData parent) : base(parent, PlannerItemType.Date)
        {
            this.parentTaskUUID = parentTaskUUID;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        /// <summary>
        /// Constructor to call when creating a new date duration sub-item
        /// </summary>
        /// <param name="parent"></param>
        public DateDurationModelData(TaskModelData parent) : base(parent, PlannerItemType.Date)
        {
            startDate = DateTime.Today;
            endDate = DateTime.Today;
            parentTaskUUID = parent.uuid;
        }

        //need parameterless constructor for serialization/deserialization...
        public DateDurationModelData() : base(PlannerItemType.Date) { }
        #endregion
        public override void PrintData()
        {
            Trace.WriteLine("DateDurationModel: ");
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("StartDate: " + startDate);
            Trace.WriteLine("EndDate: " + endDate);
        }
    }
}
