using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.ViewModels
{
    /// <summary>
    /// This class will contain the bare essentials of any "save action" functionality - specifically, 
    ///     the save action type, the plan it stems from, and the date duration view model that called the event
    ///         the task can be acquired from the plan
    ///         
    /// this will not handle external changes, like changing the file name or file path
    /// </summary>
    internal class UpdateDatabaseActionData
    {
        //the value that has been updated
        
        public PlannerViewModel planOfCaller;
        public DateDurationViewModel mostUpdatedChanges;
        public DateDurationViewModel? originalVersion;
        public List<ChangeValueType> changedValues = new List<ChangeValueType>();
    }

    internal enum ChangeValueType
    {
        typeString,         //taskName
        typeBool,           //completion
        typeInt,            //coordinates
        typeDateTime        //DateTime
    }
}
