using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace TimeManagementApp.ViewModels
{
    public class AddTimeViewModel : Screen
    {
        private readonly TaskViewModel task;
        private TimeInfoViewModel timeLeft;
        public AddTimeViewModel(TaskViewModel task, TimeInfoViewModel timeLeft)
        {
            this.task = task;
            this.timeLeft = timeLeft;
            timeLeft.PropertyChanged += TimeLeft_PropertyChanged;
        }

        #region Events
        private void TimeLeft_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanAccept);
        }
        #endregion

        #region Methods
        public void Cancel()
        {
            this.TryClose(false);
        }

        public void Accept()
        {
            this.TryClose(true);
        }

        public bool CanAccept
        {
            get { return AddTimeInfo.TotalSeconds <= timeLeft.TotalSeconds; }
        }
        #endregion

        #region Properties
        private TimeInfoViewModel addTimeInfo = new TimeInfoViewModel();
        public TimeInfoViewModel AddTimeInfo
        {
            get { return addTimeInfo; }
            set
            {
                if (value == addTimeInfo)
                    return;
                addTimeInfo = value;
                NotifyOfPropertyChange(() => AddTimeInfo);
            }
        }

        public TimeInfoViewModel TimeLeft { get { return timeLeft; } }
        #endregion
    }
}
