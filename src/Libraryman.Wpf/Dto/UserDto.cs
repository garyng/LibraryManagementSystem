using System.Linq;
using Libraryman.Entity;

namespace Libraryman.Wpf.Dto
{
	public class UserDto
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserGender { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string MembershipType { get; set; }
		public int TotalRecordCount { get; set; }
		public int InHandBooksCount { get; set; }

		public UserDto()
		{
		}

		public UserDto(User user)
		{
			UserId = user.Id;
			UserName = user.Name;
			UserGender = user.Gender.ToString();
			PhoneNumber = user.PhoneNumber;
			Email = user.Email;
			MembershipType = user.Type.Name;
			// todo: figure out how to calculate the total books borrowed -> all returned books + all havent returned book
			TotalRecordCount = user.Records.Count(r => r.Type == RecordType.Return) + user.BorrowedBooks.Count;
			InHandBooksCount = user.BorrowedBooks.Count;
		}
	}
}