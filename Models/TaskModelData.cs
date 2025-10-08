using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * * Goal of this class is to handle any information tied to the data stored in a generic task
     *      Data like start or end dates -- which only certain tasks should have -- or subitems should be stored 
     *      in a different object
     *      
     *      in this way, I can kind of use the composition pattern to keep this data by itself
     */
    [XmlInclude(typeof(BaseItemModelData)),XmlInclude(typeof(DateDurationModelData)),XmlInclude(typeof(TextModelData))]
    [XmlType("Task")]
    public class TaskModelData : BaseItemModelData
    {
        [XmlElement(ElementName = "Completion", Type = typeof(bool))]
        public bool isCompleted = false;

        [XmlElement(ElementName = "TaskName", Type = typeof(string))]
        public string taskName = "";


        [XmlArrayItem(ElementName = "TaskItem", IsNullable = false,
            Type = typeof(TaskModelData), Namespace = "http://tempuri.org/PlannerProgramSchema"),
            XmlArrayItem(ElementName = "TextItem", IsNullable = false,
            Type = typeof(TextModelData), Namespace = "http://tempuri.org/PlannerProgramSchema"),
            XmlArrayItem(ElementName = "DateDurationItem", IsNullable = false,
            Type = typeof(DateDurationModelData), Namespace = "http://tempuri.org/PlannerProgramSchema"),
            XmlArray(ElementName = "ListItems",
            Namespace = "http://tempuri.org/PlannerProgramSchema", IsNullable = false)]
        public ObservableCollection<BaseItemModelData> subItems;

        [XmlElement(ElementName = "Coords", Type = typeof(Point))]
        public Point coordinates;

        [XmlElement(ElementName = "GUID", Type = typeof(Guid))]
        public Guid uuid;

        public TaskModelData() : base(PlannerItemType.Task) 
        { 
            
        }

        public TaskModelData(TaskModelData? parent) : base(PlannerItemType.Task, parent)
        {
            this.uuid = Guid.NewGuid();
        }

        public TaskModelData(bool isCompleted, string taskName, ObservableCollection<BaseItemModelData> subItems, Point coordinates, Guid uuid, TaskModelData? parent) : base(PlannerItemType.Task, parent)
        {
            this.isCompleted = isCompleted;
            this.taskName = taskName;
            this.subItems = subItems;
            this.coordinates = coordinates;
            this.uuid = uuid;
        }

        public TaskModelData(bool isCompleted, string taskName, ObservableCollection<BaseItemModelData> subItems, Point coordinates, TaskModelData? parent) : base(PlannerItemType.Task, parent)
        {
            this.isCompleted = isCompleted;
            this.taskName = taskName;
            this.subItems = subItems;
            this.coordinates = coordinates;
            this.uuid = Guid.NewGuid();
        }
    }
}
