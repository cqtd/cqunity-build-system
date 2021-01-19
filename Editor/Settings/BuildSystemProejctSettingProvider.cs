using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class BuildSystemProejctSettingProvider : SettingsProvider
	{
		private static BuildSystemProjectSetting m_context = default;
		
		public BuildSystemProejctSettingProvider(string path, SettingsScope scopes) : base(path, scopes)
		{
			
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingProvider()
		{
			if (m_context == null)
			{
				m_context =  BuildSystemProjectSetting.Load();

				if (m_context != null)
				{
					BuildSystemProejctSettingProvider provider = 
						new BuildSystemProejctSettingProvider("Project/빌드 시스템", SettingsScope.Project)
					{
						
					};

					return provider;
				}
			}

			return null;
		}

		public override void OnGUI(string searchContext)
		{
			base.OnGUI(searchContext);

			m_context.BuildTarget = (BuildTarget) EditorGUILayout.EnumPopup("사용하는 빌드 타겟", m_context.BuildTarget);
			
			EditorGUILayout.Space(20);

			GUILayout.Label("빌드 설정");
			EditorGUILayout.BeginVertical("Box");
			
			EditorGUILayout.Space(10);

			m_context.UseGlobalCacheDirectory =
				EditorGUILayout.Toggle("글로벌 캐시 디렉터리 사용", m_context.UseGlobalCacheDirectory);

			
			
			m_context.IncreaseVersionAutomatically =
				EditorGUILayout.Toggle("버전 자동 올리기", m_context.IncreaseVersionAutomatically);
			
			
			m_context.SaveIfDirty();
			
			EditorGUILayout.Space(10);
			
			if (m_context.UseGlobalCacheDirectory)
			{
				
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}