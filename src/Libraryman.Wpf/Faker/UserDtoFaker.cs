using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Libraryman.Wpf.Dto;

namespace Libraryman.Wpf.Faker
{
	public static class UserDtoFaker

	{
		public static UserDto Generate()
		{
			return Generate(1).First();
		}

		public static IEnumerable<UserDto> Generate(int count)
		{
			return new Faker<UserDto>()
				.Rules((f, u) =>
					{
						u.UserId = f.Random.Int(1000000);
						u.UserName = f.Name.FullName();
						u.UserGender = f.PickRandom<Name.Gender>().ToString();
						u.PhoneNumber = f.Phone.PhoneNumber("+###-#########");
						u.Email = f.Internet.Email();
						u.TotalRecordCount = f.Random.Int(0, 1000);
						u.InHandBooksCount = f.Random.Int(0, 1000);
						u.MembershipType = f.PickRandom(new[] {"Non Member", "Member"});
					}
				)
				.Generate(count);
		}
	}
}