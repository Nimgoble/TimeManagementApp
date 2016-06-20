﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Caliburn.Micro;
namespace TimeManagementApp.ViewModels
{
    public class TimedTasksViewModel : Screen, IDisposable
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        private readonly SetupViewModel setupViewModel;
        public TimedTasksViewModel(Conductor<Screen>.Collection.OneActive parent, TimeInfoViewModel totalTimeInfo, List<TaskViewModel> _tasks, SetupViewModel setupViewModel)
        {
            this.parent = parent;
            this.setupViewModel = setupViewModel;
            this.totalTimeInfo = totalTimeInfo;
            timeLeft.TotalSeconds = totalTimeInfo.TotalSeconds;
            this.timer = new Timer(1000);
            this.timer.Elapsed += timer_Elapsed;
            foreach (var task in _tasks)
                tasks.Add(task);
        }

        void IDisposable.Dispose()
        {
            Stop();
        }

        protected override void OnViewReady(object view)
        {
            Start();
            base.OnViewReady(view);
        }

        #region Methods
        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void NextTask()
        {
            int index = tasks.IndexOf(currentTask);
            CurrentTask = tasks[++index];
        }

        public bool CanNextTask
        {
            get { return tasks.Any() && CurrentTask != null && CurrentTask != tasks.Last() && !automaticallySwitchTasks; }
        }

        public void GoBack()
        {
            Stop();
            foreach (var task in tasks)
                task.ElapsedTime = new TimeInfoViewModel();

            this.parent.ActivateItem(setupViewModel);
            this.parent.DeactivateItem(this, true);
        }

        public void StartNewList()
        {
            Stop();
            this.parent.ActivateItem(new SetupViewModel(parent));
            this.parent.DeactivateItem(this, true);
        }
        #endregion

        #region Events

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ++ElapsedTime;
            if (ElapsedTime >= totalTimeInfo.TotalSeconds)
                Stop();
        }

        #endregion

        #region Properties

        private Timer timer;

        private TimeInfoViewModel totalTimeInfo;
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

        private TaskViewModel currentTask = null;
        public TaskViewModel CurrentTask
        {
            get { return currentTask; }
            set
            {
                if (value == currentTask)
                    return;
                currentTask = value;
                NotifyOfPropertyChange(() => CurrentTask);
                NotifyOfPropertyChange(() => CanNextTask);
            }
        }

        private int elapsedTime = 0;
        public int ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                if (value == elapsedTime)
                    return;
                elapsedTime = value;

                //Time Left
                --timeLeft.TotalSeconds;
                //Current Task's total elapsed time.
                if (currentTask != null)
                    currentTask.ElapsedTime.TotalSeconds++;

                NotifyOfPropertyChange(() => ElapsedTime);
                NotifyOfPropertyChange(() => CurrentTaskTimeLeft);
                NotifyOfPropertyChange(() => TimeLeft);
            }
        }

        private TimeInfoViewModel timeLeft = new TimeInfoViewModel();
        public TimeInfoViewModel TimeLeft
        {
            get { return timeLeft; }
        }

        public String CurrentTaskTimeLeft
        {
            get
            {
                return (currentTask == null) ? String.Empty : currentTask.TimeLeft.ToString();
            }
        }

        private bool automaticallySwitchTasks = true;
        public bool AutomaticallySwitchTasks
        {
            get { return automaticallySwitchTasks; }
            set
            {
                if (value == automaticallySwitchTasks)
                    return;
                automaticallySwitchTasks = value;
                NotifyOfPropertyChange(() => AutomaticallySwitchTasks);
                NotifyOfPropertyChange(() => CanNextTask);
            }
        }

        #endregion
    }
}
