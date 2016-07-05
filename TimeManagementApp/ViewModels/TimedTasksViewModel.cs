using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;
using Caliburn.Micro;
using NAudio;
using TimeManagementApp.Models;
namespace TimeManagementApp.ViewModels
{
    public class TimedTasksViewModel : Screen, IDisposable
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        private readonly SetupViewModel setupViewModel;
        private readonly SettingsViewModel settings;
        private readonly IWindowManager windowManager;
        private SoundPlayer soundPlayer;
        public TimedTasksViewModel(Conductor<Screen>.Collection.OneActive parent, TimeInfoViewModel totalTimeInfo, List<TaskViewModel> _tasks, SetupViewModel setupViewModel/*, IWindowManager windowManager*/)
        {
            this.parent = parent;
            this.setupViewModel = setupViewModel;
            this.totalTimeInfo = totalTimeInfo;
            //this.windowManager = windowManager;
            timeLeft.TotalSeconds = totalTimeInfo.TotalSeconds;
            this.timer = new Timer(1000);
            this.timer.Elapsed += timer_Elapsed;
            foreach (var task in _tasks)
                tasks.Add(task);

            //tasksView = CollectionViewSource.GetDefaultView(tasks);
            //tasksView.CurrentChanging += TasksView_CurrentChanging;

            this.settings = IoC.Get<SettingsViewModel>();
            if (settings != null)
                AutomaticallySwitchTasks = settings.AutoSwitchTasks;

            soundPlayer = new SoundPlayer(new Uri("pack://application:,,,/TimeManagementApp;component/Resources/threeBeep.mp3"));
            soundPlayer.Initialize();
        }

        //private void TasksView_CurrentChanging(object sender, CurrentChangingEventArgs e)
        //{
        //    string breakHere = string.Empty;
        //}

        ~TimedTasksViewModel()
        {
            if(soundPlayer != null)
            {
                soundPlayer.Cleanup();
                soundPlayer = null;
            }
        }

        void IDisposable.Dispose()
        {
            Stop();
        }

        protected override void OnViewReady(object view)
        {
            Start();
            NotifyOfPropertyChange(() => AutomaticallySwitchTasks);
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
            this.parent.ActivateItem(new SetupViewModel(parent/*, windowManager*/));
            this.parent.DeactivateItem(this, true);
        }

        private void CheckShouldSignalWarning()
        {
            if (settings == null)
                return;

            if (!settings.ShouldWarnUserThatCurrentTasksTimeIsRunningOut)
                return;

            float percentage = ((float)CurrentTask.TimeLeft.TotalSeconds) / ((float)CurrentTask.OriginalTime.TotalSeconds);
            int timeleftPercentage = (int)(percentage * 100);
            bool isAtOrBelowWarningThreshold = timeleftPercentage <= settings.WarningBeepPercentage;
            if(isAtOrBelowWarningThreshold)
            {
                //if(settings.HighlightTimeleftDuringWarningPercentage)
                if(!soundPlayer.HasPlayed)
                    soundPlayer.Play();

            }
        }

        public void AddTimeToTask(TaskViewModel task)
        {

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

        ////private List<TaskViewModel> tasksList = new List<TaskViewModel>();
        //private ObservableCollection<TaskViewModel> tasks = new ObservableCollection<TaskViewModel>();
        //private ICollectionView tasksView = null;
        //public ICollectionView TasksView { get { return tasksView; } }

        private TaskViewModel currentTask = null;
        public TaskViewModel CurrentTask
        {
            get { return currentTask; }
            set
            {
                if (value == currentTask)
                    return;

                if
                (
                    AutomaticallySwitchTasks ||
                    (
                        value == null || value.TimeLeft.IsPositiveTime
                    )
                )
                {
                    currentTask = value;
                }
                    

                soundPlayer.Reset();
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
                {
                    currentTask.ElapsedTime.TotalSeconds++;
                    CheckShouldSignalWarning();
                }
                    

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
            }
        }

        #endregion
    }
}
