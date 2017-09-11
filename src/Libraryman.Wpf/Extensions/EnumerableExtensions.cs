using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Libraryman.Wpf.Extensions
{
	public static class EnumerableExtensions
	{
		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
		{
			return new ObservableCollection<T>(enumerable);
		}
	}
}