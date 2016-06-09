using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using TimeManagementApp.Models;

namespace TimeManagementApp.ViewModels 
{
	public class ShellViewModel : Screen
	{

		private int hours = 0;
		public int Hours
		{
			get { return hours; }
			set
			{
				if (value == hours)
					return;
				hours = value;
				NotifyOfPropertyChange(() => Hours);
			}
		}

		private int minutes = 0;
		public int Minutes {
			get { return minutes; }
			set {
				if (value == minutes)
					return;
				minutes = value;
				NotifyOfPropertyChange(() => Minutes);
			}
		}

		private int seconds = 0;
		public int Seconds {
			get { return seconds; }
			set {
				if (value == seconds)
					return;
				seconds = value;
				NotifyOfPropertyChange(() => Seconds);
			}
		}

		private ColorInfo selectedColor;
		public ColorInfo SelectedColor
		{
			get { return selectedColor; }
			set
			{
				if (value == selectedColor)
					return;
				selectedColor = value;
				NotifyOfPropertyChange(() => SelectedColor);
			}
		}

		private string newTimeSliceName = string.Empty;
		public string NewTimeSliceName
		{
			get { return newTimeSliceName; }
			set
			{
				if (value == newTimeSliceName)
					return;
				newTimeSliceName = value;
				NotifyOfPropertyChange(() => NewTimeSliceName);
			}
		}
	}
}
