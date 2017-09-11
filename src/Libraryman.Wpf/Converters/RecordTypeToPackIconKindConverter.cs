using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Libraryman.Entity;
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Converters
{
	public class RecordTypeToPackIconKindConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return value;
			RecordType type = (RecordType) value;
			switch (type)
			{
				case RecordType.Issue:
					return PackIconKind.DebugStepOut;
				case RecordType.Return:
					return PackIconKind.DebugStepInto;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class RecordTypeToPackIconKindConverterExtensions : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new RecordTypeToPackIconKindConverter();
		}
	}
}