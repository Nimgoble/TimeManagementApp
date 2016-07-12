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
using System.ComponentModel;
using TimeManagementApp.ViewModels;
namespace TimeManagementApp.Controls
{
    /// <summary>
    /// Interaction logic for TaskSectionControl.xaml
    /// </summary>
    public partial class TaskSectionControl : UserControl
    {
        public TaskSectionControl()
        {
            InitializeComponent();
            DoInitializationStuff();
        }

        public TaskSectionControl(TaskViewModel vm)
        {
            InitializeComponent();
            this.Task = vm;
            DoInitializationStuff();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            this.UpdateWidth();
            base.OnVisualParentChanged(oldParent);
        }

        private void DoInitializationStuff()
        {
            if (this.Parent != null)
            {
                FrameworkElement c = (FrameworkElement)this.Parent;
                if (c != null)
                {
                    c.SizeChanged += Parent_SizeChanged;
                }
            }
        }

        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWidth();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UpdateWidth();
            return new Size(this.Width, this.ActualHeight);
            //return base.MeasureOverride(constraint);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            UpdateOverlay();
            base.OnRender(drawingContext);
        }

        #region DependencyProperties
        //Task
        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
            "Task",
            typeof(ViewModels.TaskViewModel),
            typeof(TaskSectionControl),
            new PropertyMetadata(null, OnTaskChanged));

        [Category("TaskSectionControl")]
        [Browsable(true)]
        public ViewModels.TaskViewModel Task
        {
            get { return (ViewModels.TaskViewModel)this.GetValue(TaskProperty); }
            set { this.SetValue(TaskProperty, value); }
        }

        //WidthPercentage
        public static readonly DependencyProperty WidthPercentageProperty = DependencyProperty.Register(
            "WidthPercentage",
            typeof(double),
            typeof(TaskSectionControl),
            new PropertyMetadata(0.0, OnWidthPercentageChanged));

        [Category("TaskSectionControl")]
        [Browsable(true)]
        public double WidthPercentage
        {
            get { return (double)this.GetValue(WidthPercentageProperty); }
            set { this.SetValue(WidthPercentageProperty, value); }
        }
        #endregion

        #region Static Event
        private static void OnTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (TaskSectionControl)d;
            if (c != null)
                c.OnTaskChanged((TaskViewModel)e.OldValue, (TaskViewModel)e.NewValue);
        }

        private static void OnWidthPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (TaskSectionControl)d;
            if (c != null)
                c.UpdateWidth();
        }
        #endregion

        #region Methods
        public void UpdateWidth()
        {
            this.Visibility = (WidthPercentage <= 0) ? Visibility.Collapsed : Visibility.Visible;
            FrameworkElement c = (FrameworkElement)this.Parent;
            if (c != null && WidthPercentage > 0)
            {
                this.Width = c.ActualWidth * WidthPercentage;
            }
        }
        public void UpdateOverlay()
        {
            if (this.Task == null)
                return;

            if(this.Task.TimeLeft.IsPositiveTime || this.Task.TimeLeft.IsOutOfTime)
            {
                LinearGradientBrush progressOpacityMask = this.RectangleGrid.OpacityMask as LinearGradientBrush;
                if (progressOpacityMask == null)
                    return;

                if (progressOpacityMask.GradientStops[1].Offset == 1.0 && progressOpacityMask.GradientStops[2].Offset == 1.0)
                    return;

                double newCenterOffset = ((double)Task.ElapsedTime.TotalSeconds) / ((double)Task.OriginalTime.TotalSeconds);
                newCenterOffset = Math.Min(1.0f, newCenterOffset);
                progressOpacityMask.GradientStops[1].Offset = newCenterOffset;
                progressOpacityMask.GradientStops[2].Offset = newCenterOffset;
            }
            //If it's not positive time, then the overlay should already be gone.
        }

        public void OnTaskChanged(TaskViewModel oldValue, TaskViewModel newValue)
        {
            if (oldValue != null)
                UnbindToTaskPropertyChanges(oldValue);
            if (newValue != null)
                BindToTaskPropertyChanges(newValue);
        }

        private void BindToTaskPropertyChanges(TaskViewModel task)
        {
            task.ElapsedTime.PropertyChanged += ElapsedTime_PropertyChanged;
            //task.OriginalTime.PropertyChanged += OriginalTime_PropertyChanged;
        }

        private void UnbindToTaskPropertyChanges(TaskViewModel task)
        {
            task.ElapsedTime.PropertyChanged -= ElapsedTime_PropertyChanged;
            //task.OriginalTime.PropertyChanged -= OriginalTime_PropertyChanged;
        }

        private void ElapsedTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateOverlay();
        }
        #endregion
    }
}
