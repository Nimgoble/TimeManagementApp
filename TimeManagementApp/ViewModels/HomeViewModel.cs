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
        public HomeViewModel(Conductor<Screen>.Collection.OneActive parent)
        {
            this.parent = parent;
        }

        public void StartWizard()
        {
            parent.ActivateItem(new SetupWizardViewModel(parent));
        }

        public void SkipToSetup()
        {
            parent.ActivateItem(new SetupViewModel(parent));
        }
    }
}
