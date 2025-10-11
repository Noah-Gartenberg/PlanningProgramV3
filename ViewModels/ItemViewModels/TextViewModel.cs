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
     * Last Updated: 10/10/2025
     * This will contain the view model for the Text Item
     * 
     * Refactored constructors
     */
    public class TextViewModel : PlannerItemViewModel
    {

        #region Properties
        //get reference to State data
        public new TextModelData State
        {
            get => (TextModelData)state;

        }

        public string Text
        {
            //If there is an error where nothing updates when this is set or unset, look ehre
            //this may not be changing the data at the reference...
            get => State.text;
            set
            {
                if (!value.Equals(State.text))
                {
                    State.text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for creating a TextViewModel from scratch, with no-pre-existing state
        /// </summary>
        /// <param name="parent">the parent of the object</param>
        public TextViewModel(TaskViewModel parent) : base(parent,PlannerItemType.Text) { }

        /// <summary>
        /// Constructor for creating a TextViewModel from some pre-existing state
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="state"></param>
        public TextViewModel(TaskViewModel parent, BaseItemModelData state) : base(parent, state, PlannerItemType.Text) { }

#warning WAIT A MINUTE! IN ORDER FOR THE CONTROLS TO WORK, THEY NEED AN ACCESSIBLE CONSTRUCTOR - PROBABLY A DEFAULT CONSTRUCTOR!!!! WHICH MEANS THE DATA IN THE CONTROLS IS BEING SET TO DEFAULT???? COULD THAT BE THE ISSUE?
        public TextViewModel() : base(PlannerItemType.Text) { }

        #endregion

        #region Methods
        /// <summary>
        /// Prints the data for the view model to the output. 
        /// </summary>
        public override void PrintData()
        {
            Trace.WriteLine("Parent: " + parent);
            Trace.WriteLine("Text: " +  Text);
        }
        #endregion

    }
}
