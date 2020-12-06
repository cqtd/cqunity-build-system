using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class BuildSystemDirectoryTests
	{
		[MenuItem("Tools/Build System/Tests/Create Directory")]
		static void CreateDirectory()
		{
			var path = BuildSystemUserSetting.Load().GetFullPath();
			path += "/" + VersionManager.GetCurrentBuildVersion();
			
			var di = new DirectoryInfo(path);
			if (!di.Exists)
			{
				di.Create();
			}
		}
	}
}
