using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Libraryman.Wpf.Converters
{
	public class DateToDaysFromTodayConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return value;
			DateTime date = ((DateTime) value).Date;
			return Math.Abs((DateTime.Today - date).Days);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DateToDaysFromTodayConverterExtensions : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new DateToDaysFromTodayConverter();
		}
	}
}