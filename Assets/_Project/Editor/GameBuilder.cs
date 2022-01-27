using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;

namespace SteampunkChess.Editor
{
    public static class GameBuilder
    {
        public static AddressableAssetSettings settings { get; private set; }

        [MenuItem("Build/BuildDev")]
        public static void BuildDev()
        {
            settings = AddressableAssetSettingsDefaultObject.Settings;
            string profileId = settings.profileSettings.GetProfileId("Default");
            settings.activeProfileId = profileId;


            AddressableAssetSettings
        .BuildPlayerContent(out AddressablesPlayerBuildResult result);

            AddressableAssetSettings.BuildPlayerContent();
            var report = BuildPipeline.BuildPlayer(
                new BuildPlayerOptions
                {
                    target = BuildTarget.StandaloneWindows64,
                    locationPathName = @"./artifacts/build/chess.exe",
                    scenes = new string[] {
                        @"Assets/_Project/Game/Scenes/Boot.unity", },

                }
                );


            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new System.Exception("Failed to build dev");
        }
    }
}
