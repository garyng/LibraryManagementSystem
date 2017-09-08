using System;
using System.Windows.Markup;

namespace Libraryman.Wpf.Converters
{
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