using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Libraryman.Wpf.Extensions
{
	public static class SecureStringExtensions
	{
		/// <summary>
		/// Convert a <see cref="SecureString"/> to <see cref="string"/>
		/// </summary>
		/// <param name="ss"></param>
		/// <returns></returns>
		public static string ConvertToString(this SecureString ss)
		{
			IntPtr bstr = Marshal.SecureStringToBSTR(ss);
			try
			{
				return Marshal.PtrToStringBSTR(bstr);
			}
			finally
			{
				Marshal.FreeBSTR(bstr);
			}
		}

		/// <summary>
		/// Convert a <see cref="string"/> to <see cref="SecureString"/>
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static SecureString ConvertFromString(this string s)
		{
			return s.Aggregate(new SecureString(), (SecureString ss, char c) =>
			{
				ss.AppendChar(c);
				return ss;
			}, (SecureString ss) =>
			{
				ss.MakeReadOnly();
				return ss;
			});
		}
	}
}