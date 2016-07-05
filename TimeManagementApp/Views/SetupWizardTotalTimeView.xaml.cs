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
    /// Interaction logic for SetupWizardTotalTimeView.xaml
    /// </summary>
    public partial class SetupWizardTotalTimeView : UserControl
    {
        public SetupWizardTotalTimeView()
        {
            InitializeComponent();
            this.Loaded += SetupWizardTotalTimeView_Loaded;
        }

        private void SetupWizardTotalTimeView_Loaded(object sender, RoutedEventArgs e)
        {
            //timeControl.BeginInvoke(d => d.Focus());
            System.Threading.ThreadPool.QueueUserWorkItem(
                   (a) =>
                   {
                       System.Threading.Thread.Sleep(100);
                       timeControl.Dispatcher.Invoke(
                       new Action(() =>
                       {
                           timeControl.Focus();

                       }));
                   }
                );
        }
    }
}
