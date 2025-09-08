using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * TBH, really unsure how to implement these as well, 
     *  but in theory should only need a path to the file and the UUID Of the file
     */
    [Serializable()]
    public class LinkerModelData : BaseItemModelData
    {
        //path to file from root of Plan
        public string pathToFile = "";

        //the UUID of the item in question -- null if just a plan. 
        public string UUIDofItem = "";
        public LinkerModelData() : base(PlannerItemType.Linker) { }
    }
}
