using System.IO;
using System.Security.AccessControl;

namespace Cqunity.BuildSystem
{
	internal static class DirectoryHelper
	{
		public static void CreateIfNotExist(this DirectoryInfo di, DirectorySecurity directorySecurity = null)
		{
			if (!di.Exists)
			{
				di.Create(directorySecurity);
			}
		}
	}
}