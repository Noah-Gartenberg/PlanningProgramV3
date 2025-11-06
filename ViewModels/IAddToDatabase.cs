using PlanningProgramV3.ViewModels.ItemViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningProgramV3.ViewModels
{
    internal interface IAddToDatabase
    {
        public void TryAddToDatabase(Action<TaskViewModel, DateDurationViewModel> AddToDatabaseCallback);
    }
}
