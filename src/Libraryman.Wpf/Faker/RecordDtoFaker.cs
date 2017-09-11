using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Libraryman.Entity;
using Libraryman.Wpf.Dto;
using Libraryman.Wpf.Issue;

namespace Libraryman.Wpf.Faker
{
	public static class RecordDtoFaker
	{
		public static RecordDto Generate()
		{
			return Generate(1).First();
		}

		public static IEnumerable<RecordDto> Generate(int count)
		{
			return new Faker<RecordDto>()
				.Rules((f, r) =>
				{
					r.RecordId = f.Random.Int(0, 1000);
					r.RecordType = f.PickRandom<RecordType>();
					r.Timestamp = f.Date.Recent();
					r.StaffId = f.Random.Int(1000, 9999);
					r.StaffName = f.Name.FullName();
					r.BookBarcode = f.Random.Int(0, 10000);
					r.BookISBN = f.Random.ReplaceNumbers("###-##-#####-##-#");
					r.BookTitle = f.Lorem.Sentence();
					r.BookEdition = f.PickRandom(new List<string>()
					{
						"1st Edition",
						"2nd Edition",
						"3rd Edition",
						"4th Edition",
						"5th Edition"
					});
					r.BookPrice = f.Random.Decimal(0, 100).ToString("0.00");
					r.BookDescription = f.Lorem.Paragraphs();
					r.BookPublishedYear = f.Date.Past().Year.ToString();
					r.BookStatus = f.PickRandom<BookStatus>().ToString();
					r.BookType = f.PickRandom(new List<string>() {"Reference", "Normal"});
					r.PublisherName = f.Company.CompanyName();
				})
				.Generate(count);
		}
	}
}