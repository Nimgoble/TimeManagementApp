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
    }
}
