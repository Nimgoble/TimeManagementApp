using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Gu.Wpf.NumericInput;
using TimeManagementApp.Extensions;

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
		public int CornerRadius 
        {
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

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
            "ReadOnly",
            typeof(bool),
            typeof(TimeControl),
            new PropertyMetadata(false));

        [Category("TimeControl")]
        [Browsable(true)]
        public bool ReadOnly
        {
            get { return (bool)this.GetValue(ReadOnlyProperty); }
            set { this.SetValue(ReadOnlyProperty, value); }
        }

		public TimeControl() 
		{
			InitializeComponent();
            this.Focusable = true;
            this.Loaded += TimeControl_Loaded;
		}

        private void TimeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Style != null)
            {
                foreach(var trigger in Style.Triggers)
                {
                    if(trigger is MultiDataTrigger)
                    {
                        var multiDataTrigger = trigger as MultiDataTrigger;
                        if (multiDataTrigger == null)
                            continue;

                        foreach(var condition in multiDataTrigger.Conditions)
                        {
                        }

                        foreach (var thing in multiDataTrigger.EnterActions)
                        {
                        }

                        foreach (var thing in multiDataTrigger.ExitActions)
                        {
                        }
                    }
                    string debugMe = string.Empty;
                }
            }

        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(e.Property.Name == "Background")
            {
                string debugMe = string.Empty;
            }
            base.OnPropertyChanged(e);
        }

        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) 
		{
			if (sender is IntBox) 
			{
				var intBox = (IntBox)sender;
				if (String.IsNullOrEmpty(intBox.RegexPattern))
					return;

				Regex regex = new Regex(intBox.RegexPattern);
                //If we have some text selected, that's going to be replaced.
                string resultantText = 
                    (intBox.SelectionLength > 0) ? 
                    String.Format("{0}{1}", intBox.Text.Remove(intBox.SelectionStart, intBox.SelectionLength), e.Text) : 
                    intBox.Text + e.Text;

                bool isMatch = regex.IsMatch(resultantText);
				e.Handled = !isMatch;
			}
		}

        private void IntBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is IntBox)
            {
                var intBox = (IntBox)sender;
                if (intBox.Text.Length == 2 && intBox != SecondsBox)
                    MoveToNextUIElement();
            }
        }
        void MoveToNextUIElement()
        {
            // Creating a FocusNavigationDirection object and setting it to a
            // local field that contains the direction selected.
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;

            // MoveFocus takes a TraveralReqest as its argument.
            TraversalRequest request = new TraversalRequest(focusDirection);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void IntBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is IntBox)
            {
                var intBox = (IntBox)sender;
                intBox.SelectionStart = 0;
                intBox.SelectionLength = 2;
            }
        }

        private void UserControl_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OldFocus == null && e.NewFocus == this)
            {
                //HoursBox.BeginInvoke(d => d.Focus());
                System.Threading.ThreadPool.QueueUserWorkItem(
                   (a) =>
                   {
                       System.Threading.Thread.Sleep(100);
                       HoursBox.Dispatcher.Invoke(
                       new Action(() =>
                       {
                           HoursBox.Focus();

                       }));
                   }
                );
            }
        }
    }
}
