using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Caliburn.Micro;
using TimeManagementApp.Models;
namespace TimeManagementApp.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private Screen previousScreen;
        private Conductor<Screen>.Collection.OneActive parent;
        private Settings settings = new Settings();
        public SettingsViewModel()
        {
        }

        #region Methods
        public void LoadSettings()
        {
            using (FileStream settingsFile = new FileStream("Settings.json", FileMode.OpenOrCreate))
            {
                using (StreamReader reader= new StreamReader(settingsFile))
                {
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        try
                        {
                            JsonSerializer serializer = JsonSerializer.Create();
                            this.settings = serializer.Deserialize<Settings>(jsonReader);
                            if (settings == null)
                                settings = new Settings();
                        }
                        catch
                        {
                            //TODO: Logging
                        }
                    }
                }
            }
        }

        public void WriteSettings()
        {
            using (FileStream settingsFile = new FileStream("Settings.json", FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(settingsFile))
                {
                    string json = JsonConvert.SerializeObject(settings);
                    writer.Write(json);
                }
            }
        }

        public void SetParentAndPreviousScreen(Conductor<Screen>.Collection.OneActive parent, Screen previousScreen)
        {
            this.parent = parent;
            this.previousScreen = previousScreen;
        }

        public void GoBack()
        {
            this.parent.ActivateItem(previousScreen);
            this.parent.DeactivateItem(this, true);
        }
        #endregion

        #region Properties
        public bool AutoSwitchTasks
        {
            get { return settings.AutoSwitchTasks; }
            set
            {
                if (value == settings.AutoSwitchTasks)
                    return;
                settings.AutoSwitchTasks = value;
                NotifyOfPropertyChange(() => AutoSwitchTasks);
            }
        }

        public float WarningBeepPercentage
        {
            get { return settings.WarningBeepPercentage; }
            set
            {
                if (value == settings.WarningBeepPercentage)
                    return;
                settings.WarningBeepPercentage = value;
                NotifyOfPropertyChange(() => WarningBeepPercentage);
            }
        }

        public bool HighlightTimeleftDuringWarningPercentage
        {
            get { return settings.HighlightTimeleftDuringWarningPercentage; }
            set
            {
                if (value == settings.HighlightTimeleftDuringWarningPercentage)
                    return;
                settings.HighlightTimeleftDuringWarningPercentage = value;
                NotifyOfPropertyChange(() => HighlightTimeleftDuringWarningPercentage);
            }
        }

        public bool ShowSetupWizard
        {
            get { return settings.ShowSetupWizard; }
            set
            {
                if (value == settings.ShowSetupWizard)
                    return;
                settings.ShowSetupWizard = value;
                NotifyOfPropertyChange(() => ShowSetupWizard);
            }
        }
        #endregion
    }
}
