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
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Faker;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for BorrowedBookDtoControl.xaml
	/// </summary>
	public partial class BorrowedBookDtoControl : UserControl
	{
		public static readonly DependencyProperty BorrowedBookDtoProperty = DependencyProperty.Register(
			"BorrowedBookDto", typeof(BorrowedBookDto), typeof(BorrowedBookDtoControl),
			new PropertyMetadata(default(BorrowedBookDto)));

		public BorrowedBookDto BorrowedBookDto
		{
			get { return (BorrowedBookDto) GetValue(BorrowedBookDtoProperty); }
			set { SetValue(BorrowedBookDtoProperty, value); }
		}

		public BorrowedBookDtoControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignBorrowedBookDtoControl
	{
		public BorrowedBookDto BorrowedBookDto { get; set; } = BorrowedBookDtoFaker.Generate();
	}

#endif
}