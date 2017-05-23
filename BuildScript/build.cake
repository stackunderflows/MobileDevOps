#tool "nuget:?package=NUnit.Runners&version=2.6.4"
#addin "Cake.FileHelpers"
#addin "Cake.Xamarin"

// --------------------------------------------------------------------------------
//  Arguments
// --------------------------------------------------------------------------------

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var platform = Argument("platform", "iPhone");
var buildNumber = Argument("buildNumber", "");
var testcloudapikey = Argument("testcloudapikey", "");
var iosdevicehash = Argument("iosdevicehash", "");
var androiddevicehash = Argument("androiddevicehash", "");
var email = Argument("email", "");

// --------------------------------------------------------------------------------
//  General Tasks
// --------------------------------------------------------------------------------

var solutionName = "./MyWeather.sln";

Task("Clean-Solution")
    .Does(() =>
{
	DotNetBuild(solutionName, settings => 
	    settings.SetConfiguration(configuration)
	        .WithTarget("Clean")
	        .WithProperty("TreatWarningsAsErrors","true"));
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(solutionName);
});

// --------------------------------------------------------------------------------
//  iOS Targets
// --------------------------------------------------------------------------------

var iosProjectDirectory = "./MyWeather.iOS";
var iosProjectFile = "MyWeather.iOS.csproj";
var iosProjectPath = $"{iosProjectDirectory}/{iosProjectFile}";
var iosAppName = "MyWeatheriOS";
var ipaName = $"{iosAppName}.ipa";
var iosAppId = "";

Task("Clean-iOS")
    .Does(() =>
{
	DotNetBuild(iosProjectPath, settings => 
	    settings.SetConfiguration(configuration)
	        .WithTarget("Clean")
    		.WithProperty("Platform", platform)
    		.WithProperty("OutputPath", $"bin/{platform}/{configuration}/")
	        .WithProperty("TreatWarningsAsErrors","true"));

	CleanDirectories($"{iosProjectDirectory}/bin/{platform}/{configuration}");		
});

Task("Build-iOS")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean-iOS")
    .Does(() =>
{
    DotNetBuild (iosProjectPath, settings => 
	    settings.SetConfiguration(configuration)
    		.WithProperty("Platform", platform)
    		.WithProperty("OutputPath", $"bin/{platform}/{configuration}/"));

	MoveFiles(
        $"{iosProjectDirectory}/bin/{platform}/{configuration}/{iosAppName}*/{ipaName}", 
        $"{iosProjectDirectory}/bin/{platform}/{configuration}");
});

Task("Move-IPA")
    .IsDependentOn("Build-iOS")
    .Does(() =>
{
	MoveFiles(
        $"{iosProjectDirectory}/bin/{platform}/{configuration}/{iosAppName}*/{ipaName}", 
        $"{iosProjectDirectory}/bin/{platform}/{configuration}");
});

// --------------------------------------------------------------------------------
//  Android Targets
// --------------------------------------------------------------------------------

var androidProjectDirectory = "./MyWeather.Droid";
var androidProjectFile = "MyWeather.Android.csproj";
var androidProjectPath = $"{androidProjectDirectory}/{androidProjectFile}";
var androidBundleName = "MyWeather.Droid";
var apkName = $"{androidBundleName}-Signed.apk";
var androidAppId = "";

Task("Clean-Android")
    .Does(() =>
{
    DotNetBuild(androidProjectPath, settings => 
	    settings.SetConfiguration(configuration)
	        .WithTarget("Clean")
    		.WithProperty("OutputPath", $"bin/{configuration}/")
	        .WithProperty("TreatWarningsAsErrors","true"));
});

Task("Build-Android")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean-Android")
    .Does(() =>
{
	AndroidPackage(androidProjectPath, true, settings => 
	    settings.SetConfiguration(configuration)
    		.WithProperty("OutputPath", $"bin/{configuration}/"));
});

// --------------------------------------------------------------------------------
//  Unit Test Targets
// --------------------------------------------------------------------------------

