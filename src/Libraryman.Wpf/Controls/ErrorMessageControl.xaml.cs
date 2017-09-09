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
	/// Interaction logic for ErrorMessageControl.xaml
	/// </summary>
	public partial class ErrorMessageControl : UserControl
	{
		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register(
			"ErrorMessage", typeof(string), typeof(ErrorMessageControl), new PropertyMetadata(default(string)));

		public string ErrorMessage
		{
			get { return (string) GetValue(ErrorMessageProperty); }
			set { SetValue(ErrorMessageProperty, value); }
		}

		public ErrorMessageControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignErrorMessageControl
	{
		public string ErrorMessage { get; set; } = "This is a simple error message. This is another simple error message.";
	}
#endif
}
