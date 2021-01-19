using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class Wildcard
	{
		public static string ProjectName {
			get
			{
				return PlayerSettings.productName;
			}
		}
	
		public static string ProjectRoot {
			get
			{
				return Application.dataPath.Replace("/Assets","");
			}
		}
	}
}