using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace TimeManagementApp.ViewModels
{
    public class HomeViewModel : Screen
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        private readonly IWindowManager windowManager;
        public HomeViewModel(Conductor<Screen>.Collection.OneActive parent, IWindowManager windowManager)
        {
            this.parent = parent;
            this.windowManager = windowManager;
        }

        public void StartWizard()
        {
            parent.ActivateItem(new SetupWizardViewModel(parent, windowManager));
        }

        public void SkipToSetup()
        {
            parent.ActivateItem(new SetupViewModel(parent, windowManager));
        }
    }
}
