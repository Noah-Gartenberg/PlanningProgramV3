using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlanningProgramV3.ViewModels.Calendar
{
    /**
     * This interface represents most of the things that the Calendar tasks will need (sans a reference to the UUID or Handle
     *  and the filepath to the plans in question)
     */
    public interface ICalendarTask
    {
        string TaskName { get; set; }
        bool Completion { get; set; }
        DateTime? DateStart { get; set; }
        DateTime? DateEnd { get; set; }
    }
}
