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
using Libraryman.Entity;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Faker;
using Libraryman.Wpf.UserInfo;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for RecordDtoInfoControl.xaml
	/// </summary>
	public partial class RecordDtoControl : UserControl
	{
		public static readonly DependencyProperty RecordDtoProperty = DependencyProperty.Register(
			"RecordDto", typeof(RecordDto), typeof(RecordDtoControl), new PropertyMetadata(default(RecordDto)));

		public RecordDto RecordDto
		{
			get { return (RecordDto) GetValue(RecordDtoProperty); }
			set { SetValue(RecordDtoProperty, value); }
		}

		public RecordDtoControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignRecordDtoInfoControl
	{
		public RecordDto RecordDto { get; set; } = RecordDtoFaker.Generate();
	}

#endif
}