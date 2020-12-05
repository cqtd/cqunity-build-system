namespace Cqunity.BuildSystem
{
	public class GlobalSettingContextBridge
	{
		private readonly BuildSystemUserSetting m = default;

		public GlobalSettingContextBridge()
		{
			m = BuildSystemUserSetting.Load();
		}
	}
}