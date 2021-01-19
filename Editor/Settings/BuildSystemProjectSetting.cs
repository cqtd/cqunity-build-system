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
		[SerializeField] private bool useGlobalCacheDirectory = default;
		[SerializeField] private bool increaseVersionAutomatically = default;
		[SerializeField] private string globalDirectory = default;
		
		private bool isMarkAsDirty = default;

		public BuildSystemProjectSetting()
		{
			m_buildTarget = EditorUserBuildSettings.activeBuildTarget;

			useGlobalCacheDirectory = true;
			increaseVersionAutomatically = true;
			globalDirectory = "D:/Unity/Build Cache";
		}

		public static string GlobalArchiveDirectory {
			get => Load().globalDirectory;
		}

		public static bool UseGlobalArchive {
			get => Load().useGlobalCacheDirectory;
		}
		
		public BuildTarget BuildTarget {
			get
			{
				return m_buildTarget; 
			}
			set
			{
				m_buildTarget = value;
				isMarkAsDirty = true;
			}
		}

		public bool UseGlobalCacheDirectory {
			get
			{
				return useGlobalCacheDirectory;
			}
			set
			{
				useGlobalCacheDirectory = value;
				isMarkAsDirty = true;
			}
		}
		
		public bool IncreaseVersionAutomatically {
			get
			{
				return increaseVersionAutomatically;
			}
			set
			{
				increaseVersionAutomatically = value;
				isMarkAsDirty = true;
			}
		}
		
		
		public static string GetFilePath {
			get
			{
				return PathManagement.ProjectRoot + @"\ProjectSettings\BuildSystemSetting.asset";
			}
		}

		public static string GetFolderPath {
			get
			{
				return GetFilePath.GetUpperDirectory();
			}
		}

		public static BuildSystemProjectSetting Load()
		{
			if (!new FileInfo(GetFilePath).Exists)
			{
				BuildSystemProjectSetting m_context = new BuildSystemProjectSetting();


				File.WriteAllText(GetFilePath, JsonUtility.ToJson(m_context));
				Debug.Log("새 세팅파일이 생성되었습니다.");

				return m_context;
			}

			return JsonUtility.FromJson<BuildSystemProjectSetting>(File.ReadAllText(GetFilePath));
		}

		public void Save()
		{
			File.WriteAllText(GetFilePath, JsonUtility.ToJson(this));
			isMarkAsDirty = false;
		}

		public void SaveIfDirty()
		{
			if (isMarkAsDirty)
			{
				Save();
			}
		}
	}
}