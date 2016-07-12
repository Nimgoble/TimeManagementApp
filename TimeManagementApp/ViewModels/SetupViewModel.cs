using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using TimeManagementApp.Models;
using TimeManagementApp.Extensions;
namespace TimeManagementApp.ViewModels
{
    public class SetupViewModel : Screen/*, IDropTarget*/
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        private readonly IWindowManager windowManager;
        private List<TaskViewModel> testTasks = new List<TaskViewModel>()
        {
            new TaskViewModel("Task 1", new ColorInfo("Aqua", System.Windows.Media.Colors.Aqua), new TimeInfoViewModel(0, 0, 10)),
            new TaskViewModel("Task 2", new ColorInfo("BlueViolet", System.Windows.Media.Colors.BlueViolet), new TimeInfoViewModel(0, 0, 10)),
            new TaskViewModel("Task 3", new ColorInfo("Chartreuse", System.Windows.Media.Colors.Chartreuse), new TimeInfoViewModel(0, 0, 10))
        };

        public SetupViewModel(Conductor<Screen>.Collection.OneActive parent, IWindowManager windowManager)
        {
            this.parent = parent;
            this.windowManager = windowManager;
            DoSetup();

            //foreach (var task in testTasks)
            //    tasks.Add(task);
        }

        public SetupViewModel(Conductor<Screen>.Collection.OneActive parent, TimeInfoViewModel totalTime, List<TaskViewModel> tasks, IWindowManager windowManager)
        {
            this.parent = parent;
            this.windowManager = windowManager;
            this.totalTimeInfo.TotalSeconds = totalTime.TotalSeconds;
            foreach (var task in tasks)
                this.tasks.Add(task);
            DoSetup();
        }

        private void DoSetup()
        {
            this.totalTimeInfo.PropertyChanged += totalTimeInfo_PropertyChanged;
            tasks.CollectionChanged += tasks_CollectionChanged;
            foreach (var task in tasks)
                task.OriginalTime.PropertyChanged += this.task_OriginalTime_PropertyChanged;

            CalculateValidationErrors();
        }

        #region IDropTarget
        //public void DragOver(IDropInfo dropInfo)
        //{
        //    TaskViewModel sourceItem = dropInfo.Data as TaskViewModel;
        //    dropInfo.Effects = (sourceItem == null) ? System.Windows.DragDropEffects.None : System.Windows.DragDropEffects.Move;
        //}

        //public void Drop(IDropInfo dropInfo)
        //{
        //    TaskViewModel sourceItem = dropInfo.Data as TaskViewModel;
        //    if (sourceItem == null)
        //        return;

        //    tasks.Remove(sourceItem);
        //    //- 1 because we just removed one.
        //    tasks.Insert(dropInfo.InsertIndex - 1, sourceItem);
        //}
        #endregion

