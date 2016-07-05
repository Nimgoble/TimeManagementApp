using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TimeManagementApp.Models;
namespace TimeManagementApp.ViewModels
{
    public class SetupWizardViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly Conductor<Screen>.Collection.OneActive parent;
        public SetupWizardViewModel(Conductor<Screen>.Collection.OneActive parent)
        {
            this.parent = parent;
            totalTimeInfo.PropertyChanged += this.totalTimeInfo_PropertyChanged;
        }


        #region Methods
        public void MoveToNextSetupState()
        {
            if (wizardViewState == WizardViewState.NumberOfTasks)
            {
                List<TaskViewModel> tasks = new List<TaskViewModel>();
                int averageSeconds = totalTimeInfo.TotalSeconds / numberOfTasks;
                int leftoverSeconds = totalTimeInfo.TotalSeconds % numberOfTasks;

                for (int i = 1; i <= numberOfTasks; ++i)
                {
                    var task = new TaskViewModel(String.Format("Task {0}", i.ToString()), ColorInfo.GetRandomColor(), new TimeInfoViewModel(averageSeconds));
                    tasks.Add(task);
                }

                //Add the remaining seconds
                for (int i = 0; i < leftoverSeconds; ++i)
                    tasks[i].OriginalTime.TotalSeconds++;

                parent.ActivateItem(new SetupViewModel(parent, totalTimeInfo, tasks));
            }

            WizardViewState++;
        }

        public bool CanMoveToNextSetupState
        {
            get
            {
                switch (wizardViewState)
                {
                    case WizardViewState.TotalTime:
                        return TotalTimeInfo.IsPositiveTime;
                    case WizardViewState.NumberOfTasks:
                        return NumberOfTasks > 0 && (TotalTimeInfo.TotalSeconds / NumberOfTasks) > 0;
                    default:
                        return true;
                }
            }
        }
        #endregion

        #region Events
        void totalTimeInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => CanMoveToNextSetupState);
        }
        #endregion

        #region Properties
        private WizardViewState wizardViewState = WizardViewState.TotalTime;
        public WizardViewState WizardViewState
        {
            get { return wizardViewState; }
            set
            {
                if (value == wizardViewState)
                    return;
                wizardViewState = value;
                NotifyOfPropertyChange(() => WizardViewState);
            }
        }

        private TimeInfoViewModel totalTimeInfo = new TimeInfoViewModel(0, 0, 0);
        public TimeInfoViewModel TotalTimeInfo
        {
            get { return totalTimeInfo; }
            set
            {
                if (value == totalTimeInfo)
                    return;
                totalTimeInfo = value;
                NotifyOfPropertyChange(() => TotalTimeInfo);
            }
        }

        private int numberOfTasks = 0;
        public int NumberOfTasks
        {
            get { return numberOfTasks; }
            set
            {
                if (value == numberOfTasks)
                    return;
                numberOfTasks = value;
                NotifyOfPropertyChange(() => NumberOfTasks);
                NotifyOfPropertyChange(() => CanMoveToNextSetupState);
            }
        }
        #endregion
    }
}
