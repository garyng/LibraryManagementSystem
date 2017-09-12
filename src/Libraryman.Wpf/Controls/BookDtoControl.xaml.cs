using System.Windows;
using System.Windows.Controls;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Faker;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for BookDtoControl.xaml
	/// </summary>
	public partial class BookDtoControl : UserControl
	{
		public static readonly DependencyProperty BookDtoProperty = DependencyProperty.Register(
			"BookDto", typeof(BookDto), typeof(BookDtoControl), new PropertyMetadata(default(BookDto)));

		public BookDto BookDto
		{
			get { return (BookDto) GetValue(BookDtoProperty); }
			set { SetValue(BookDtoProperty, value); }
		}

		public BookDtoControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignBookDtoControl
	{
		public BookDto BookDto { get; set; } = BookDtoFaker.Generate();
	}

#endif
}