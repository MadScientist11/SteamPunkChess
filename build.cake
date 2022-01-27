#addin nuget:?package=Cake.Unity&version=0.8.1

var target = Argument("target", "Build-Dev");


Task("Clean-Artifacts")
    .Does(() =>
{
    CleanDirectory($"./artifacts");
});

Task("Build-Dev")
    .IsDependentOn("Clean-Artifacts")
    .Does(() =>
{
        UnityEditor(2020,3,
		new UnityEditorArguments{
			BatchMode = true,
			Quit = true,
			ExecuteMethod = "SteampunkChess.Editor.GameBuilder.BuildDev",
			BuildTarget = BuildTarget.Win64,
			LogFile = "./artifacts/unity.log",
			
		},
		new UnityEditorSettings{
			RealTimeLog = true,
		});
  
});
RunTarget(target);

