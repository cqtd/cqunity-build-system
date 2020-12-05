using UnityEditor;

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
		}
	}
}