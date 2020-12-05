using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	[Serializable]
	public class BuildSystemProjectSetting
	{
		[SerializeField] private BuildTarget m_buildTarget = default;
		public BuildTarget BuildTarget {
			get
			{
				return m_buildTarget; 
			}
			set
			{
				m_buildTarget = value;
			}
		}

		public static string GetFilePath {
			get
			{
				return Application.dataPath.GetUpperDirectory() + @"\ProjectSettings\BuildSystemSetting.cq";
			}
		}

		public static string GetFolderPath {
			get
			{
				return GetFilePath.GetUpperDirectory();
			}
		}
		
		private static BuildSystemProjectSetting Get()
		{
			if (!new FileInfo(GetFilePath).Exists)
			{
				BuildSystemProjectSetting m_context = new BuildSystemProjectSetting();


				File.WriteAllText(GetFilePath, JsonUtility.ToJson(m_context));
				Debug.Log("새 세팅파일이 생성되었습니다.");

				return m_context;
			}
			else
			{
				return JsonUtility.FromJson<BuildSystemProjectSetting>(File.ReadAllText(GetFilePath));
			}
		}

		public static BuildSystemProjectSetting Load()
		{
			return Get();
		}

		public void Save()
		{
			File.WriteAllText(GetFilePath, JsonUtility.ToJson(this));
		}
	}
}