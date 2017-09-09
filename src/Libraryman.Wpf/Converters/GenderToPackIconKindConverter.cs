using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Converters
{
	public class GenderToPackIconKindConverterExtension: MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new GenderToPackIconKindConverter();
		}
	}

	public class GenderToPackIconKindConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return value;
			string str = (string) value;
			if (str == "Female")
			{
				return PackIconKind.GenderFemale;
			}
			return PackIconKind.GenderMale;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}