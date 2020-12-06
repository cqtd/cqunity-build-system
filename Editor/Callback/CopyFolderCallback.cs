using System.IO;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	
	[CreateAssetMenu(menuName = "Build Callback/Copy Folder Callback", fileName = "CopyFolderCallback", order = 701)]
	public class CopyFolderCallback : DirectoryCallbackBase
	{
		public string[] m_blacklists = default;
		
		public override void Execute()
		{
			FileManagement.CopyFilesRecursive(
				new DirectoryInfo(m_source), 
				new DirectoryInfo(m_destination),
				m_blacklists);
		}
	}
}