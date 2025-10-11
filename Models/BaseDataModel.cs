using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 8/19/2025 - added Duration
     * Goal of this enum is to define the type of state being saved from the PlannerItems
     *  
     */
    public enum PlannerItemType
    {
        Generic,
        Task,
        Text,
        Image,
        Linker,
        Date
    }
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * Goal of this class is to enable storing information for each different type of PlannerItem within the same XML file without needing custom methods for each one and to reinvent the wheel repeatedly
     *  so basically to make code more reuseable is what I'm getting at. 
     *  Also to separate the state from potential sub-items so that way they are not required.
     */
    public abstract class BaseItemModelData
    {
        public PlannerItemType? dataType;

        public TaskModelData parent;

        public BaseItemModelData(PlannerItemType dataType)
        {
            parent = null;
            this.dataType = dataType;
        }

        public BaseItemModelData(PlannerItemType dataType, TaskModelData? parent)
        {
            this.dataType = dataType;
            this.parent = parent;
        }

        public abstract void PrintData();
    }
}
