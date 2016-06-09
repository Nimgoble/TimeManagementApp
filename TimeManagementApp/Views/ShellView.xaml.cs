using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Gu.Wpf.NumericInput;
namespace TimeManagementApp.Views {
	/// <summary>
	/// Interaction logic for ShellView.xaml
	/// </summary>
	public partial class ShellView : UserControl {
		public ShellView() {
			InitializeComponent();
		}

		private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (sender is IntBox)
			{
				var intBox = (IntBox) sender;
				if (String.IsNullOrEmpty(intBox.RegexPattern))
					return;

				Regex regex = new Regex(intBox.RegexPattern);
				bool isMatch = regex.IsMatch(intBox.Text + e.Text);
				e.Handled = !isMatch;
			}
		}
	}
}
