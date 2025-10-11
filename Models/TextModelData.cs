using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace PlanningProgramV3.Models
{
    /**
     * Noah Gartenberg
     * Last Updated: 10/10/2025
     * Goal of this class is to handle storing state for Text Items in a planner
     * It needs to store a string - but one that would perhaps be larger
     */
    [XmlInclude(typeof(BaseItemModelData))]
    public class TextModelData : BaseItemModelData
    {
        [XmlElement(ElementName = "TextItem")]
        public string text;

        #region Constructors
        //probably the only constructor that will actually see any use
        public TextModelData(TaskModelData parent) : base(parent, PlannerItemType.Text) 
        {
            text = ""; 
        }
        public TextModelData(string text, TaskModelData parent) : base(parent, PlannerItemType.Text)
        {
            this.text = text;
        }
        public TextModelData() : this("") {  }

        //THIS CONSTRUCTOR WILL NEVER BE USED SINCE A TEXT OBJECT CAN NOT BE WITHOUT A PARENT
        public TextModelData(string text) : base(PlannerItemType.Text)
        {
            this.text = text;
        }
        #endregion

        public override void PrintData()
        {
            Trace.WriteLine("Text: ");
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("Text: " + text);
        }
    }
}
