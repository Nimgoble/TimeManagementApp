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
        }

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

        private TimeInfoViewModel originalTime;
        public TimeInfoViewModel OriginalTime
        {
            get { return originalTime; }
            set
            {
                if (value == originalTime)
                    return;
                originalTime = value;
                NotifyOfPropertyChange(() => OriginalTime);
            }
        }

        private TimeInfoViewModel completionTime;
        public TimeInfoViewModel CompletionTime
        {
            get { return completionTime; }
            set
            {
                if (value == completionTime)
                    return;
                completionTime = value;
                NotifyOfPropertyChange(() => CompletionTime);
            }
        }
    }
}
