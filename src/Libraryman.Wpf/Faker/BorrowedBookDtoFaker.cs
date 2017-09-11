using System.Collections.Generic;
using System.Linq;
using Bogus;
using Libraryman.Wpf.Dto;

namespace Libraryman.Wpf.Faker
{
	public static class BorrowedBookDtoFaker
	{
		public static BorrowedBookDto Generate()
		{
			return Generate(1).First();
		}

		public static IEnumerable<BorrowedBookDto> Generate(int count)
		{
			return new Faker<BorrowedBookDto>()
				.Rules((f, bb) =>
				{
					bb.BookBarcode = f.Random.Int(0, 10000);
					bb.BookISBN = f.Random.ReplaceNumbers("###-##-#####-##-#");
					bb.BookTitle = f.Lorem.Sentence();
					bb.BookEdition = f.PickRandom(new List<string>()
					{
						"1st Edition",
						"2nd Edition",
						"3rd Edition",
						"4th Edition",
						"5th Edition"
					});
					bb.PublishedYear = f.Date.Past().Year.ToString();
					bb.UserId = f.Random.Int(1000000, 9999999);
					bb.UserName = f.Name.FullName();
					bb.UserType = f.PickRandom(new List<string>()
					{
						"Member",
						"Non Member"
					});
					bb.RecordId = f.Random.Int(0, 1000);
					bb.BorrowedDate = f.Date.Recent();
					bb.DueDate = f.Date.Recent(20);
					bb.IsOverdue = true;
					bb.OverdueFine = f.Random.Decimal(0, 10);
				})
				.Generate(count);
		}
	}
}