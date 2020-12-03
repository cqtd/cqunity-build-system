using UnityEditor;

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
	}
}