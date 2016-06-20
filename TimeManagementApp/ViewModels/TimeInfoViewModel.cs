using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace TimeManagementApp.ViewModels
{
    public class TimeInfoViewModel : Screen
    {
        public TimeInfoViewModel() { }

        public TimeInfoViewModel(int totalSeconds)
        {
            //The order of this matters.
            this.totalSeconds = totalSeconds;
        }

        public TimeInfoViewModel(int hours, int minutes, int seconds) 
        {
            //The order of this matters.
            this.Seconds = seconds;
            this.Minutes = minutes;
            this.Hours = hours;
        }

        public int Hours
        {
            get { return totalSeconds / 3600; }
            set { TotalSeconds += ((value - Hours) * 3600); }
        }

        public int Minutes
        {
            get { return (totalSeconds / 60) % 60; }
            set { TotalSeconds += ((value - Minutes) * 60); }
        }

        public int Seconds
        {
            get { return totalSeconds % 60; }
            set { TotalSeconds += (value - Seconds); }
        }

        private int totalSeconds;
        public int TotalSeconds
        {
            get { return totalSeconds; }
            set
            {
                if (value == totalSeconds)
                    return;
                //totalSeconds = (value < 0 ) ? 0 : value; //Cap it at 0
                totalSeconds = value;
                NotifyOfPropertyChange(() => TotalSeconds);
                NotifyOfPropertyChange(() => Seconds);
                NotifyOfPropertyChange(() => Minutes);
                NotifyOfPropertyChange(() => Hours);
            }
        }

        public bool IsPositiveTime { get { return totalSeconds > 0; } }

        public bool IsOutOfTime { get { return totalSeconds <= 0; } }

        public bool IsNegativeTime { get { return totalSeconds < 0; } }


        public static TimeInfoViewModel operator +(TimeInfoViewModel a, TimeInfoViewModel b)
        {
            return new TimeInfoViewModel(a.TotalSeconds + b.TotalSeconds);
        }

        public static TimeInfoViewModel operator -(TimeInfoViewModel a, TimeInfoViewModel b)
        {
            return new TimeInfoViewModel(a.TotalSeconds - b.TotalSeconds);
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}:{2}", Hours.ToString().PadLeft(2, '0'), Minutes.ToString("D2"), Seconds.ToString("D2"));
        }
    }
}