Task("Clean-Unit-Tests")
    .Does(() =>
{
    var portableTestProjects = Context.Globber.GetFiles("./**/*.UnitTests.csproj").ToArray();

    foreach (var testProject in portableTestProjects)
    {
        DotNetBuild (testProject, settings => 
            settings.SetConfiguration(configuration)
	          .WithTarget("Clean")
	          .WithProperty("TreatWarningsAsErrors","true"));
    }
});

Task("Build-Unit-Tests")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean-Unit-Tests")
    .Does(() =>
{
    var portableTestProjects = Context.Globber.GetFiles("./**/*.UnitTests.csproj").ToArray();

    foreach (var testProject in portableTestProjects)
    {
        DotNetBuild (testProject, settings => 
            settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build-Unit-Tests")
    .Does(() =>
{
    NUnit($"./**/bin/{configuration}/*.UnitTests.dll");
});

// --------------------------------------------------------------------------------
//  UI Test Targets
// --------------------------------------------------------------------------------

var uiTestProjectDirectory = "./MyWeather.UITest";
var uiTestProjectFile = "MyWeather.UITest.csproj";
var uiTestProjectPath = $"{uiTestProjectDirectory}/{uiTestProjectFile}";

Task("Clean-UI-Tests")
    .Does(() =>
{
      DotNetBuild (uiTestProjectPath, settings => 
          settings.SetConfiguration(configuration)
          .WithTarget("Clean")
          .WithProperty("TreatWarningsAsErrors","true"));
});

Task("Build-UI-Tests")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Clean-UI-Tests")
    .Does(() =>
{
      DotNetBuild (uiTestProjectPath, settings => 
          settings.SetConfiguration(configuration));
});

Task("Upload-iOS-Test-Cloud")
  .IsDependentOn("Move-IPA")
  .IsDependentOn("Build-UI-Tests")
  .Does(()=>
  {
    if (!string.IsNullOrEmpty(testcloudapikey) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(iosdevicehash) && configuration != "Release")
    {
        var ipapath = $"{iosProjectDirectory}/bin/{platform}/{configuration}/{ipaName}";

        // Try and find a test-cloud-exe from the installed nugets
        var testCloudExePath = GetFiles ($"packages/**/**/test-cloud.exe")
            .FirstOrDefault ();

        // Run testcloud
        TestCloud (ipapath,
            testcloudapikey,
            iosdevicehash,
            email,
            $"{uiTestProjectDirectory}/bin/Debug",
            new TestCloudSettings {
                ToolPath = testCloudExePath
            });
    }
    else
    {
        Information("Unable to upload tests to Test Cloud app because the release configuration was used; or test cloud api key, device hash, and/or email was not specified.");
    }
  });

Task("Upload-Android-Test-Cloud")
  .IsDependentOn("Build-Android")
  .IsDependentOn("Build-UI-Tests")
  .Does(()=>
  {
    if (!string.IsNullOrEmpty(testcloudapikey) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(androiddevicehash) && configuration != "Release")
    {
        var apkpath = $"{androidProjectDirectory}/bin/{configuration}/{apkName}";

        // Try and find a test-cloud-exe from the installed nugets
        var testCloudExePath = GetFiles ($"packages/**/**/test-cloud.exe")
            .FirstOrDefault ();

        // Run testcloud
        TestCloud (apkpath,
            testcloudapikey,
            androiddevicehash,
            email,
            $"{uiTestProjectDirectory}/bin/Debug",
            new TestCloudSettings {
                ToolPath = testCloudExePath
            });
    }
    else
    {
        Information("Unable to upload tests to Test Cloud app because the release configuration was used; or test cloud api key, device hash, and/or email was not specified.");
    }
  });

Task("Upload-Test-Cloud")
  .IsDependentOn("Upload-iOS-Test-Cloud")
  .IsDependentOn("Upload-Android-Test-Cloud")
  .Does(()=>
{
    Information("Success!");
});

// --------------------------------------------------------------------------------
//  Defaults
// --------------------------------------------------------------------------------

Task("Default")
    .Does(() => 
{
	// Do nothing by default
});

RunTarget(target);