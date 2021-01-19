using UnityEditor;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	internal class BuildLog
	{
		private BuildTarget target;
		private bool enable;
			
		public BuildLog(BuildTarget target, bool enable)
		{
			this.target = target;
			this.enable = enable;
		}

		public void Verbose(string msg)
		{
			if (!enable) return;
				
			Debug.Log($"[{target}] {msg}");
		}
			
		public void Success(string msg)
		{
			if (!enable) return;
				
			Debug.Log($"<color=green>[{target}] {msg}</color>");
		}

		public void Warn(string msg)
		{
			if (!enable) return;
				
			Debug.Log($"<color=yellow>[{target}] {msg}</color>");
		}

		public void Fatal(string msg)
		{
			if (!enable) return;
				
			Debug.Log($"<color=red>[{target}] {msg}</color>");
		}
	}
}