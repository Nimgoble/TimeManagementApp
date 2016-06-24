using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeManagementApp.Models
{
    public class Settings
    {
        public Settings()
        {
            AutoSwitchTasks = false;
            WarningBeepPercentage = 20.0f;
            HighlightTimeleftDuringWarningPercentage = true;
            ShowSetupWizard = true;
        }
        public bool AutoSwitchTasks { get; set; }

        public float WarningBeepPercentage { get; set; }

        public bool HighlightTimeleftDuringWarningPercentage { get; set; }

        public bool ShowSetupWizard { get; set; }
    }
}
