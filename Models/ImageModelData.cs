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
     * TBH, really not sure how to handle implementing this, so idk what it needs
     *  Technically may only need a string to a filepath, or it could need whatever else?
     *  https://www.codeproject.com/Articles/5360403/How-to-Make-WPF-Behave-like-Windows-when-Dealing-w follow this link
     */
    [Serializable()]
    public class ImageModelData : BaseItemModelData
    {
        public string path;
        public ImageModelData() : base(PlannerItemType.Image) { }

    }
}
