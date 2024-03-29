﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Caliburn.Micro;
using TimeManagementApp.Models;
using log4net;
namespace TimeManagementApp.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(SettingsViewModel));
        private Screen previousScreen;
        private Conductor<Screen>.Collection.OneActive parent;
        private Settings settings = new Settings();
        public SettingsViewModel()
        {
            LoadSettings();
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
                        catch(Exception ex)
                        {
                            //TODO: Logging
                            logger.Error("Error loading settings", ex);
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
            WriteSettings();
            this.parent.ActivateItem(previousScreen);
            //this.parent.DeactivateItem(this, false);
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

        public bool ShouldWarnUserThatCurrentTasksTimeIsRunningOut
        {
            get { return settings.ShouldWarnUserThatCurrentTasksTimeIsRunningOut; }
            set
            {
                if (value == settings.ShouldWarnUserThatCurrentTasksTimeIsRunningOut)
                    return;
                settings.ShouldWarnUserThatCurrentTasksTimeIsRunningOut = value;
                NotifyOfPropertyChange(() => ShouldWarnUserThatCurrentTasksTimeIsRunningOut);
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
