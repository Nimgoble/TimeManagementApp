using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using TimeManagementApp.Models;
namespace TimeManagementApp.ViewModels
{
    public class SetupViewModel : Screen/*, IDropTarget*/
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;

        private List<TaskViewModel> testTasks = new List<TaskViewModel>()
        {
            new TaskViewModel("Task 1", new ColorInfo("Aqua", System.Windows.Media.Colors.Aqua), new TimeInfoViewModel(0, 0, 10)),
            new TaskViewModel("Task 2", new ColorInfo("BlueViolet", System.Windows.Media.Colors.BlueViolet), new TimeInfoViewModel(0, 0, 10)),
            new TaskViewModel("Task 3", new ColorInfo("Chartreuse", System.Windows.Media.Colors.Chartreuse), new TimeInfoViewModel(0, 0, 10))
        };

        public SetupViewModel(Conductor<Screen>.Collection.OneActive parent)
        {
            this.parent = parent;
            this.newTimeInfo.PropertyChanged += newTimeInfo_PropertyChanged;
            this.totalTimeInfo.PropertyChanged += totalTimeInfo_PropertyChanged;
            tasks.CollectionChanged += tasks_CollectionChanged;

            //foreach (var task in testTasks)
            //    tasks.Add(task);
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
                NotifyOfPropertyChange(() => CanMoveToNextSetupState);
                newTimeInfo.TotalSeconds = GetTimeRemaining().TotalSeconds;
            }
        }

        void tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanStartTasks);
            NotifyOfPropertyChange(() => CanAddTasks);
        }

        void newTimeInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Minutes" || e.PropertyName == "Seconds" || e.PropertyName == "Hours")
                NotifyOfPropertyChange(() => CanAddTask);
        }
        #endregion

        #region Methods
        public void AddTask()
        {
            //Add it
            tasks.Add(new TaskViewModel(newTimeSliceName, selectedColor, newTimeInfo));

            //Set default values for (potential) new one.
            NewTimeSliceName = String.Format("Task {0}", tasks.Count + 1);
            SelectedColor = ColorInfo.GetRandomColor();

            newTimeInfo.PropertyChanged -= newTimeInfo_PropertyChanged;
            NewTimeInfo = GetTimeRemaining();
            newTimeInfo.PropertyChanged += newTimeInfo_PropertyChanged;

            NotifyOfPropertyChange(() => CanStartTasks);
            NotifyOfPropertyChange(() => CanAddTasks);
        }

        /// <summary>
        /// Whether or not we have enough information in the "Add A Task" row to add said task.
        /// </summary>
        public bool CanAddTask
        {
            get
            {
                return
                    !String.IsNullOrEmpty(newTimeSliceName) &&
                    newTimeInfo.IsPositiveTime &&
                    selectedColor != null &&
                    GetTimeRemaining().IsPositiveTime;
            }
        }

        public void RemoveTask(TaskViewModel task)
        {
            if (task == null)
                return;
            tasks.Remove(task);
            newTimeInfo.TotalSeconds += task.OriginalTime.TotalSeconds;
        }

        public void StartTasks()
        {
            TimedTasksViewModel timedTasks = new TimedTasksViewModel(parent, TotalTimeInfo, tasks.ToList(), this);
            parent.ActivateItem(timedTasks);
            parent.DeactivateItem(this, true);
        }

        public bool CanStartTasks
        {
            get { return TotalTimeInfo.IsPositiveTime && tasks.Count > 0 && !GetTimeRemaining().IsPositiveTime; }
        }

        /// <summary>
        /// Whether or not we can add more tasks
        /// </summary>
        public bool CanAddTasks
        {
            get { return TotalTimeInfo.IsPositiveTime && GetTimeRemaining().IsPositiveTime; }
        }

        public void MoveToNextSetupState()
        {
            if (wizardViewState == WizardViewState.SetupOverview)
                return;

            if(wizardViewState == WizardViewState.NumberOfTasks)
            {
                tasks.Clear();
                int averageSeconds = totalTimeInfo.TotalSeconds / requestedTotalNumberOfTasks;
                int leftoverSeconds = totalTimeInfo.TotalSeconds % requestedTotalNumberOfTasks;
                
                for (int i = 1; i <= requestedTotalNumberOfTasks; ++i)
                    tasks.Add(new TaskViewModel(String.Format("Task {0}", i.ToString()), ColorInfo.GetRandomColor(), new TimeInfoViewModel(averageSeconds)));

                //Add the remaining seconds
                for(int i = 0; i < leftoverSeconds; ++i)
                    tasks[i].OriginalTime.TotalSeconds++;
            }

            WizardViewState++;
        }

        public bool CanMoveToNextSetupState
        {
            get
            {
                switch (wizardViewState)
                {
                    case WizardViewState.TotalTime:
                        return TotalTimeInfo.IsPositiveTime;
                    case WizardViewState.NumberOfTasks:
                        return RequestedTotalNumberOfTasks > 0 && (TotalTimeInfo.TotalSeconds / RequestedTotalNumberOfTasks) > 0;
                    case WizardViewState.SetupOverview:
                        return true;
                    default:
                        return true;
                }
            }
        }

        public void SkipSetupWizard()
        {
            WizardViewState = WizardViewState.SetupOverview;
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

        private TimeInfoViewModel newTimeInfo = new TimeInfoViewModel();
        public TimeInfoViewModel NewTimeInfo
        {
            get { return newTimeInfo; }
            set
            {
                if (value == newTimeInfo)
                    return;
                newTimeInfo = value;
                NotifyOfPropertyChange(() => NewTimeInfo);
            }
        }

        private ColorInfo selectedColor;
        public ColorInfo SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (value == selectedColor)
                    return;
                selectedColor = value;
                NotifyOfPropertyChange(() => SelectedColor);
                NotifyOfPropertyChange(() => CanAddTask);
            }
        }

        private String newTimeSliceName = string.Empty;
        public String NewTimeSliceName
        {
            get { return newTimeSliceName; }
            set
            {
                if (value == newTimeSliceName)
                    return;
                newTimeSliceName = value;
                NotifyOfPropertyChange(() => NewTimeSliceName);
                NotifyOfPropertyChange(() => CanAddTask);
            }
        }

        private ObservableCollection<TaskViewModel> tasks = new ObservableCollection<TaskViewModel>();
        public ObservableCollection<TaskViewModel> Tasks { get { return tasks; } }

        private WizardViewState wizardViewState = WizardViewState.TotalTime;
        public WizardViewState WizardViewState
        {
            get { return wizardViewState; }
            set
            {
                if (value == wizardViewState)
                    return;
                wizardViewState = value;
                NotifyOfPropertyChange(() => WizardViewState);
            }
        }

        private int requestedTotalNumberOfTasks = 0;
        public int RequestedTotalNumberOfTasks
        {
            get { return requestedTotalNumberOfTasks; }
            set
            {
                if (value == requestedTotalNumberOfTasks)
                    return;
                requestedTotalNumberOfTasks = value;
                NotifyOfPropertyChange(() => RequestedTotalNumberOfTasks);
                NotifyOfPropertyChange(() => CanMoveToNextSetupState);
            }
        }

        public ObservableCollection<String> validationErrors = new ObservableCollection<string>();
        public ObservableCollection<String> ValidationErrors { get { return validationErrors; } }

        #endregion

    }
}
