using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Cqunity.BuildSystem
{
	public class BuildTool
	{
		private static readonly string date = DateTime.Now.ToString("yyyy-MM-dd");
		private static readonly string time = DateTime.Now.ToString("hh:mm:ss tt");

		// for release
		private static readonly string deployDir = Wildcard.ProjectRoot + "/Build/deploy";
		private static readonly string deployDirFormat = deployDir + "/{0}";
		
		// for initial build
		private static readonly string stageDir = Wildcard.ProjectRoot + "/Build/container";
		private static readonly string stageDirFormat = stageDir + "/{0}/{1}";
		
		// for archive
		private static readonly string archiveRootPath = Wildcard.ProjectRoot + "/Build/archive";
		private static readonly string archiveDirFormat = archiveRootPath + "/{0}/{1}";
		
		
		// latest build
		// project root / Build / latest / {BuildTarget} / .exe or .apk or .abb
		
		// archive build
		// archive root / company name / product name / {BuildTarget} / version
		
		// deploy
		// deploy root / 

		#region Build/Platform

		[MenuItem("Build/Platform/All Platforms", false, 500)]
		private static void Build_All()
		{
			if (!DisplayConfirmMessage("모든 플랫폼")) return;
			
			UpdateBuildInfo();
			
			bool win64 = Build_Internal(BuildTarget.StandaloneWindows64);;
			bool webgl = Build_Internal(BuildTarget.WebGL);

			if (webgl && win64)
				VersionManager.IncreaseBuild();
		}
		
		[MenuItem("Build/Platform/Web GL", false, 530)]
		private static void Build_WebGL()
		{
			if (!DisplayConfirmMessage("WebGl")) return;
			Build_Internal(BuildTarget.WebGL, true);
		}
		
		[MenuItem("Build/Platform/Standalone Windows x64", false, 531)]
		private static void Build_Standalone_Win()
		{
			if (!DisplayConfirmMessage("Windows x64")) return;
			Build_Internal(BuildTarget.StandaloneWindows64, true);
		}
		
		[MenuItem("Build/Platform/Android Mono", false, 540)]
		private static void Build_Android_Mono()
		{
			if (!DisplayConfirmMessage("Android Mono")) return;
			
			Console.Clear();
			
			Build_Internal(BuildTarget.Android, true);
		}
		
		[MenuItem("Build/Platform/Android IL2CPP", false, 541)]
		private static void Build_Android_il2cpp()
		{
			if (!DisplayConfirmMessage("Android IL2CPP")) return;
			
			Console.Clear();
			
			ScriptingImplementation previous = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android);
			PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
				
			Build_Internal(BuildTarget.Android, true);
			
			PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, previous);
		}
		
		
		[MenuItem("Build/Create Build Information")]
		private static void Menu_UpdateBuildInfo()
		{
			UpdateBuildInfo();
		}

		#endregion

		[MenuItem("Build/Android")]
		private static void Build_Android()
		{
			Console.Clear();
			
			string buildDirectory = $"{Wildcard.ProjectRoot}/Build/stage/{BuildTarget.Android}";
			
			string filename = PlayerSettings.applicationIdentifier + "_v" + VersionManager.GetCurrentBuildVersion();
			string extension = EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
			
			Debug.Log(buildDirectory);

			string latestDirectory = $"{Wildcard.ProjectRoot}/Build/latest";
			string archiveDirectory = BuildSystemProjectSetting.UseGlobalArchive
				? $"{BuildSystemProjectSetting.GlobalArchiveDirectory}/{Wildcard.ProjectName}"
				: $"{Wildcard.ProjectRoot}/Build/archive";

			DirectoryInfo buildDir = new DirectoryInfo(buildDirectory);
			DirectoryInfo latestDir = new DirectoryInfo(latestDirectory);
			DirectoryInfo archiveDir = new DirectoryInfo(archiveDirectory);
			
			buildDir.CreateIfNotExist();
			latestDir.CreateIfNotExist();
			archiveDir.CreateIfNotExist();

			string output = $"{buildDirectory}/{filename}{extension}";

			Debug.Log($"Estimated output :: {output}");

			PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
				out string[] defines);
			
			// 빌드 옵션
			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = BuildTarget.Android,
				locationPathName = output,
				extraScriptingDefines = defines,
			};
			
			// 빌드
			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 아카이브 경로로 카피
				FileManagement.CopyFilesRecursive(buildDir, archiveDir);
				

				// 최신 버전 경로로 카피
				// 배포 경로로 카피
				FileManagement.CopyFilesRecursive(
					buildDir,
					latestDir,
					"BackUpThisFolder_ButDontShipItWithYourGame"
				);
				
				Debug.Log($"<color=yellow>[{options.target}] Build Complete.</color>");
			}
		}
		

		private static bool Build_Internal(BuildTarget target, bool increaseVersion = false, bool logger = true)
		{
			BuildLog log = new BuildLog(target, logger);

			DirectoryInfo ConfigurePath(string description, string pathFormat, params object[] args)
			{
				string root = string.Format(pathFormat, args);
				DirectoryInfo di = new DirectoryInfo(root);

				if (!di.Exists)
					di.Create();
				
				log.Verbose($"{description} 경로 : {di.FullName}");
				return di;
			}
			
			DirectoryInfo archieveDi = ConfigurePath("아카이브", archiveDirFormat, VersionManager.GetCurrentBuildVersion(), target);
			DirectoryInfo deployDi = ConfigurePath("배포", deployDirFormat, target);

			// 실제 빌드 될 경로
			string folderDir;
			string playerDir;
			
			switch (target)
			{
				case BuildTarget.StandaloneWindows64:
					folderDir = string.Format(stageDirFormat, target.ToString(), $"{Wildcard.ProjectName}");
					playerDir = folderDir + $"/{Wildcard.ProjectName}.exe";
					break;

				case BuildTarget.WebGL:
					folderDir = string.Format(stageDirFormat, target.ToString(), $"{Wildcard.ProjectName}");
					playerDir = folderDir;

					break;

				case BuildTarget.iOS:
				case BuildTarget.Android:
				{
					folderDir = string.Format(stageDirFormat, target.ToString(), $"{Wildcard.ProjectName}");
					if (EditorUserBuildSettings.buildAppBundle)
					{
						playerDir = folderDir +$"/{Wildcard.ProjectName}.aab";
					}
					else
					{
						playerDir = folderDir +$"/{Wildcard.ProjectName}.apk";
					}
					break;
				}
				case BuildTarget.StandaloneOSX:
				case BuildTarget.StandaloneWindows:
				case BuildTarget.WSAPlayer:
				case BuildTarget.StandaloneLinux64:
				case BuildTarget.PS4:
				case BuildTarget.XboxOne:
				case BuildTarget.tvOS:
				case BuildTarget.Switch:
				case BuildTarget.Lumin:
				case BuildTarget.Stadia:
				case BuildTarget.NoTarget:
					log.Fatal("이 빌드 플랫폼은 사용할 수 없습니다.");
					return false;
				
				default:
					log.Fatal("알 수 없는 플랫폼");
					return false;
			}

			DirectoryInfo unityPlayerDir = new DirectoryInfo(playerDir);
			DirectoryInfo unityFolderDir = new DirectoryInfo(folderDir);

			if (!unityFolderDir.Exists) unityFolderDir.Create();
			log.Verbose($"빌드 폴더 경로 : {unityFolderDir.FullName}");
			log.Verbose($"빌드 플레이어 경로 : {unityPlayerDir.FullName}");

			PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
				out var defines);
			
			// 빌드 옵션
			BuildPlayerOptions options = new BuildPlayerOptions
			{
				scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray(),
				target = target,
				locationPathName = unityPlayerDir.FullName,
				extraScriptingDefines = defines,
			};
			

			BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == BuildResult.Succeeded)
			{
				// 아카이브 경로로 카피
				FileManagement.CopyFilesRecursive(unityFolderDir, archieveDi);
				
				// 배포 경로로 카피
				FileManagement.CopyFilesRecursive(
					unityFolderDir,
					deployDi,
					"BackUpThisFolder_ButDontShipItWithYourGame"
					);
				
				log.Success($"<color=yellow>[{options.target}] Build Complete.</color>");
				
				if (increaseVersion) VersionManager.IncreaseBuild();
				return true;
			}

			return false;
		}
		
		

		#region Utility

		private static bool DisplayConfirmMessage(string msg)
		{
			return EditorUtility.DisplayDialog("확인", $"{msg}\n빌드를 진행하시겠습니까?", "빌드", "취소");
		}

		private static void UpdateBuildInfo()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.AppendLine("public class BuildInfo {");
			sb.AppendLine("\tpublic const string buildVersion = " + $"\"0x{PlayerSettings.Android.bundleVersionCode:X}\";");
			sb.AppendLine("\tpublic const string buildDate = " + $"\"{date}\";");
			sb.AppendLine("\tpublic const string buildTime = " + $"\"{time}\";");
			sb.AppendLine("}");
			
			File.WriteAllText("Assets/BuildInfo.cs", sb.ToString());
			AssetDatabase.Refresh();
		}
		
		#endregion

	}
}