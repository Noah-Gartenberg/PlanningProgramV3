using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PlanningProgramV3.ViewModels.Calendar
{
    
    /**
     * This class will actually be what is stored in calendarTasks lists. 
     * Need to store the data that would be necessary to access the filepath, filename, coordinates, etc... that is largely just the primary keys, and the dates. 
     *      not storing the filepath, filename, coordinates, or other such data, as those should not be necessary for displaying to the calendar
     *          //they only become necessary if the user clicks on the data. 
     */
    public class CalendarTaskData : ICalendarTask
    {
        public Guid TaskGUID { get; private set; }
        public string TaskName { get; set; }
        public bool Completion { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        
        public CalendarTaskData(object[] values)
        {
            for(int i = 0; i < values.Length; i++)
            {
                //values in order is: 
                /* 
                 * 0. TaskGUID, 
                 * 1. TaskName, 
                 * 2. TaskCompletion, 
                 * 3. StartDate, 
                 * 4. EndDate
                 */

                switch (i)
                {
                    case 0:
#pragma warning disable CS8604 // If is null, is much bigger problem
                        TaskGUID = new Guid(values[i].ToString());
#pragma warning restore CS8604 //  If is null, is much bigger problem
                        break;
                    case 1:
#pragma warning disable CS8604 // If is null, is much bigger problem
                        TaskName = values[i].ToString();
#pragma warning restore CS8604 //  If is null, is much bigger problem
                        break;
                    case 2:
#pragma warning disable CS8604 // If is null, is much bigger problem
                        //unable to cast int 64 to bool, so just check if value is > 0
                        if ((long)values[i] > 0)
                            Completion = true;
                        else
                            Completion = false;
#pragma warning restore CS8604 //  If is null, is much bigger problem
                        break;
                    case 3:
#pragma warning disable CS8604 // If is null, is much bigger problem
                        //unsure if conversion is allowed
                        DateStart = DateTime.Parse(values[i].ToString());
                        //DateStart = (DateTime)values[i];
#pragma warning restore CS8604 //  If is null, is much bigger problem
                        break;
                    case 4:
#pragma warning disable CS8604 // If is null, is much bigger problem
                        DateEnd = DateTime.Parse(values[i].ToString());
#pragma warning restore CS8604 //  If is null, is much bigger problem
                        break;
                    default:
                        break;
                }
                //throw new NotImplementedException("Have not implemented array input constructor for CalendarTaskData");   
            }
        }

        //value is > 4 values, so pushing one value onto the stack - how fix?

        public CalendarTaskData(string tableGUID, string taskName, bool completion, DateTime? dateStart, DateTime? dateEnd)
        {
            TaskGUID = new Guid(tableGUID);
            TaskName = taskName;
            Completion = completion;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
