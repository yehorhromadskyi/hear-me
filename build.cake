#addin Cake.HockeyApp
#addin nuget:?package=Cake.AppVeyor
#addin nuget:?package=Refit&version=3.0.0
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./src/HearMeApp.Android/bin") + Directory(configuration);

Task("Clean")
	.Does(() =>
	{
		CleanDirectory(Directory("./src/HearMeApp.Android/obj"));
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
    .Does(() => UploadToHockeyApp(buildDir + File("HearMeApp.Android-Signed.apk"), new HockeyAppUploadSettings {
		Version = BuildSystem.AppVeyor.Environment.Build.Version
	}));

Task("Info")
	.Does(() => 
	{
		Information(
        @"Build:
        Folder: {0}
        Id: {1}
        Number: {2}
        Version: {3}",
        BuildSystem.AppVeyor.Environment.Build.Folder,
        BuildSystem.AppVeyor.Environment.Build.Id,
        BuildSystem.AppVeyor.Environment.Build.Number,
        BuildSystem.AppVeyor.Environment.Build.Version);
	});

Task("Default").IsDependentOn("Info");



RunTarget(target);