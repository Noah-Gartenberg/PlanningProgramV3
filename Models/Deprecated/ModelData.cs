namespace PlanningProgramV3.Models.Deprecated
{
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * Goal of this enum is to define the type of state being saved from the PlannerItems
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
}

namespace PlanningProgramV3.Models.Deprecated
{
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
        public BaseItemModelData(PlannerItemType dataType)
        {
            this.dataType = dataType;
        }
    }

    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * * Goal of this class is to handle any information tied to the data stored in a generic task
     *      Data like start or end dates -- which only certain tasks should have -- or subitems should be stored 
     *      in a different object
     *      
     *      in this way, I can kind of use the composition pattern to keep this data by itself
     */
    public class TaskModelData : BaseItemModelData
    {
        public bool isCompleted = false;
        public string taskName = "";
        public TaskModelData() : base(PlannerItemType.Task) { }
    }

    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * Goal of this class is to handle storing state for Text Items in a planner
     * It needs to store a string - but one that would perhaps be larger
     */
    public class TextModelData : BaseItemModelData
    {
        public string text = "";
        public TextModelData() : base(PlannerItemType.Text) { }
    }

    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * TBH, really not sure how to handle implementing this, so idk what it needs
     *  Technically may only need a string to a filepath, or it could need whatever else?
     *  https://www.codeproject.com/Articles/5360403/How-to-Make-WPF-Behave-like-Windows-when-Dealing-w follow this link
     */
    public class ImageModelData : BaseItemModelData
    {
        public string path;
        public ImageModelData() : base(PlannerItemType.Image) { }

    }

    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * TBH, really unsure how to implement these as well, 
     *  but in theory should only need a path to the file and the UUID Of the file
     */
    public class LinkerModelData : BaseItemModelData
    {
        //path to file from root of Plan
        public string pathToFile = "";

        //the UUID of the item in question -- null if just a plan. 
        public string UUIDofItem = "";
        public LinkerModelData() : base(PlannerItemType.Linker) { }
    }

    /**
     * 8/17/2025
     * This class stores date information for a task that is time sensetive
     * 
     */
    public class TaskDurationModelData : BaseItemModelData
    {
        public string parentPlanFile = "";
        public string parentTaskUUID = "";
        public DateTime startDate;
        public DateTime endDate;

        public TaskDurationModelData() : base(PlannerItemType.Date)
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
        }
    }
    
}
