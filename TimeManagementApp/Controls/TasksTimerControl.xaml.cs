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
using System.Text.RegularExpressions;
using System.Threading;
using TimeManagementApp.ViewModels;
namespace TimeManagementApp.Controls
{
    /// <summary>
    /// Interaction logic for TasksTimerControl.xaml
    /// </summary>
    public partial class TasksTimerControl : UserControl
    {
        #region DependencyProperties

        //Tasks
        public static readonly DependencyProperty TasksProperty = DependencyProperty.Register(
            "Tasks",
            typeof(IList<ViewModels.TaskViewModel>),
            typeof(TasksTimerControl),
            new PropertyMetadata(null, OnTasksChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public IList<ViewModels.TaskViewModel> Tasks
        {
            get { return (IList<ViewModels.TaskViewModel>)this.GetValue(TasksProperty); }
            set { this.SetValue(TasksProperty, value); }
        }

        //CurrentTask
        public static readonly DependencyProperty CurrentTaskProperty = DependencyProperty.Register(
            "CurrentTask",
            typeof(ViewModels.TaskViewModel),
            typeof(TasksTimerControl),
            new PropertyMetadata(null, OnCurrentTaskChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public ViewModels.TaskViewModel CurrentTask
        {
            get { return (ViewModels.TaskViewModel)this.GetValue(CurrentTaskProperty); }
            set { this.SetValue(CurrentTaskProperty, value); }
        }

        //ElapsedTime
        public static readonly DependencyProperty ElapsedTimeProperty = DependencyProperty.Register(
            "ElapsedTime",
            typeof(int),
            typeof(TasksTimerControl),
            new PropertyMetadata(0, OnElapsedTimeChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public int ElapsedTime
        {
            get { return (int)this.GetValue(ElapsedTimeProperty); }
            set 
            { 
                this.SetValue(ElapsedTimeProperty, value); 
            }
        }

        //TotalTime
        public static readonly DependencyProperty TotalTimeProperty = DependencyProperty.Register(
            "TotalTime",
            typeof(ViewModels.TimeInfoViewModel),
            typeof(TasksTimerControl),
            new PropertyMetadata(null, OnTotalTimeChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public ViewModels.TimeInfoViewModel TotalTime
        {
            get { return (ViewModels.TimeInfoViewModel)this.GetValue(TotalTimeProperty); }
            set
            {
                this.SetValue(TotalTimeProperty, value);
            }
        }

        //AutoSwitchToNextTask
        public static readonly DependencyProperty AutoSwitchToNextTaskProperty = DependencyProperty.Register(
            "AutoSwitchToNextTask",
            typeof(bool),
            typeof(TasksTimerControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAutoSwitchToNextTaskChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public bool AutoSwitchToNextTask
        {
            get { return (bool)this.GetValue(AutoSwitchToNextTaskProperty); }
            set
            {
                this.SetValue(AutoSwitchToNextTaskProperty, value);
            }
        }

        #endregion

        Dictionary<TaskViewModel, TaskSectionControl> taskSections = new Dictionary<TaskViewModel, TaskSectionControl>();

        public TasksTimerControl()
        {
            InitializeComponent();
        }

        #region Methods

        private void ClearExistingTaskSections()
        {
            foreach (var taskSection in taskSections)
                this.RectangleGrid.Children.Remove(taskSection.Value);

            taskSections.Clear();
        }

        public void UpdateTasks(IList<ViewModels.TaskViewModel> oldValue, IList<ViewModels.TaskViewModel> newValue)
        {
            InitializeRectangles();
        }

        public void InitializeRectangles()
        {
            this.RectangleGrid.ColumnDefinitions.Clear();
            this.RectangleGrid.Children.Clear();

            ClearExistingTaskSections();
            if(this.TotalTime == null || !this.TotalTime.IsPositiveTime || Tasks == null)
                return;

            int i = 0;
            foreach(var task in Tasks)
            {
                TaskSectionControl section = new TaskSectionControl(task);
                section.WidthPercentage = (double)
                    (
                        ((double)task.OriginalTime.TotalSeconds) / ((double)this.TotalTime.TotalSeconds)
                    );
                section.Width = this.ActualWidth * section.WidthPercentage;

                //double width = this.ActualWidth *
                //    (double)
                //    (
                //        ((double)task.OriginalTime.TotalSeconds) / ((double)this.TotalTime.TotalSeconds)
                //    );

                //New column definition
                var def = new ColumnDefinition();// { Width = new GridLength(width) };
                this.RectangleGrid.ColumnDefinitions.Add(def);
                def.Width = new GridLength(section.WidthPercentage, GridUnitType.Star);


                //Rectangle
                //Rectangle rec = new Rectangle();
                //rec.Fill = task.ColorInfo.Brush;
                //rec.Width = width;
                //rec.Height = this.ActualHeight;
                section.Height = this.ActualHeight;
                this.RectangleGrid.Children.Add(section);
                Grid.SetColumn(section, i);
                ++i;
                taskSections[task] = section;
            }

            UpdateCurrentTask();
        }

        public void UpdateIndicator()
        {
            if(this.TotalTime == null || !this.TotalTime.IsPositiveTime)
                return;
            
            double x = (((double)this.ElapsedTime) * this.ActualWidth) / ((double)this.TotalTime.TotalSeconds);
            Canvas.SetLeft(Indicator, x);

            //LinearGradientBrush progressOpacityMask = this.RectangleGrid.OpacityMask as LinearGradientBrush;
            //if (progressOpacityMask == null)
            //    return;

            //double newCenterOffset = ((double)this.ElapsedTime) / ((double)this.TotalTime.TotalSeconds);
            //progressOpacityMask.GradientStops[1].Offset = newCenterOffset;
            //progressOpacityMask.GradientStops[2].Offset = newCenterOffset;
        }

        public void UpdateCurrentTask()
        {
            if (this.Tasks == null || this.Tasks.Count() == 0)
                return;

            if (CurrentTask == null)
                CurrentTask = this.Tasks[0];

            int currentIndex = this.Tasks.IndexOf(CurrentTask);
            if (currentIndex == (Tasks.Count - 1))
                return;

            TaskSectionControl c = null;
            if (!taskSections.TryGetValue(CurrentTask, out c))
                return;

            if (CurrentTask.TimeLeft.IsOutOfTime)
            {
                if(AutoSwitchToNextTask)
                {
                    int currentTaskEnds = this.Tasks.Take(currentIndex + 1).Sum(x => x.OriginalTime.TotalSeconds);
                    if (ElapsedTime < currentTaskEnds)
                        return;

                    CurrentTask = Tasks[currentIndex + 1];
                }
                else
                {
                    //Adjust widths here.
                     
                }
            }
        }

        public void TotalTimeChanged()
        {
            InitializeRectangles();
        }

        #endregion

        #region Events
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InitializeRectangles();
            UpdateIndicator();
        }
        #endregion

        #region Static Events
        private static void OnTasksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TasksTimerControl c = (TasksTimerControl)d;
            if (c != null)
                c.UpdateTasks((IList<TaskViewModel>)e.OldValue, (IList<ViewModels.TaskViewModel>)e.NewValue);
        }

        private static void OnCurrentTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnElapsedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TasksTimerControl c = (TasksTimerControl)d;
            if (c != null)
            {
                c.UpdateIndicator();
                c.UpdateCurrentTask();
            }
        }

        private static void OnTotalTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TasksTimerControl c = (TasksTimerControl)d;
            if (c != null)
                c.TotalTimeChanged();
        }

        private static void OnAutoSwitchToNextTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TasksTimerControl c = (TasksTimerControl)d;
            if (c != null)
            {
                c.AutoSwitchToNextTask = (bool)e.NewValue;
            }
        }
        
        #endregion
    }
}
