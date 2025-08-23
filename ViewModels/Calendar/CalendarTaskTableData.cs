using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PlanningProgramV3.ViewModels.Calendar
{
    [Table("tasks")]
    class CalendarTaskTableData
    {
        public string GUID { get; set; }
    }
}
