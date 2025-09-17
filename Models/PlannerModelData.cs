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
     * 8/20/2025
     * Noah Gartenberg
     * This class will contain the data necessary for storing the data of one plan
     */
    [Serializable()]
    public class PlannerModelData
    {
        //public Point CameraCoords;

        //public int PlanWidth;
        //public int PlanHeight;

        public string fileName;
        //Right now only needs to contain the top of the list of plan items, as the rest will contain their children
            //need to use taskviewmodel instead of taskmodeldata because don't have time/energy to refactor TO DO, REFACTOR
        public ObservableCollection<TaskModelData> topPlanItems;

        public PlannerModelData() {
            topPlanItems = new ObservableCollection<TaskModelData>();
            fileName = string.Empty;
        }
    }
}
