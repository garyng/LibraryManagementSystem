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
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for SearchControl.xaml
	/// </summary>
	public partial class SearchControl : UserControl
	{
		public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
			"Hint", typeof(string), typeof(SearchControl), new PropertyMetadata(default(string)));

		public string Hint
		{
			get { return (string) GetValue(HintProperty); }
			set { SetValue(HintProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(SearchControl),
			new FrameworkPropertyMetadata()
			{
				BindsTwoWayByDefault = true,
				DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
			});

		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}


		public static readonly DependencyProperty PackIconKindProperty = DependencyProperty.Register(
			"PackIconKind", typeof(PackIconKind), typeof(SearchControl), new PropertyMetadata(default(PackIconKind)));

		public PackIconKind PackIconKind
		{
			get { return (PackIconKind) GetValue(PackIconKindProperty); }
			set { SetValue(PackIconKindProperty, value); }
		}

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command", typeof(ICommand), typeof(SearchControl), new PropertyMetadata(default(ICommand)));

		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public SearchControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignSearchControl
	{
		public string Hint { get; set; }
		public string Text { get; set; }
		public PackIconKind PackIconKind { get; set; }
		public ICommand Command { get; set; }

		public DesignSearchControl()
		{
			Hint = "Search barcode...";
			Text = "123";
			PackIconKind = PackIconKind.Magnify;
		}
	}

#endif
}