using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanningProgramV3.Views.Calendar;

namespace PlanningProgramV3.ViewModels.Calendar
{
    public interface ICalendarDisplay
    {
        public void HighlightTask(CalendarTaskView eventToSelect);
        //public void OpenPlanReferenced(CalendarTaskView eventToSelect);
        //public void OpenPlanForDay();
    }
}
