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
     * This class will actually be what is stored in calendar calendarTasks -- right now needs only hold the int ID in the table for the task
     *  as all other data can be fetched from the SQL table?    
     *          NO! NEEDS TO HOLD ALL ICALENDARTASK DATA AND THE INT ID
     *          
     *          CORRECTION FURTHER: MUST HOLD MOST DATA, CAN AVOID HOLDING THE GUID AND FILE NAME SINCE THOSE AREN'T IMMEDIATELY NECESSARY
     *              
     *  
     *  As of 8/26/2025, I need to implement the interface and change the arrays in monthly and weekly calendar to have the ID
     *          from there, when selected, they'll need to access the database
     */
    public class CalendarTaskData : ICalendarTask
    {
        //returns if the table had a bool, since bool is smaller than a string, and since this data is only necessary if users click on the calendarTasks
        public string TableGUID { get; set; } 
        public bool HadGUID { get; set; }
        public bool HadFileName { get; set; }
        public string TaskName { get; set; }
        public bool Completion { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        

        public CalendarTaskData(string tableGUID, bool hadGUID, bool hadFileName, string taskName, bool completion, DateTime? dateStart, DateTime? dateEnd)
        {
            TableGUID = tableGUID;
            HadGUID = hadGUID;
            HadFileName = hadFileName;
            TaskName = taskName;
            Completion = completion;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
