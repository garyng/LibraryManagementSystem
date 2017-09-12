using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Libraryman.Wpf.Faker;
using Libraryman.Wpf.Issue;

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