using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class LauncherTool
	{
		[MenuItem("Build/Run/Standalone x64")]
		private static void RunLatestStandAlone()
		{
			string path = $"{Application.dataPath}/win64/bin/{Wildcard.ProjectName}.exe".Replace("/Assets", "");
			
			Process launcher = new Process {StartInfo = {FileName = path}};
			launcher.Start();
		}
		
		[MenuItem("Build/Run/Standalone x64",true)]
		private static bool ValidateRunLatestStandAlone()
		{
			string path = $"{Application.dataPath}/win64/bin/{Wildcard.ProjectName}.exe".Replace("/Assets", "");
			
			return new FileInfo(path).Exists;
		}
	}
}