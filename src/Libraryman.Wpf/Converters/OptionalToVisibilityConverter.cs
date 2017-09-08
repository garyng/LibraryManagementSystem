using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Optional;

namespace Libraryman.Wpf.Converters
{
	public class OptionalToVisibilityConverter : IValueConverter
	{
		public bool IsReversed { get; set; }

		public OptionalToVisibilityConverter()
		{
		}

		public OptionalToVisibilityConverter(bool isReversed)
		{
			this.IsReversed = isReversed;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return Visibility.Collapsed;

			Option<bool> optional = (Option<bool>) value;
			return optional.Match(
				some: v =>
				{
					v = IsReversed ? !v : v;
					return v ? Visibility.Visible : Visibility.Collapsed;
				},
				none: () => Visibility.Collapsed);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}