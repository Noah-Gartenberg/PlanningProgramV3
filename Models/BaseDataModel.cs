using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
     * Last Updated: 10/10/2025
     * Goal of this class is to enable storing information for each different type of PlannerItem within the same XML file without needing custom methods for each one and to reinvent the wheel repeatedly
     *  so basically to make code more reuseable is what I'm getting at. 
     *  Also to separate the state from potential sub-items so that way they are not required.
     *  
     *  Trying to figure out how to make saving work - wondering if issue may be that I'm not passing by value into things?
     */
    public abstract class BaseItemModelData
    {
        [XmlIgnore]
        public PlannerItemType dataType;
        [XmlIgnore]

        public TaskModelData? parent;

        //xml serialization needs a default constructor - perhaps this will fix some of my serialization woes?
        public BaseItemModelData() : this(PlannerItemType.Generic) { }

        public BaseItemModelData(PlannerItemType dataType)
        {
            parent = null;
            this.dataType = dataType;
        }

        //if passing in parent, it won't be null
        public BaseItemModelData(TaskModelData parent, PlannerItemType dataType)
        {
            this.dataType = dataType;
            this.parent = parent;
        }

        /// <summary>
        /// Made a method to print data to see if models and view models were out of sink with each other
        /// </summary>
        public abstract void PrintData();
    }
}
