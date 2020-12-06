using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class BuildSystemUserSettingProvider : SettingsProvider
	{
		private const string SETTING_TITLE = "빌드 시스템";
		
		private const string SETTING_PATH = "/디렉터리";
		private const string SETTING_GENERAL = "/일반";

		private static BuildSystemUserSetting setting = default;

		public BuildSystemUserSettingProvider(string path, SettingsScope scopes) : base(path, scopes)
		{

		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);

			switch (settingsPath)
			{
				case SETTING_TITLE + SETTING_PATH: DrawDirectory(); break;
				case SETTING_TITLE + SETTING_GENERAL: DrawGeneral(); break;
				default: DrawTitle(); break;
			}
		}

		private void DrawDirectory()
		{
			EditorGUILayout.BeginVertical("Box");

			EditorGUI.indentLevel++;
			{
				EditorGUILayout.Space(10);
				setting.m_buildPath = EditorGUILayout.TextField("빌드 아카이브 경로", setting.m_buildPath);
				EditorGUILayout.Space(6);

				using (new EditorGUILayout.HorizontalScope())
				{
					setting.m_useCompanyName = EditorGUILayout.Toggle("회사 이름 포함", setting.m_useCompanyName);
					if (setting.m_useCompanyName)
					{
						PlayerSettings.companyName = EditorGUILayout.TextField(PlayerSettings.companyName);
					}
				}
					
				using (new EditorGUILayout.HorizontalScope())
				{
					setting.m_useProductName = EditorGUILayout.Toggle("제품 이름 포함", setting.m_useProductName);
					if (setting.m_useProductName)
					{
						PlayerSettings.productName = EditorGUILayout.TextField(PlayerSettings.productName);
					}
				}
					
				using (new EditorGUILayout.HorizontalScope())
				{
					setting.m_usePlatformName = EditorGUILayout.Toggle("플랫폼 이름 포함", setting.m_usePlatformName);
				}
					
				EditorGUILayout.Space(6);
				using (new EditorGUILayout.HorizontalScope())
				{
					EditorGUILayout.LabelField("전체 경로", setting.GetFullPath());
				}
					
				EditorGUILayout.Space(10);
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();

			if (GUI.changed)
			{
				setting.Save();
				AssetDatabase.SaveAssets();
			}
		}

		private void DrawTitle()
		{
			EditorGUILayout.BeginVertical("Box");

			GUILayout.Label("YEAH");

			EditorGUILayout.EndVertical();
		}

		private void DrawGeneral()
		{
			
		}
		
		private static bool IsAvailable()
		{
			if (setting == null)
			{
				setting = BuildSystemUserSetting.Load();
			}
			
			return setting != null;
		}

		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider()
		{
			if (IsAvailable())
			{
				BuildSystemUserSettingProvider provider = new BuildSystemUserSettingProvider(SETTING_TITLE + SETTING_GENERAL, SettingsScope.User);

				return provider;
			}

			return null;
		}

		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider2()
		{
			if (IsAvailable())
			{
				BuildSystemUserSettingProvider provider = new BuildSystemUserSettingProvider(SETTING_TITLE, SettingsScope.User);

				return provider;
			}

			return null;
		}
		
		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider3()
		{
			if (IsAvailable())
			{
				BuildSystemUserSettingProvider provider = new BuildSystemUserSettingProvider(SETTING_TITLE + SETTING_PATH, SettingsScope.User);

				return provider;
			}

			return null;
		}
	}
}