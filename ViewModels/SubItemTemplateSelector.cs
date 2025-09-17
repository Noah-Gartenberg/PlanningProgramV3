using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PlanningProgramV3.ViewModels.ItemViewModels;

namespace PlanningProgramV3.ViewModels
{
    public class SubItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Task { get; set; }
        public DataTemplate Text { get; set; }
        public DataTemplate Date { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is TaskViewModel)
            {
                return Task;
            }
            else if (item is TextViewModel)
            {
                return Text;
            }
            else if (item is DateDurationViewModel)
            {
                return Date;
            }
                throw new ArgumentException("The item passed into the template selector was not of the current item types supported");
        }
    }
}
