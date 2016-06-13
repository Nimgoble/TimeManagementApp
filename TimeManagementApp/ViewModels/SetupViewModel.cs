using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TimeManagementApp.Models;
using GongSolutions.Wpf.DragDrop;
namespace TimeManagementApp.ViewModels
{
    public class SetupViewModel : Screen/*, IDropTarget*/
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        public SetupViewModel(Conductor<Screen>.Collection.OneActive parent)
        {
            this.parent = parent;
            this.newTimeInfo.PropertyChanged += newTimeInfo_PropertyChanged;
            this.totalTimeInfo.PropertyChanged += totalTimeInfo_PropertyChanged;
            tasks.CollectionChanged += tasks_CollectionChanged;
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
            }
        }

        void tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanStartTasks);
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
            tasks.Add(new TaskViewModel(newTimeSliceName, selectedColor, newTimeInfo));

            newTimeInfo.PropertyChanged -= newTimeInfo_PropertyChanged;
            NewTimeInfo = GetTimeRemaining();
            newTimeInfo.PropertyChanged += newTimeInfo_PropertyChanged;

            SelectedColor = null;
            NewTimeSliceName = String.Empty;
            NotifyOfPropertyChange(() => CanStartTasks);
        }

        public bool CanAddTask
        {
            get
            {
                return
                    !String.IsNullOrEmpty(newTimeSliceName) &&
                    newTimeInfo.IsValidTime &&
                    selectedColor != null &&
                    GetTimeRemaining().IsValidTime;
            }
        }

        public void RemoveTask(TaskViewModel task)
        {
            if (task == null)
                return;
            tasks.Remove(task);
        }

        public void StartTasks()
        {
            TimedTasksViewModel timedTasks = new TimedTasksViewModel(tasks.ToList());
            parent.ActivateItem(timedTasks);
        }

        public bool CanStartTasks
        {
            get { return TotalTimeInfo.IsValidTime && tasks.Count > 0 && !GetTimeRemaining().IsValidTime; }
        }
        #endregion

        #region Properties
        private TimeInfoViewModel totalTimeInfo = new TimeInfoViewModel();
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

    }
}
