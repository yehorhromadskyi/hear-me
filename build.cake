#addin Cake.HockeyApp

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
    .Does(() => UploadToHockeyApp(buildDir + File("HearMeApp.Android-Signed.apk")));

Task("Info")
    .IsDependentOn("Upload-To-HockeyApp")
	.Does(() => {
		Information(@"Build version: {BuildSystem.AppVeyor.Environment.Build.Version}");
	});

Task("Default").IsDependentOn("Info");

RunTarget(target);