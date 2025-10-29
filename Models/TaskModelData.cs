using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * * Goal of this class is to handle any information tied to the data stored in a generic task
     *      Data like start or end dates -- which only certain calendarTasks should have -- or subitems should be stored 
     *      in a different object
     *      
     *      in this way, I can kind of use the composition pattern to keep this data by itself
     */
    [XmlType("TaskModelData", Namespace = "http://www.tempuri.org/PlannerModelData.xsd")]
    [XmlInclude(typeof(BaseItemModelData)),XmlInclude(typeof(DateDurationModelData)),XmlInclude(typeof(TextModelData))]
    public class TaskModelData : BaseItemModelData
    {
        [XmlElement("Completion", Type = typeof(bool))]
        public bool isCompleted = false;

        [XmlElement("TaskName", Type = typeof(string))]
        public string taskName = "";


        [XmlArrayItem("TaskItem", IsNullable = true,
            Type = typeof(TaskModelData)),
            XmlArrayItem("BaseItem", IsNullable = true, 
            Type =(typeof(BaseItemModelData))),
            XmlArrayItem("TextItem", IsNullable = true,
            Type = typeof(TextModelData)),
            XmlArrayItem("DateDurationItem", IsNullable = true,
            Type = typeof(DateDurationModelData)),
            XmlArray("ListItems", IsNullable = true)]
        public List<BaseItemModelData> subItems;

        [XmlElement("Coords", Type = typeof(Point))]
        public Point coordinates;

        [XmlElement("GUID", Type = typeof(Guid))]
        public Guid uuid;

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        /// <param name="parent"></param>
        public TaskModelData() : base(PlannerItemType.Task) 
        {
            //okay so what I know is this:
            //the task name, and list of sub items upon serialization are set to null/ empty strings (respectively)
            //the uuid is a different than the task had
            //the coordinates are literally fine for some reason...
            //completion is always false even wehn it shouldn't be
            //for most of these, these are the default values. 
            //when I set the taskName variable in this method for testing purposes, that name was saved into the file
            //supposedly, the xml serializer can not serialize arrays of arrays of arraylists or arrays of list<t>, and yet that wouldn't explain the task name, completion or the uuid...

            //I have done tests to check and see if this constructor is being called on serialization. It is not.
            //I have added break points to ensure the data in the model is being set. it is. 
            //I'm at my wits end and I don't know what's broken...

            subItems = [];
            this.uuid = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor to call when creating a new task from scratch
        /// </summary>
        /// <param name="parent"></param>
        public TaskModelData(TaskModelData parent) : base(parent, PlannerItemType.Task)
        {
            subItems = [];
            this.uuid = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor to call when creating a new task from pre-existing data that hasn't yet been formatted into state
        /// </summary>
        /// <param name="isCompleted"></param>
        /// <param name="taskName"></param>
        /// <param name="subItems"></param>
        /// <param name="coordinates"></param>
        /// <param name="uuid"></param>
        /// <param name="parent"></param>
        public TaskModelData(bool isCompleted, string taskName, List<BaseItemModelData> subItems, Point coordinates, Guid uuid, TaskModelData parent) : base(parent, PlannerItemType.Task)
        {
            this.isCompleted = isCompleted;
            this.taskName = taskName;
            this.subItems = subItems;
            this.coordinates = coordinates;
            this.uuid = uuid;
        }

        public TaskModelData(bool isCompleted, string taskName, List<BaseItemModelData> subItems, Point coordinates, TaskModelData parent) : base(parent, PlannerItemType.Task)
        {
            this.isCompleted = isCompleted;
            this.taskName = taskName;
            this.subItems = subItems;
            this.coordinates = coordinates;
            this.uuid = Guid.NewGuid();
        }
        #endregion
        #region Methods
        
        public override void PrintData()
        {
            Trace.WriteLine("TaskModel: ");
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("Task Name: " + taskName);
            Trace.WriteLine("Task Completion: " + isCompleted);
            Trace.WriteLine("Guid: " + uuid);
            for (int i = 0; i < subItems.Count; i++)
            {
                subItems[i].PrintData();
            }
        }

        public void AddItem(BaseItemModelData item)
        {
            subItems.Add(item);
            PrintData();
        }

        /// <summary>
        /// "Deletes" the item from the list in the model.
        /// </summary>
        /// <param name="indexToDeleteAt"></param>
        public void DeleteSubItem(int indexToDeleteAt)
        {
            subItems.RemoveAt(indexToDeleteAt);
        }
        #endregion
    }
}