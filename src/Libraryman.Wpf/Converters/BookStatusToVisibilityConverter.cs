using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Libraryman.Entity;

namespace Libraryman.Wpf.Converters
{
	public class BookStatusToVisibilityConverter : IValueConverter
	{
		public bool IsReversed { get; set; }

		public BookStatusToVisibilityConverter(bool isReversed)
		{
			IsReversed = isReversed;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return value;
			BookStatus status = (BookStatus) value;
			bool visible = status == BookStatus.Available;
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

	public sealed class BookStatusToVisibilityConverterExtension : MarkupExtension
	{
		[ConstructorArgument("isReversed")]
		public bool IsReversed { get; set; }

		public BookStatusToVisibilityConverterExtension()
		{
		}

		public BookStatusToVisibilityConverterExtension(bool isReversed)
		{
			IsReversed = isReversed;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new BookStatusToVisibilityConverter(IsReversed);
		}
	}

}