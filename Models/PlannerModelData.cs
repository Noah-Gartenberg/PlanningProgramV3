using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PlanningProgramV3.Models
{
    /**
     * 8/20/2025
     * Noah Gartenberg
     * This class will contain the data necessary for storing the data of one plan
     */
    [XmlRoot(Namespace = "http://www.tempuri.org/PlannerModelData.xsd")]
    [XmlInclude(typeof(VersionData)),XmlInclude(typeof(Task)), XmlInclude(typeof(BaseItemModelData)), XmlInclude(typeof(DateDurationModelData)), XmlInclude(typeof(TextModelData))]
    public class PlannerModelData
    {
        //public Point CameraCoords;

        //public int PlanWidth;
        //public int PlanHeight;

        [XmlElementAttribute(ElementName = "PlanName", Type = typeof(string))]
        public string fileName;
        //Right now only needs to contain the top of the list of plan items, as the rest will contain their children
        //need to use taskviewmodel instead of taskmodeldata because don't have time/energy to refactor TO DO, REFACTOR
        //THIS HAS BEEN DONE, BUT MAYBE SHOULD BE REPLACED WITH A LIST?



        //the highest level calendarTasks in plan
        //in this way, this will be more of a linked list than anything else
        [XmlArrayItem(ElementName = "Task", 
            Type = typeof(TaskModelData)),
            XmlArray(ElementName = "PlanTasks", IsNullable = true)]
        public List<TaskModelData> planTasks;

        //Current Version
        
        [XmlElement(ElementName = "SoftwareVersion",Type = typeof(VersionData))]
        VersionData planVersion;

        #region Constructors
        //default constructor
        public PlannerModelData() {
            planTasks = new List<TaskModelData>();
            fileName = string.Empty;
            planVersion = VersionData.CurrentVersion;
        }

        /// <summary>
        /// Constructor with items that need to be inputted
        /// </summary>
        /// <param name="planItems"></param>
        /// <param name="fileName"></param>
        /// <param name="Version"></param>
        public PlannerModelData(ref List<TaskModelData> planItems, string fileName, VersionData Version)
        {
            planTasks = planItems;
            this.fileName = fileName;
            this.planVersion = Version;
        }
        #endregion


        public void PrintPlannerDataMethod()
        {
            Trace.WriteLine("Planner Data: ");
            Trace.WriteLine("FileName: " + fileName);
            for (int i = 0; i < planTasks.Count; i++) {
                planTasks[i].PrintData();
            }
        }

        /// <summary>
        /// Method to add calendarTasks - done this way to ensure that I'm not directly accessing the list from the view model, for rigth now
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(TaskModelData task)
        {
            planTasks.Add(task);
        }
    }
}
