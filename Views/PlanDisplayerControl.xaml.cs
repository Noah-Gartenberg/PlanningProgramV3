using PlanningProgramV3.ViewModels;
using PlanningProgramV3.ViewModels.ItemViewModels;
using PlanningProgramV3.Views.PlanControls;
using System;
using System.Collections.Generic;
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

namespace PlanningProgramV3.Views
{
    /// <summary>
    /// Interaction logic for PlanDisplayerControl.xaml
    /// </summary>
    public partial class PlanDisplayerControl : UserControl
    {
        public FrameworkElement SelectedObject
        {
            get => (FrameworkElement)GetValue(SelectedObjectProperty);
            set => SetValue(SelectedObjectProperty, value);
        }

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(FrameworkElement), typeof(PlanDisplayerControl));

        public PlannerViewModel CurrPlanner { get; set; }
        public PlanDisplayerControl()
        {
            InitializeComponent();
            CurrPlanner = new PlannerViewModel();
            CurrPlanner.AddHighestTask(new TaskUserControl());
            PlannerDisplayer.ItemsSource = CurrPlanner.HighestTasks;
        }
        

        private void PlannerDisplayer_MouseMove(object sender, MouseEventArgs e)
        {
            //For right now, will just not do selection logic if the selected item is not the highest level parent -- need to figure out how to ensure that selected view model is at top of its stack though
            if ((PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && viewModel.Parent == null && e.LeftButton == MouseButtonState.Pressed)
            {

                DataObject data = new DataObject(DataFormats.Serializable, PlannerDisplayer.SelectedItem);

                DragDrop.DoDragDrop(SelectedObject, data, DragDropEffects.Move);
                if (SelectedObject != null)
                {
                    SelectedObject.IsHitTestVisible = false;
                }

            }
        }

        private void PlannerDisplayer_DragOver(object sender, DragEventArgs e)
        {
            //For right now, will just not do selection logic if the selected item is not the highest level parent
            if ((PlannerDisplayer.SelectedItem is TaskViewModel viewModel) && viewModel.Parent == null)
            {
                Point dropPosition = e.GetPosition(PlannerDisplayer);

                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);

            }

        }

        private void PlannerDisplayer_Drop(object sender, DragEventArgs e)
        {


            object data = e.Data.GetData(DataFormats.Serializable);

            if (data is UIElement element)
            {

                Point dropPosition = e.GetPosition(PlannerDisplayer);
                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);
                CurrPlanner.AddHighestTask(element);
                //command isn't called here, for some reason
                //TO DO MAKE THIS WORK AND FIGURE OUT WHY DROP ISN'T EVER CALLED
                CurrPlanner.SetTopTaskPosition(element);

            }
            SelectedObject.IsHitTestVisible = true;
            PlannerDisplayer.SelectedIndex = -1;

        }

        private void PlannerDisplayer_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == PlannerDisplayer)
            {
                object data = e.Data.GetData(DataFormats.Serializable);



                if (data is UIElement element)
                {
                    CurrPlanner.RemoveTopTask(data);
                }
            }


        }

        private void PlannerDisplayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //Shout out people in comments explaining how I can fix this so that selected object doesn't throw an error
            //https://stackoverflow.com/questions/610343/wpf-listbox-getting-uielement-instead-of-of-selecteditem
            //in theory at least if I can get it working

            //for right now, only worrying about one item
            if (SelectedObject != null)
            {
                SelectedObject.IsHitTestVisible = true;
            }
            if (PlannerDisplayer.SelectedIndex != -1)
            {

                SelectedObject = PlannerDisplayer.ItemContainerGenerator.ContainerFromItem(PlannerDisplayer.SelectedItem) as FrameworkElement;
            }
            else
                SelectedObject = null;
        }
    }
}
