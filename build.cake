var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./src/HearMeApp.Android/bin") + Directory(configuration);

Task("Clean")
	.Does(() =>
	{
	    CleanDirectory(buildDir);
	});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
	{
	    NuGetRestore("./src/HearMeApp.Android.sln");
	});

	Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
	{
	    if(IsRunningOnWindows())
	    {
	      // Use MSBuild
	      MSBuild("./src/HearMeApp.Android.sln", settings =>
	        settings.SetConfiguration(configuration));
	    }
	    else
	    {
	      // Use XBuild
	      XBuild("./src/HearMeApp.Android.sln", settings =>
	        settings.SetConfiguration(configuration));
	    }
	});

Task("Default").IsDependentOn("Build");

RunTarget(target);