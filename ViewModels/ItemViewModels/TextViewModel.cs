using PlanningProgramV3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.ViewModels.ItemViewModels
{
    /** 
     * Noah Gartenberg
     * Last Updated: 7/11/2025
     * This will contain the view model for the Text Item
     */
    public class TextViewModel : PlannerItemViewModel
    {
        #region Fields

        #endregion

        #region Properties
        public string Text
        {
            //If there is an error where nothing updates when this is set or unset, look ehre
            //this may not be changing the data at the reference...
            get => ((TextModelData)state).text;
            set
            {
                if (!value.Equals((((TextModelData)state).text)))
                {
                    ((TextModelData)state).text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }
        #endregion

        #region Constructors
        //constructor for creating a new model from pre-existing model data
        public TextViewModel(ref TaskViewModel parent) : base(in parent.State) { }
        public TextViewModel() : base(in new TextModelData()) { }

        public override void PrintData()
        {
            Trace.WriteLine("Parent: " + Parent);
            Trace.WriteLine("Text: " +  Text);
        }
        #endregion

    }
}
