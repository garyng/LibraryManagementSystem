using System.Collections.Generic;
using System.Linq;
using Bogus;
using Libraryman.Entity;
using Libraryman.Wpf.Issue;

namespace Libraryman.Wpf.Faker
{
	public static class BookDtoFaker
	{
		public static BookDto Generate()
		{
			return Generate(1).First();
		}

		public static IEnumerable<BookDto> Generate(int count)
		{
			return new Faker<BookDto>()
				.Rules((f, b) =>
				{
					b.BookBarcode = f.Random.Int(0, 10000);
					b.BookTitle = f.Lorem.Sentence();
					b.BookISBN = f.Random.ReplaceNumbers("###-##-#####-##-#");
					b.BookEdition = f.PickRandom(new List<string>()
					{
						"1st Edition",
						"2nd Edition",
						"3rd Edition",
						"4th Edition",
						"5th Edition"
					});
					b.BookPrice = f.Random.Decimal(0, 100);
					b.BookDescription = f.Lorem.Paragraphs();
					b.BookPublishedYear = f.Date.Past().Year.ToString();
					b.BookStatus = f.PickRandom<BookStatus>();
					b.BookType = f.PickRandom(new List<string>()
					{
						"Reference",
						"Normal"
					});
					b.IsBookBorrowable = f.Random.Bool();
					b.PublisherName = f.Company.CompanyName();
					b.LibraryName = $"Ohaiko Central Library ({f.Address.City()})";
					b.AuthorNames = f.Make(10, () => f.Name.FullName());
				})
				.Generate(count);
		}
	}
}