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

namespace TimeManagementApp.Controls
{
    /// <summary>
    /// Interaction logic for TasksTimerControl.xaml
    /// </summary>
    public partial class TasksTimerControl : UserControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty TasksProperty = DependencyProperty.Register(
            "Tasks",
            typeof(IEnumerable<ViewModels.TaskViewModel>),
            typeof(TimeControl),
            new PropertyMetadata(null, OnTasksChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public IEnumerable<ViewModels.TaskViewModel> Tasks
        {
            get { return (IEnumerable<ViewModels.TaskViewModel>)this.GetValue(TasksProperty); }
            set { this.SetValue(TasksProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskProperty = DependencyProperty.Register(
            "CurrentTask",
            typeof(ViewModels.TaskViewModel),
            typeof(TimeControl),
            new PropertyMetadata(null, OnCurrentTaskChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public ViewModels.TaskViewModel Tasks
        {
            get { return (ViewModels.TaskViewModel)this.GetValue(TasksProperty); }
            set { this.SetValue(TasksProperty, value); }
        }

        public static readonly DependencyProperty ElapsedTimeProperty = DependencyProperty.Register(
            "ElapsedTime",
            typeof(int),
            typeof(TimeControl),
            new PropertyMetadata(null, OnElapsedTimeChanged));

        [Category("TasksTimerControl")]
        [Browsable(true)]
        public int ElapsedTime
        {
            get { return (int)this.GetValue(ElapsedTimeProperty); }
            set { this.SetValue(ElapsedTimeProperty, value); }
        }

        #endregion

        public TasksTimerControl()
        {
            InitializeComponent();
        }

        #region Methods

        #endregion

        #region Static Events
        private static void OnTasksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TasksTimerControl c = (TasksTimerControl)d;
            if(c != null)
            {

            }
        }

        private static void OnCurrentTaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnElapsedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
