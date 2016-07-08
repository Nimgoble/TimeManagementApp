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

        public bool IsZeroTime { get { return totalSeconds == 0; } }


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

        public string ToEnglishString()
        {
            List<string> parts = new List<string>();
            if (Hours > 0)
                parts.Add(String.Format("{0} hours", Hours));
            if (Minutes > 0)
                parts.Add(String.Format("{0} minutes", Minutes));
            if (Seconds > 0)
                parts.Add(String.Format("{0} seconds", Seconds));

            if (!parts.Any())
                return String.Empty;

            if (parts.Count == 1)
                return parts[0];

            if (parts.Count == 2)
                return String.Format("{0} and {1}", parts[0], parts[1]);

            if (parts.Count == 3)
                return String.Format("{0}, {1}, and {2}", parts[0], parts[1], parts[2]);

            return String.Empty;
        }
    }
}