        #region Events
        void totalTimeInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Minutes" || e.PropertyName == "Seconds" || e.PropertyName == "Hours")
            {
                NotifyOfPropertyChange(() => CanStartTasks);
                NotifyOfPropertyChange(() => CanAddTask);
                NotifyOfPropertyChange(() => CanAddTasks);
                CalculateValidationErrors();
            }
        }

        void tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanStartTasks);
            NotifyOfPropertyChange(() => CanAddTask);
            NotifyOfPropertyChange(() => CanAddTasks);
            CalculateValidationErrors();
        }

        private void task_OriginalTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if (e.PropertyName == "Minutes" || e.PropertyName == "Seconds" || e.PropertyName == "Hours")
            {
                NotifyOfPropertyChange(() => CanStartTasks);
                NotifyOfPropertyChange(() => CanAddTask);
                NotifyOfPropertyChange(() => CanAddTasks);
                CalculateValidationErrors();
            }
        }
        #endregion

        #region Methods
        public void AddTask(System.Windows.Controls.DataGrid tasksGrid)
        {
            //GetTimeRemaining()
            SelectedTask = InternalAddTask(String.Format("Task {0}", tasks.Count + 1), ColorInfo.GetRandomColor(), new TimeInfoViewModel());
            if (tasksGrid != null)
            {
                tasksGrid.ScrollIntoView(selectedTask);
                var cellInfo = new System.Windows.Controls.DataGridCellInfo(selectedTask, tasksGrid.Columns[1]);
                int row = tasksGrid.Items.IndexOf(selectedTask);
                tasksGrid.CurrentCell = cellInfo;
                var cell = tasksGrid.GetCell(row, 1);
                if (cell != null)
                    cell.Focus();
                tasksGrid.BeginEdit();
            }
                
            NotifyOfPropertyChange(() => CanAddTask);
            NotifyOfPropertyChange(() => CanStartTasks);
            NotifyOfPropertyChange(() => CanAddTasks);
            CalculateValidationErrors();
        }

        /// <summary>
        /// Whether or not we have enough information in the "Add A Task" row to add said task.
        /// </summary>
        public bool CanAddTask
        {
            get
            {
                return GetTimeRemaining().IsPositiveTime;
            }
        }

        public void RemoveTask(TaskViewModel task)
        {
            if (task == null)
                return;
            tasks.Remove(task);
            task.OriginalTime.PropertyChanged -= task_OriginalTime_PropertyChanged;
            CalculateValidationErrors();
        }

        public void StartTasks()
        {
            TimedTasksViewModel timedTasks = new TimedTasksViewModel(parent, TotalTimeInfo, tasks.ToList(), this, windowManager);
            parent.ActivateItem(timedTasks);
            parent.DeactivateItem(this, true);
        }

        public bool CanStartTasks
        {
            get { return TotalTimeInfo.IsPositiveTime && tasks.Count > 0 && GetTimeRemaining().IsZeroTime; }
        }

        /// <summary>
        /// Whether or not we can add more tasks
        /// </summary>
        public bool CanAddTasks
        {
            get { return TotalTimeInfo.IsPositiveTime && GetTimeRemaining().IsPositiveTime; }
        }

        public TimeInfoViewModel GetTotalTasksTime()
        {
            return new TimeInfoViewModel(tasks.Sum(x => x.OriginalTime.TotalSeconds));
        }

        public TimeInfoViewModel GetTimeRemaining()
        {
            var totalTaskTime = GetTotalTasksTime();
            var rtn = TotalTimeInfo - totalTaskTime;
            return rtn;
        }

        private TaskViewModel InternalAddTask(String name, ColorInfo colorInfo, TimeInfoViewModel timeInfo)
        {
            //Add it
            var task = new TaskViewModel(name, colorInfo, timeInfo);
            tasks.Add(task);
            task.OriginalTime.PropertyChanged += task_OriginalTime_PropertyChanged;
            return task;
        }

        private void CalculateValidationErrors()
        {
            validationErrors.Clear();
            if(totalTimeInfo.IsZeroTime)
            {
                validationErrors.Add("Please set your Total Time to at least one second.");
                return;
            }
            if(tasks.Count == 0)
            {
                validationErrors.Add("Please add at least one task.");
                return;
            }

            TimeInfoViewModel timeRemaining = GetTimeRemaining();
            if (timeRemaining.IsPositiveTime)
                validationErrors.Add(String.Format("Please use up the remaining {0}, or remove that much time from your Total Time.", timeRemaining.ToEnglishString()));
            else if (timeRemaining.IsNegativeTime)
                validationErrors.Add(String.Format("Please remove {0} from your tasks, or add that much time to your Total Time.", new TimeInfoViewModel(-timeRemaining.TotalSeconds).ToEnglishString()));

            NotifyOfPropertyChange(() => HasValidationErrors);
        }
        #endregion

        #region Properties
        //private TimeInfoViewModel totalTimeInfo = new TimeInfoViewModel(0, 0, 30);
        private TimeInfoViewModel totalTimeInfo = new TimeInfoViewModel(0, 0, 0);
        public TimeInfoViewModel TotalTimeInfo
        {
            get { return totalTimeInfo; }
            set
            {
                if (value == totalTimeInfo)
                    return;
                totalTimeInfo = value;
                NotifyOfPropertyChange(() => TotalTimeInfo);
            }
        }

        private ObservableCollection<TaskViewModel> tasks = new ObservableCollection<TaskViewModel>();
        public ObservableCollection<TaskViewModel> Tasks { get { return tasks; } }

        private TaskViewModel selectedTask = null;
        public TaskViewModel SelectedTask
        {
            get { return selectedTask; }
            set
            {
                if (value == selectedTask)
                    return;
                selectedTask = value;
                NotifyOfPropertyChange(() => SelectedTask);
            }
        }

        public ObservableCollection<String> validationErrors = new ObservableCollection<string>();
        public ObservableCollection<String> ValidationErrors { get { return validationErrors; } }

        public bool HasValidationErrors
        {
            get { return validationErrors.Count > 0; }
        }

        #endregion

    }
}
