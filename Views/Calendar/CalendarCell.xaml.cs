using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlanningProgramV3.Views.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarCell.xaml
    /// </summary>
    public partial class CalendarCell : UserControl
    {
        public DateTime CellDate { get; set; } 

        public CalendarCell()
        {
            InitializeComponent();
            DataContext = this;
        }

        //Should open plan for the day itself
        private void CellDateTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException("Thsi method should open the plan for the actual day of");
        }
    }
}
