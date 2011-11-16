using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Dotjosh.IRobotOnCLR.Client.Silverlight.Converters
{
	public class BumpToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
				return new SolidColorBrush(Colors.Red);
			return new SolidColorBrush(Colors.Transparent);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}