using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	/// <summary>
	/// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/LogEntries.bindings.cs
	/// </summary>
	public static class Console
	{
		private const string menuName = "Tools/Console/Clear #&c";

		private const string assemblyName = "UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

		private const string className = "LogEntries";
		private const string methodName = "Clear";

		private static MethodInfo method = null;

		[MenuItem(menuName)]
		public static void Clear()
		{
			try
			{
				if (method == null)
				{
					method = GetMethod();
				}

				method.Invoke(null, null);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

		private static MethodInfo GetMethod()
		{
			// UnityEditor.LogEntries.Clear();
			
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.First(e => e.FullName == assemblyName)
				.GetTypes().First(e => e.Name == className)
				.GetMethod(methodName);
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void DomainReset()
		{
			method = null;
		}
	}
}