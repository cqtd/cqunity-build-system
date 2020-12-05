using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class GlobalSettingProvider : SettingsProvider
	{
		private const string SETTING_TITLE = "빌드 시스템";
		
		private const string SETTING_PATH = "/디렉터리";
		private const string SETTING_GENERAL = "/일반";
		
		private static BuildSystemUserSetting m = default;
		private static BuildSystemUserSetting Context {
			get
			{
				if (m == null)
				{
					m = BuildSystemUserSetting.Load();
				}
		
				return m;
			}
		}

		public GlobalSettingProvider(string path, SettingsScope scopes) : base(path, scopes)
		{

		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);

			if (settingsPath == SETTING_TITLE + SETTING_PATH)
			{
				EditorGUILayout.BeginVertical("Box");

				EditorGUI.indentLevel++;
				{
					EditorGUILayout.Space(10);
					Context.m_buildPath = EditorGUILayout.TextField("빌드 아카이브 경로", Context.m_buildPath);
					EditorGUILayout.Space(6);

					using (new EditorGUILayout.HorizontalScope())
					{
						Context.m_useCompanyName = EditorGUILayout.Toggle("회사 이름 포함", Context.m_useCompanyName);
						if (Context.m_useCompanyName)
						{
							PlayerSettings.companyName = EditorGUILayout.TextField(PlayerSettings.companyName);
						}
					}
					
					using (new EditorGUILayout.HorizontalScope())
					{
						Context.m_useProductName = EditorGUILayout.Toggle("제품 이름 포함", Context.m_useProductName);
						if (Context.m_useProductName)
						{
							PlayerSettings.productName = EditorGUILayout.TextField(PlayerSettings.productName);
						}
					}
					
					using (new EditorGUILayout.HorizontalScope())
					{
						Context.m_usePlatformName = EditorGUILayout.Toggle("플랫폼 이름 포함", Context.m_usePlatformName);
					}
					
					EditorGUILayout.Space(6);
					using (new EditorGUILayout.HorizontalScope())
					{
						EditorGUILayout.LabelField("전체 경로", Context.GetFullPath());
					}
					
					EditorGUILayout.Space(10);
				}
				EditorGUI.indentLevel--;

				EditorGUILayout.EndVertical();

				if (GUI.changed)
				{
					Context.Save();
					AssetDatabase.SaveAssets();
				}
			}
			else if (settingsPath == SETTING_TITLE)
			{
				EditorGUILayout.BeginVertical("Box");

				GUILayout.Label("YEAH");

				EditorGUILayout.EndVertical();
			}
		}

		private static bool IsAvailable()
		{
			return Context != null;
		}

		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider()
		{
			if (IsAvailable())
			{
				GlobalSettingProvider provider = new GlobalSettingProvider(SETTING_TITLE + SETTING_GENERAL, SettingsScope.User)
				{
					
				};

				return provider;
			}

			return null;
		}

		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider2()
		{
			if (IsAvailable())
			{
				GlobalSettingProvider provider = new GlobalSettingProvider(SETTING_TITLE, SettingsScope.User)
				{
					
				};

				return provider;
			}

			return null;
		}
		
		[SettingsProvider]
		public static SettingsProvider CreateGlobalSettingProvider3()
		{
			if (IsAvailable())
			{
				GlobalSettingProvider provider = new GlobalSettingProvider(SETTING_TITLE + SETTING_PATH, SettingsScope.User)
				{
					
				};

				return provider;
			}

			return null;
		}
	}
}