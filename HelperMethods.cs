using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3
{
    public class HelperMethods
    {
        /**
         * Helper method to get the first day of the week in which the date that is the parameter is within.
         * @param: date - DateTime - the date which the user wants to search for the first day of the week that it is part of
         */
        public static DateTime GetFirstDayOfWeekDate(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            int daysFromStartOfWeek = 0;
            //figure out how many days from start of the week the current date is. 
            switch (day)
            {
                case DayOfWeek.Monday:
                    daysFromStartOfWeek = 0;
                    break;
                case DayOfWeek.Tuesday:
                    daysFromStartOfWeek = 1;
                    break;
                case DayOfWeek.Wednesday:
                    daysFromStartOfWeek = 2;
                    break;
                case DayOfWeek.Thursday:
                    daysFromStartOfWeek = 3;
                    break;
                case DayOfWeek.Friday:
                    daysFromStartOfWeek = 4;
                    break;
                case DayOfWeek.Saturday:
                    daysFromStartOfWeek = 5;
                    break;
                case DayOfWeek.Sunday:
                    daysFromStartOfWeek = 6;
                    break;
            }

            return new DateTime(date.Year, date.Month, date.AddDays(-1 * daysFromStartOfWeek).Day);
        }
    }
}
