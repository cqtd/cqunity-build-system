namespace Cqunity.BuildSystem
{
	internal static class DirectoryManagement
	{
		private static string GetUpperDirectoryInternal(this string path)
		{
			int lastIndex = path.LastIndexOf('/');
			return path.Substring(0, lastIndex);
		}

		internal static string GetUpperDirectory(this string path, ushort count = 1)
		{
			while (count > 0)
			{
				count--;
				path = path.GetUpperDirectoryInternal();
			}

			return path;
		}
	}
}