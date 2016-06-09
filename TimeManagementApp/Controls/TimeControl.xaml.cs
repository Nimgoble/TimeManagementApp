using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Gu.Wpf.NumericInput;

namespace TimeManagementApp.Controls 
{
	/// <summary>
	/// Interaction logic for TimeControl.xaml
	/// </summary>
	public partial class TimeControl : UserControl 
	{
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
			"CornerRadius",
			typeof(int),
			typeof(TimeControl),
			new PropertyMetadata(7));

		[Category("TimeControl")]
		[Browsable(true)]
		public int CornerRadius {
			get { return (int)this.GetValue(CornerRadiusProperty); }
			set { this.SetValue(CornerRadiusProperty, value); }
		}

		public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
			"RegexPattern",
			typeof(string),
			typeof(TimeControl),
			new PropertyMetadata(default(string)));

		/// <summary>
		/// Gets or sets a regex pattern for validation
		/// </summary>
		[Category("TimeControl")]
		[Browsable(true)]
		public string RegexPattern 
		{
			get { return (string)this.GetValue(RegexPatternProperty); }
			set { this.SetValue(RegexPatternProperty, value); }
		}

		public static readonly DependencyProperty HoursProperty = DependencyProperty.Register(
			"Hours",
			typeof(int),
			typeof(TimeControl),
			new PropertyMetadata(00));

		[Category("TimeControl")]
		[Browsable(true)]
		public int Hours 
		{
			get { return (int)this.GetValue(HoursProperty); }
			set { this.SetValue(HoursProperty, value); }
		}

		public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register(
			"Minutes",
			typeof(int),
			typeof(TimeControl),
			new PropertyMetadata(00));

		[Category("TimeControl")]
		[Browsable(true)]
		public int Minutes {
			get { return (int)this.GetValue(MinutesProperty); }
			set { this.SetValue(MinutesProperty, value); }
		}

		public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register(
			"Seconds",
			typeof(int),
			typeof(TimeControl),
			new PropertyMetadata(00));

		[Category("TimeControl")]
		[Browsable(true)]
		public int Seconds {
			get { return (int)this.GetValue(SecondsProperty); }
			set { this.SetValue(SecondsProperty, value); }
		}

		public TimeControl() 
		{
			InitializeComponent();
			this.DataContext = this;
		}

		private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) 
		{
			if (sender is IntBox) 
			{
				var intBox = (IntBox)sender;
				if (String.IsNullOrEmpty(intBox.RegexPattern))
					return;

				Regex regex = new Regex(intBox.RegexPattern);
				bool isMatch = regex.IsMatch(intBox.Text + e.Text);
				e.Handled = !isMatch;
			}
		}
	}
}
