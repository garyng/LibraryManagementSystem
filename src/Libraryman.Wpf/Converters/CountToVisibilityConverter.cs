using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Libraryman.Wpf.Converters
{
	public class CountToVisibilityConverter : IValueConverter
	{
		public bool IsReversed { get; set; }

		public CountToVisibilityConverter(bool isReversed)
		{
			IsReversed = isReversed;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return value;
			int count = (int) value;
			bool visible = count > 0;
			if (IsReversed)
			{
				visible = !visible;
			}
			return visible ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class CollectionCountToVisibilityConverterExtension : MarkupExtension
	{
		[ConstructorArgument("isReversed")]
		public bool IsReversed { get; set; }

		public CollectionCountToVisibilityConverterExtension()
		{
			
		}
		public CollectionCountToVisibilityConverterExtension(bool isReversed)
		{
			IsReversed = isReversed;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new CountToVisibilityConverter(IsReversed);
		}
	}
}