﻿using System;
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

        private void TasksGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string breakHere = String.Empty;
        }

        private void TasksGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            string breakHere = String.Empty;
        }
    }
}
