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
using MaterialDesignThemes.Wpf;

namespace Libraryman.Wpf.Controls
{
	/// <summary>
	/// Interaction logic for UserDtoControl.xaml
	/// </summary>
	public partial class UserDtoControl : UserControl
	{
		public static readonly DependencyProperty UserDtoProperty = DependencyProperty.Register(
			"UserDto", typeof(UserDto), typeof(UserDtoControl), new PropertyMetadata(default(UserDto)));

		public UserDto UserDto
		{
			get { return (UserDto) GetValue(UserDtoProperty); }
			set { SetValue(UserDtoProperty, value); }
		}

		public static readonly DependencyProperty BackgroundColorZoneModeProperty = DependencyProperty.Register(
			"BackgroundColorZoneMode", typeof(ColorZoneMode), typeof(UserDtoControl), new PropertyMetadata(default(ColorZoneMode)));

		public ColorZoneMode BackgroundColorZoneMode
		{
			get { return (ColorZoneMode) GetValue(BackgroundColorZoneModeProperty); }
			set { SetValue(BackgroundColorZoneModeProperty, value); }
		}

		public static readonly DependencyProperty MemberBadgeColorZoneModeProperty = DependencyProperty.Register(
			"MemberBadgeColorZoneMode", typeof(ColorZoneMode), typeof(UserDtoControl), new PropertyMetadata(default(ColorZoneMode)));

		public ColorZoneMode MemberBadgeColorZoneMode
		{
			get { return (ColorZoneMode) GetValue(MemberBadgeColorZoneModeProperty); }
			set { SetValue(MemberBadgeColorZoneModeProperty, value); }
		}
		public UserDtoControl()
		{
			InitializeComponent();
		}
	}

#if DEBUG

	public class DesignUserDtoControl
	{
		public UserDto UserDto { get; set; } = new UserDto()
		{
			UserId = 100001,
			Email = "email@email.com",
			InHandBooksCount = 1000,
			MembershipType = "Non Member",
			PhoneNumber = "+60 123 123 123",
			TotalRecordCount = 21234,
			UserGender = "Female",
			UserName = "Testing"
		};

		public ColorZoneMode BackgroundColorZoneMode { get; set; } = ColorZoneMode.Accent;
		public ColorZoneMode MemberBadgeColorZoneMode { get; set; } = ColorZoneMode.PrimaryDark;

	}

#endif
}