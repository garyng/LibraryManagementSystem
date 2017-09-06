using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Libraryman.Common.Extensions
{
	public static class SHA256Extensions
	{
		public static string ToSHA256(this string source)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				return string.Concat(sha256
					.ComputeHash(Encoding.UTF8.GetBytes(source))
					.Select(b => b.ToString("x")));
			}
		}
	}
}