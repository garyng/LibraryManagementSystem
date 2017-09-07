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
using ControlzEx;
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for ReportCardControl.xaml
	/// </summary>
	public partial class ReportCardControl : Card
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
			"Title", typeof(string), typeof(ReportCardControl), new PropertyMetadata(default(string)));

		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(
			"Subtitle", typeof(string), typeof(ReportCardControl), new PropertyMetadata(default(string)));

		public string Subtitle
		{
			get { return (string) GetValue(SubtitleProperty); }
			set { SetValue(SubtitleProperty, value); }
		}

		public static readonly DependencyProperty PackIconKindProperty = DependencyProperty.Register(
			"PackIconKind", typeof(PackIconKind), typeof(ReportCardControl), new PropertyMetadata(default(PackIconKind)));

		public PackIconKind PackIconKind
		{
			get { return (PackIconKind) GetValue(PackIconKindProperty); }
			set { SetValue(PackIconKindProperty, value); }
		}

		public static readonly DependencyProperty ColorZoneModeProperty = DependencyProperty.Register(
			"ColorZoneMode", typeof(ColorZoneMode), typeof(ReportCardControl), new PropertyMetadata(default(ColorZoneMode)));

		public ColorZoneMode ColorZoneMode
		{
			get { return (ColorZoneMode) GetValue(ColorZoneModeProperty); }
			set { SetValue(ColorZoneModeProperty, value); }
		}

		public ReportCardControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DebugReportCardControl
	{
		public string Title { get; set; } = "Title";
		public string Subtitle { get; set; } = "Subtitle";
		public PackIconKind PackIconKind { get; set; } = PackIconKind.Book;
		public ColorZoneMode ColorZoneMode { get; set; } = ColorZoneMode.PrimaryDark;
	}

#endif
}