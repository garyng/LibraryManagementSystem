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

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for IsEmptyMessageControl.xaml
	/// </summary>
	public partial class IsEmptyMessageControl : UserControl
	{
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
			"Message", typeof(string), typeof(IsEmptyMessageControl), new PropertyMetadata(default(string)));

		public string Message
		{
			get { return (string) GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}
		public IsEmptyMessageControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignIsEmptyMessageControl
	{
		public string Message { get; set; } = "Much emptiness... Why not add something here?";
	}
#endif
}
