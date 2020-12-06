using UnityEngine;

namespace Cqunity.BuildSystem
{
	internal static class PathManagement
	{
		public static string ProjectRoot {
			get
			{
				return Application.dataPath.GetUpperDirectory();
			}
		}
	}
}