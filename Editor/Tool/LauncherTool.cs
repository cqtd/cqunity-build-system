using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class LauncherTool
	{
		[MenuItem("Run/Standalone x64")]
		static void RunLatestStandAlone()
		{
			string path = $"{Application.dataPath}/win64/bin/{Wildcard.ProjectName}.exe".Replace("/Assets", "");
			
			Process launcher = new Process {StartInfo = {FileName = path}};
			launcher.Start();
		}
	}
}