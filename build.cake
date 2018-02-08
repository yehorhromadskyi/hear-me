#addin Cake.HockeyApp
#addin Cake.AndroidAppManifest
#addin Cake.AppVeyor
#addin nuget:?package=System.Net.Http&version=4.1.0.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDirectory = Directory("./src/HearMeApp.Android/bin") + Directory(configuration);

var manifestFile = File("./src/HearMeApp.Android/Properties/AndroidManifest.xml");

var version = BuildSystem.AppVeyor.Environment.Build.Version;

Task("Clean")
	.Does(() =>
	{
		CleanDirectory(Directory("./src/HearMeApp.Android/obj"));
	    CleanDirectory(buildDirectory);
	});

Task("Update-AndroidManifest")
    .Does (() =>
	{
	    var manifest = DeserializeAppManifest(manifestFile);
	    manifest.VersionName = version;
	    manifest.VersionCode = 1;
	
	    SerializeAppManifest(manifestFile, manifest);
	});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
	{
	    NuGetRestore("./src/HearMeApp.Android.sln");
	});

Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("Update-AndroidManifest")
	.Does(() =>
	{
	   MSBuild("./src/HearMeApp.Android.sln", new MSBuildSettings {
			Verbosity = Verbosity.Minimal,
			Configuration = configuration
	    }.WithTarget("SignAndroidPackage")
		 .WithProperty("AndroidKeyStore", "true")
         .WithProperty("AndroidSigningKeyAlias", "hearme")
         .WithProperty("AndroidSigningStorePass", EnvironmentVariable("KEYSTORE_PASSWORD"))
         .WithProperty("AndroidSigningKeyStore", "hearme.keystore")
         .WithProperty("AndroidSigningKeyPass", EnvironmentVariable("KEYSTORE_PASSWORD"))
         .WithProperty("DebugSymbols", "false"));
	});

Task("Upload-To-HockeyApp")
    .IsDependentOn("Build")
    .Does(() => UploadToHockeyApp(buildDirectory + File("HearMeApp.Android-Signed.apk")));

Task("Info")
    .IsDependentOn("Upload-To-HockeyApp")
	.Does(() => {
		Information(@"Build version: {0}", version);
	});

Task("Default").IsDependentOn("Info");

RunTarget(target);