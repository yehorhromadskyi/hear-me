#addin Cake.HockeyApp

var HOCKEYAPP_API_TOKEN = EnvironmentVariable("cc9c3ebafbc94d588a6469f09e57b79b");

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
	   MSBuild("./src/HearMeApp.Android.sln", new MSBuildSettings {
			Verbosity = Verbosity.Minimal,
			Configuration = configuration
	    }.WithTarget("SignAndroidPackage"));
	});

Task("Upload-To-HockeyApp")
    .IsDependentOn("Build")
    .Does(() => UploadToHockeyApp(buildDir + File("HearMeApp.Android.apk")));

Task("Default").IsDependentOn("Upload-To-HockeyApp");

RunTarget(target);