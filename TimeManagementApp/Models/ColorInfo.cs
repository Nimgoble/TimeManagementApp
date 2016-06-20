using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeManagementApp.Models 
{
	public class ColorInfo 
	{

		public ColorInfo()
		{
			this.colorName = "Invalid Color";
			this.color = Colors.AliceBlue;
			brush = new SolidColorBrush(color);
		}

		public ColorInfo(string colorName, Color color)
		{
			this.colorName = colorName;
			this.color = color;
			brush = new SolidColorBrush(color);
		}

		private string colorName = string.Empty;
		public String ColorName 
		{
			get { return colorName; }
		}

		private Color color;
		public Color Color 
		{
			get { return color; }
		}

		private SolidColorBrush brush;
		public SolidColorBrush Brush 
		{
			get { return brush; }
		}

        public override bool Equals(object obj)
        {
            if(obj is ColorInfo)
            {
                var c = (ColorInfo)obj;
                return c.Color == this.Color;
            }
            return false;
        }

        public static List<ColorInfo> GetColorInfos()
		{
			var colorQuery =
			from PropertyInfo property in typeof(Colors).GetProperties()
			orderby property.Name
			select new ColorInfo
			(
				property.Name,
				(Color)property.GetValue(null, null)
			);
			return colorQuery.ToList();
		}

        private static Random randomColorSeed = new Random();
        public static ColorInfo GetRandomColor()
        {
            var colorInfos = ColorInfo.GetColorInfos();
            int colorIndex = randomColorSeed.Next(colorInfos.Count);
            return colorInfos[colorIndex];
        }
	}
}
