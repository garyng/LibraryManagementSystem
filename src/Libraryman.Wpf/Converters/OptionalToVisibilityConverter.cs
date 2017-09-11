using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Optional;

namespace Libraryman.Wpf.Converters
{
	public class OptionalToVisibilityConverter : IValueConverter
	{
		public bool IsReversed { get; set; }

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

	public sealed class OptionalToVisibilityConverterExtension : MarkupExtension
	{
		[ConstructorArgument("isReversed")]
		public bool IsReversed { get; set; }

		public OptionalToVisibilityConverterExtension()
		{
		}

		public OptionalToVisibilityConverterExtension(bool isReversed)
		{
			IsReversed = isReversed;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new OptionalToVisibilityConverter(IsReversed);
		}
	}
}