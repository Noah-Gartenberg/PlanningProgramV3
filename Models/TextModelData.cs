using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 7/9/2025
     * Goal of this class is to handle storing state for Text Items in a planner
     * It needs to store a string - but one that would perhaps be larger
     */
    [XmlType("Text")]
    public class TextModelData : BaseItemModelData
    {
        [XmlElement(ElementName = "TextItem", Namespace = "http://tempuri.org/PlannerProgramSchema")]
        public string text = "";
        public TextModelData() : base(PlannerItemType.Text) { }

        public TextModelData(string text) : base(PlannerItemType.Text)
        {
            this.text = text;
        }
    }
}
