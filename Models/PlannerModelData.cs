using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [XmlRootAttribute("Plan", Namespace="http://tempuri.org/Plan.xsd", IsNullable = true)]
    [XmlInclude(typeof(VersionData)),XmlInclude(typeof(Task))]
    [XmlType("Plan")]
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


        
        [XmlArrayItem(ElementName = "Task", 
            Type = typeof(TaskModelData), Namespace = "http://tempuri.org/PlannerProgramSchema"),
            XmlArray(ElementName = "PlanTasks", IsNullable = true)]
        public ObservableCollection<TaskModelData> topPlanItems;

        //Current Version
        
        [XmlElement(ElementName = "SoftwareVersion",Type = typeof(VersionData))]
        VersionData planVersion;

        public PlannerModelData() {
            topPlanItems = new ObservableCollection<TaskModelData>();
            fileName = string.Empty;
            planVersion = VersionData.CurrentVersion;
        }

        public PlannerModelData(ObservableCollection<TaskModelData> planItems, string fileName, VersionData Version)
        {
            topPlanItems = planItems;
            this.fileName = fileName;
            this.planVersion = Version;
        }
    }
}
