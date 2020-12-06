using UnityEngine;

namespace Cqunity.BuildSystem
{
	public abstract class CallbackBase : ScriptableObject
	{
		public abstract void Execute();
	}
}