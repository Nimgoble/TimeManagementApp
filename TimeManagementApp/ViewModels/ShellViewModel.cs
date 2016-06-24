using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using TimeManagementApp.Models;

namespace TimeManagementApp.ViewModels 
{
	public class ShellViewModel : Conductor<Screen>.Collection.OneActive
    {
        private SetupViewModel setupViewModel;
        public ShellViewModel()
        {
            setupViewModel = new SetupViewModel(this);
            this.ActivateItem(setupViewModel);
            DisplayName = "Time Management App";
        }

        public void ShowSettings()
        {
            var settings = IoC.Get<SettingsViewModel>();
            if (settings == null)
                return;

            settings.SetParentAndPreviousScreen(this, this.ActiveItem);
            this.ActivateItem(settings);
        }

        public override void ActivateItem(Screen item)
        {
            base.ActivateItem(item);
            NotifyOfPropertyChange(() => CanNavigateToSettings);
        }

        public bool CanNavigateToSettings
        {
            get { return (this.ActiveItem is TimedTasksViewModel) == false && (this.ActiveItem is SettingsViewModel) == false; }
        }
    }
}
