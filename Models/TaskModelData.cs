using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    [Serializable()]
    public class TaskModelData : BaseItemModelData
    {

        public bool isCompleted = false;
        public string taskName = "";
        public ObservableCollection<BaseItemModelData> subItems;

        public Point coordinates;
        public string uuid;

        public TaskModelData() : base(PlannerItemType.Task) { }
        public TaskModelData(bool isCompleted, string taskName, ObservableCollection<BaseItemModelData> subItems, Point coordinates, string uuid, BaseItemModelData parent) : base(PlannerItemType.Task, parent)
        {
            this.isCompleted = isCompleted;
            this.taskName = taskName;
            this.subItems = subItems;
            this.coordinates = coordinates;
            this.uuid = uuid;
        }
    }
}
