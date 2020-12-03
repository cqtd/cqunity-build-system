using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	[Serializable]
	public class GlobalSettingContext : IEquatable<GlobalSettingContext>
	{
		/// <summary>
		/// Shipping directory
		/// </summary>
		[SerializeField] public string m_buildPath = default;

		/// <summary>
		/// Shipping directory 경로에 Company 이름 포함
		/// </summary>
		[SerializeField] public bool m_useCompanyName = default;

		/// <summary>
		/// Shipping directory 경로에 Product 이름 포함
		/// </summary>
		[SerializeField] public bool m_useProductName = default;

		/// <summary>
		/// Shipping directory 경로에 Platform 이름 포함
		/// </summary>
		[SerializeField] public bool m_usePlatformName = default;
		
		/// <summary>
		/// 경로를 가져옵니다.
		/// </summary>
		/// <returns>생성된 빌드 경로 루트 디렉토리</returns>
		public string GetFullPath()
		{
			StringBuilder sb = new StringBuilder(200);

			sb.Append(m_buildPath);
			sb.Append("/");

			if (m_useCompanyName)
			{
				sb.Append(PlayerSettings.companyName);
				sb.Append("/");
			}

			if (m_useProductName)
			{
				sb.Append(PlayerSettings.productName);
				sb.Append("/");	
			}

			if (m_usePlatformName)
			{
				sb.Append(EditorUserBuildSettings.activeBuildTarget);
				sb.Append("/");
			}

			return sb.ToString();
		}

		#region IEquatable

		public bool Equals(GlobalSettingContext other)
		{
			if (this.m_buildPath != other?.m_buildPath) return false;
			if (this.m_useCompanyName != other?.m_useCompanyName) return false;

			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((GlobalSettingContext) obj);
		}

		public override int GetHashCode()
		{
			return (m_buildPath + m_useCompanyName).GetHashCode();
		}

		#endregion
		
		public GlobalSettingContext()
		{
			m_buildPath = DEFAULT_BUILD_PATH;
			m_useCompanyName = DEFAULT_USE_COMPANY_NAME;
			m_useProductName = DEFAULT_USE_PRODUCT_NAME;
		}
		
		private const string DEFAULT_BUILD_PATH = "D:/Unity Engine Build";
		private const bool DEFAULT_USE_COMPANY_NAME = false;
		private const bool DEFAULT_USE_PRODUCT_NAME = true;

		private static string m_settingRootPath = Application.persistentDataPath.GetUpperDirectory(2);
		private static string m_settingDirectory = m_settingRootPath + @"\Cqunity\BuildSetting.cqu";
		
		public static string SettingPath {
			get { return m_settingDirectory; }
		}

		private static GlobalSettingContext Get()
		{
			if (!new FileInfo(m_settingDirectory).Exists)
			{
				GlobalSettingContext m_context = new GlobalSettingContext();

				DirectoryInfo directory = new DirectoryInfo(m_settingRootPath + @"\Cqunity");
				if (!directory.Exists)
				{
					directory.Create();
				}

				File.WriteAllText(m_settingDirectory, JsonUtility.ToJson(m_context));
				Debug.Log("새 세팅파일이 생성되었습니다.");

				return m_context;
			}
			else
			{
				return JsonUtility.FromJson<GlobalSettingContext>(File.ReadAllText(m_settingDirectory));
			}
		}

		public static GlobalSettingContext Load()
		{
			GlobalSettingContext m_context = Get();

			return m_context;
		}

		public void Save()
		{
			File.WriteAllText(m_settingDirectory, JsonUtility.ToJson(this));
		}
	}
}