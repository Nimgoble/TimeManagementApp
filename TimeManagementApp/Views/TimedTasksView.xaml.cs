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
using TimeManagementApp.ViewModels;
using TimeManagementApp.Extensions;
namespace TimeManagementApp.Views
{
    /// <summary>
    /// Interaction logic for TimedTasksView.xaml
    /// </summary>
    public partial class TimedTasksView : UserControl
    {
        public TimedTasksView()
        {
            InitializeComponent();
        }

        private void TasksGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TimedTasksViewModel dataContext = (TimedTasksViewModel)this.DataContext;
            if (dataContext == null)
                return;

            var source = e.OriginalSource;
            if (source == null)
                return;

            if (source is Visual == false || source is IInputElement == false) 
                return;

            Visual visualSource = (Visual)source;
            var parentButton = visualSource.GetParentOfType<Button>();

            if (dataContext.AutomaticallySwitchTasks)
            {
                if(parentButton != null)
                    parentButton.ClickButton();
                e.Handled = true;
                return;
            }
        }

        private void TasksGrid_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TimedTasksViewModel dataContext = (TimedTasksViewModel)this.DataContext;
            if (dataContext == null)
                return;

            if (dataContext.AutomaticallySwitchTasks)
            {
                e.Handled = true;
                return;
            }
        }
    }
}
