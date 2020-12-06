using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cqunity.BuildSystem
{
	public class BuildSystemProjectCallbacksProvider : SettingsProvider
	{
		private static BuildSystemProjectCallbacks m_setting = default;
		
		public BuildSystemProjectCallbacksProvider(string path, SettingsScope scopes) : base(path, scopes)
		{
			
		}

		[SettingsProvider]
		public static SettingsProvider CreateSettingProvider()
		{
			if (m_setting == null)
			{
				m_setting = BuildSystemProjectCallbacks.GetSetting();

				if (m_setting != null)
				{
					BuildSystemProjectCallbacksProvider provider = 
						new BuildSystemProjectCallbacksProvider("Project/Callbacks", SettingsScope.Project)
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

			if (m_setting == null)
				return;

			DrawArray("Pre Process", ref m_setting.m_preprocess);
			DrawArray("Post Process", ref m_setting.m_postprocess);
		}

		private void DrawArray<T>(string name, ref T[] list) where T : Object
		{
			int count = list.Length;
			int newCount;
			
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label(name);
				
				GUILayout.FlexibleSpace();
				
				newCount = EditorGUILayout.IntField("", count);
				if (count != newCount)
				{
					Array.Resize(ref list, newCount);
				}	
			}

			EditorGUI.indentLevel += 3;
			
			for (int i = 0; i < newCount; i++)
			{
				list[i] = (T) EditorGUILayout.ObjectField((Object) list[i], typeof(T), false);
			}
			
			EditorGUI.indentLevel -= 3;

			if (GUI.changed)
			{
				m_setting.Save();
			}

			if (newCount > 0)
			{
				EditorGUILayout.Space(10);
			}
		}
	}
}