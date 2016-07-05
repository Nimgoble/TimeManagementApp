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
using Caliburn.Micro;
namespace TimeManagementApp.Views
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView : UserControl
    {
        public SetupView()
        {
            InitializeComponent();
            //DataContextChanged += SetupView_DataContextChanged;
        }

        //private void SetupView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (!(e.NewValue is Screen) || !(e.OldValue is Screen))
        //        return;

        //    if(e.NewValue is Screen)
        //    {
        //        Screen newContext = (Screen)e.NewValue;
        //        if(newContext != null)
        //            newContext.PropertyChanged += Context_PropertyChanged;
        //    }
        //    if(e.OldValue is Screen)
        //    {
        //        Screen oldContext = (Screen)e.OldValue;
        //        if(oldContext != null)
        //            oldContext.PropertyChanged -= Context_PropertyChanged;
        //    }
        //}

        //private void Context_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "WizardViewState")
        //        this.ourContentControl.Focus();
        //}
    }
}
