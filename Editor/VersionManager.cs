using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Cqunity.BuildSystem
{
    [InitializeOnLoad]
    public class VersionManager
    {
        private const string MENU_PATH = "Build/Version/";
        private static bool autoIncreaseBuildVersion;

        private const string autoIncreaseMenuName = MENU_PATH + "빌드 버전 자동 올리기";

        static VersionManager()
        {
            if (EditorApplication.timeSinceStartup < 10f)
            {
                autoIncreaseBuildVersion = EditorPrefs.GetBool(autoIncreaseMenuName, true);
            }

            if (!ValidateVersion())
            {
                FixVersion();
            }
        }

        [MenuItem(autoIncreaseMenuName, false, 1)]
        private static void SetAutoIncrease()
        {
            autoIncreaseBuildVersion = !autoIncreaseBuildVersion;
            EditorPrefs.SetBool(autoIncreaseMenuName, autoIncreaseBuildVersion);
            Debug.Log("Auto Increase : " + autoIncreaseBuildVersion);
        }

        [MenuItem(autoIncreaseMenuName, true)]
        private static bool SetAutoIncreaseValidate()
        {
            Menu.SetChecked(autoIncreaseMenuName, autoIncreaseBuildVersion);
            return true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        [MenuItem(MENU_PATH + "현재 버전 확인", false, 2)]
        private static void CheckCurrentVersion()
        {
            Debug.Log(PlayerSettings.bundleVersion +
                      " [0x" + PlayerSettings.Android.bundleVersionCode.ToString("X") + "]");
        }

        [MenuItem(MENU_PATH + "Increase Season Version", false, 50)]
        private static void IncreaseSeason()
        {
            EditVersion(1, 0, 0, 0);
        }

        [MenuItem(MENU_PATH + "Increase Major Version", false, 51)]
        private static void IncreaseMajor()
        {
            EditVersion(0, 1, 0, 0);
        }

        [MenuItem(MENU_PATH + "Increase Minor Version", false, 52)]
        private static void IncreaseMinor()
        {
            EditVersion(0, 0, 1, 0);
        }

        public static void IncreaseBuild()
        {
            EditVersion(0, 0, 0, 1);
        }

        private static void EditVersion(int season, int majorIncr, int minorIncr, int buildIncr)
        {
            string[] lines = PlayerSettings.bundleVersion.Split('.');

            int SeasonVersion = int.Parse(lines[0]) + season;
            int MajorVersion = int.Parse(lines[1]) + majorIncr;
            int MinorVersion = int.Parse(lines[2]) + minorIncr;
            int Build = int.Parse(lines[3]) + buildIncr;

            PlayerSettings.bundleVersion = SeasonVersion.ToString("0") + "." +
                                           MajorVersion.ToString("0") + "." +
                                           MinorVersion.ToString("0") + "." +
                                           Build.ToString("0");

            PlayerSettings.Android.bundleVersionCode =
                SeasonVersion * 100000000 + MajorVersion * 1000000 + MinorVersion * 10000 + Build;

            // CheckCurrentVersion();
        }

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (autoIncreaseBuildVersion) IncreaseBuild();
        }

        public static string GetNextBuildVersion()
        {
            string[] lines = PlayerSettings.bundleVersion.Split('.');

            int SeasonVersion = int.Parse(lines[0]);
            int MajorVersion = int.Parse(lines[1]);
            int MinorVersion = int.Parse(lines[2]);
            int Build = int.Parse(lines[3]) + 1;

            return SeasonVersion.ToString("0") + "." +
                   MajorVersion.ToString("0") + "." +
                   MinorVersion.ToString("0") + "." +
                   Build.ToString("0");
        }

        private static bool ValidateVersion()
        {
            string[] lines = PlayerSettings.bundleVersion.Split('.');
            if (lines.Length < 4)
            {
                return false;
            }

            return true;
        }

        private static void FixVersion()
        {
            PlayerSettings.bundleVersion = "0.0.0.1";
            Debug.Log($"Version Updated! :: {PlayerSettings.bundleVersion}");
        }

        public static string GetCurrentBuildVersion()
        {
            string[] lines = PlayerSettings.bundleVersion.Split('.');

            int SeasonVersion = int.Parse(lines[0]);
            int MajorVersion = int.Parse(lines[1]);
            int MinorVersion = int.Parse(lines[2]);
            int Build = int.Parse(lines[3]);

            return SeasonVersion.ToString("0") + "." +
                   MajorVersion.ToString("0") + "." +
                   MinorVersion.ToString("0") + "." +
                   Build.ToString("0");
        }
    }
}