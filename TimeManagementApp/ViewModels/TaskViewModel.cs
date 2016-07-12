using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TimeManagementApp.Models;
namespace TimeManagementApp.ViewModels
{
    public class TaskViewModel : Screen
    {

        public TaskViewModel(string name, ColorInfo colorInfo, TimeInfoViewModel originalTime)
        {
            this.name = name;
            this.colorInfo = colorInfo;
            this.originalTime = originalTime;
            this.elapsedTime.PropertyChanged += ElapsedTime_PropertyChanged;
            this.originalTime.PropertyChanged += OriginalTime_PropertyChanged;
        }

        #region Events
        private void ElapsedTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => TimeLeft);
            NotifyOfPropertyChange(() => IsInWarningPercentage);
        }

        private void OriginalTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => TimeLeft);
            NotifyOfPropertyChange(() => IsInWarningPercentage);
        }

        #endregion

        #region Properties
        private ColorInfo colorInfo;
        public ColorInfo ColorInfo
        {
            get { return colorInfo; }
            set
            {
                if (value == colorInfo)
                    return;
                colorInfo = value;
                NotifyOfPropertyChange(() => ColorInfo);
            }
        }

        private String name = String.Empty;
        public String Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private TimeInfoViewModel originalTime = new TimeInfoViewModel();
        public TimeInfoViewModel OriginalTime
        {
            get { return originalTime; }
            set
            {
                if (value == originalTime)
                    return;
                originalTime = value;
                NotifyOfPropertyChange(() => OriginalTime);
                NotifyOfPropertyChange(() => TimeLeft);
                NotifyOfPropertyChange(() => IsInWarningPercentage);
            }
        }

        private TimeInfoViewModel elapsedTime = new TimeInfoViewModel();
        public TimeInfoViewModel ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                if (value == elapsedTime)
                    return;
                elapsedTime = value;
                NotifyOfPropertyChange(() => ElapsedTime);
                NotifyOfPropertyChange(() => TimeLeft);
                NotifyOfPropertyChange(() => IsInWarningPercentage);
            }
        }

        public TimeInfoViewModel TimeLeft
        {
            get { return originalTime - elapsedTime; }
        }

        private float warningPercentage = 0.0f;
        public float WarningPercentage
        {
            get { return warningPercentage; }
            set
            {
                if (value == warningPercentage)
                    return;
                warningPercentage = value;
                NotifyOfPropertyChange(() => WarningPercentage);
                NotifyOfPropertyChange(() => IsInWarningPercentage);
            }
        }

        public bool IsInWarningPercentage
        {
            get
            {
                if (warningPercentage <= 0.0f)
                    return false;

                float percentage = ((float)TimeLeft.TotalSeconds) / ((float)originalTime.TotalSeconds);
                int timeleftPercentage = (int)(percentage * 100);
                bool isAtOrBelowWarningThreshold = timeleftPercentage <= warningPercentage;
                return isAtOrBelowWarningThreshold;
            }
        }

        private bool isActiveTask = false;
        public bool IsActiveTask
        {
            get { return isActiveTask; }
            set
            {
                if (value == isActiveTask)
                    return;
                isActiveTask = value;
                NotifyOfPropertyChange(() => IsActiveTask);
            }
        }
        #endregion
    }
}
