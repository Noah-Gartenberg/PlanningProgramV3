using PlanningProgramV3.Converters;
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


        //dependency and corresponding property so can be binded to by main window
        public static readonly DependencyProperty CurrPlannerProperty = DependencyProperty.Register(
            "CurrentPlanner", typeof(PlannerViewModel), typeof(PlanDisplayerControl));

        public PlannerViewModel CurrentPlanner
        {
            get => ((PlannerViewModel)GetValue(CurrPlannerProperty));
            set => SetValue(CurrPlannerProperty, value);
        }
        public PlanDisplayerControl()
        {
            InitializeComponent();
        }

        private void AddTaskItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlanner.AddTask.Execute(this);
        }

        public void RefreshList()
        {
            PlannerDisplayer.Items.Refresh();
        }

        private void PlannerDisplayer_MouseMove(object sender, MouseEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            try
            {
                object data = e.Data.GetData(DataFormats.Serializable);

                Point dropPosition = e.GetPosition(PlannerDisplayer);
                Canvas.SetLeft(SelectedObject, dropPosition.X);
                Canvas.SetTop(SelectedObject, dropPosition.Y);
                CurrentPlanner.SetTopTaskPosition(SelectedObject);


                //set the position of the object
                //I know this isn't technically MVVM - I mean I don't think it's a violation per se, but yeah. 
                //couldn't figure out how to do this from the planner
                if (SelectedObject.DataContext is TaskViewModel task)
                {
                    task.X = Convert.ToDouble(PixelToCoordinate.ConvertToCoordinate(dropPosition.X, typeof(int), 1500, null));
                    task.Y = Convert.ToDouble(PixelToCoordinate.ConvertToCoordinate(dropPosition.Y, typeof(int), 1500, null));
                }
                SelectedObject.IsHitTestVisible = true;
                PlannerDisplayer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            

        }

        

        private void PlannerDisplayer_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == PlannerDisplayer)
            {
                object data = e.Data.GetData(DataFormats.Serializable);



                if (data is UIElement element)
                {
                    CurrentPlanner.DeleteItem(data);
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
