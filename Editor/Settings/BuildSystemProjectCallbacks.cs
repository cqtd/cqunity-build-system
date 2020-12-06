using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	[Serializable]
	public class BuildSystemProjectCallbacks
	{
		private const string PATH = @"\ProjectSettings\BuildSystemCallbacks.asset";

		public CallbackBase[] m_preprocess;
		public CallbackBase[] m_postprocess;

		public BuildSystemProjectCallbacks()
		{
			m_preprocess = new CallbackBase[0];
			m_postprocess = new CallbackBase[0];
		}
		
		public static void Create()
		{
			BuildSystemProjectCallbacks profile = new BuildSystemProjectCallbacks();
			string json = JsonUtility.ToJson(profile);
			
			File.WriteAllText(PathManagement.ProjectRoot + PATH, json);
		}

		public static BuildSystemProjectCallbacks GetSetting()
		{
			if (!new FileInfo(PathManagement.ProjectRoot + PATH).Exists)
			{
				Create();
			}
			
			BuildSystemProjectCallbacks profile =
				JsonUtility.FromJson<BuildSystemProjectCallbacks>(File.ReadAllText(PathManagement.ProjectRoot + PATH));

			return profile;
		}

		public void Save()
		{
			string json = JsonUtility.ToJson(this);
			File.WriteAllText(PathManagement.ProjectRoot + PATH, json);
		}
	}
}