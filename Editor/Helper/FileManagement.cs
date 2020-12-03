using System.IO;

namespace Cqunity.BuildSystem
{
	internal static class FileManagement
	{
		internal static void CopyFilesRecursive(DirectoryInfo source, DirectoryInfo target, string blacklist = null) {
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!string.IsNullOrEmpty(blacklist) && dir.FullName.Contains(blacklist))
					continue;
				
				CopyFilesRecursive(dir, target.CreateSubdirectory(dir.Name));
			}
			
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
		}
	}
}