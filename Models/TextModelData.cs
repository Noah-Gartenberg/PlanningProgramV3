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
     * Goal of this class is to handle storing state for Text Items in a planner
     * It needs to store a string - but one that would perhaps be larger
     */
    [Serializable()]
    public class TextModelData : BaseItemModelData
    {
        public string text = "";
        public TextModelData() : base(PlannerItemType.Text) { }
    }
}
