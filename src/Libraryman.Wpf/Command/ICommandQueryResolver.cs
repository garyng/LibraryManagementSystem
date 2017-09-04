namespace Libraryman.Wpf.Command
{
	/// <summary>
	/// Act as a bridge to AutoFac
	/// </summary>
	public interface ICommandQueryResolver
	{
		T Resolve<T>();
	}
}