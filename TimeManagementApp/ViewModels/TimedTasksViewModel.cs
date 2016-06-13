using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Caliburn.Micro;
namespace TimeManagementApp.ViewModels
{
    public class TimedTasksViewModel : Screen
    {

        public TimedTasksViewModel(TimeInfoViewModel totalTimeInfo, List<TaskViewModel> _tasks)
        {
            this.totalTimeInfo = totalTimeInfo;
            this.timer = new Timer(1000);
            this.timer.Elapsed += timer_Elapsed;
            foreach (var task in _tasks)
                tasks.Add(task);
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
                NotifyOfPropertyChange(() => ElapsedTime);
            }
        }

        #endregion
    }
}
