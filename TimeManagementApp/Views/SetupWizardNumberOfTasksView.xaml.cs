using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeManagementApp.Extensions;
namespace TimeManagementApp.Views
{
    /// <summary>
    /// Interaction logic for SetupWizardNumberOfTasksView.xaml
    /// </summary>
    public partial class SetupWizardNumberOfTasksView : UserControl
    {
        public SetupWizardNumberOfTasksView()
        {
            InitializeComponent();
            this.Loaded += SetupWizardNumberOfTasksView_Loaded;
        }

        private void SetupWizardNumberOfTasksView_Loaded(object sender, RoutedEventArgs e)
        {
            //numberControl.BeginInvoke(d => d.Focus());
            System.Threading.ThreadPool.QueueUserWorkItem(
                   (a) =>
                   {
                       System.Threading.Thread.Sleep(100);
                       numberControl.Dispatcher.Invoke(
                       new Action(() =>
                       {
                           numberControl.Focus();

                       }));
                   }
                );
        }
    }
}
