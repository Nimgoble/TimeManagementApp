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
        private readonly IWindowManager windowManager;
        private HomeViewModel homeViewModel = null;
        public ShellViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            homeViewModel = new HomeViewModel(this);
            this.ActivateItem(homeViewModel);
            DisplayName = "Time Management App";
        }

        public void ShowHome()
        {
            if(this.ActiveItem != null)
                this.DeactivateItem(this.ActiveItem, true);
            this.ActivateItem(homeViewModel);
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
            NotifyOfPropertyChange(() => CanNavigateToHome);
        }

        public bool CanNavigateToSettings
        {
            get { return (this.ActiveItem is TimedTasksViewModel) == false && (this.ActiveItem is SettingsViewModel) == false; }
        }

        public bool CanNavigateToHome
        {
            get { return (this.ActiveItem is HomeViewModel) == false; }
        }
    }
}
