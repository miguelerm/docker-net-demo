#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var tmpPath = Directory("./artifacts/tmp");

var shareAssemblyInfo = File("ShareAssemblyInfo.cs");
var shareAssemblyInfoPath = Directory("./src/Core/Properties");
var shareAssemblyInfoFile = shareAssemblyInfoPath + shareAssemblyInfo;

// Define directories.
var buildDir = Directory("./artifacts") + Directory(configuration);

// Solution to build
var solutionPath = "./src/DockerMonoSample.sln";

Setup((context) =>
{
    CleanDirectory(tmpPath);
    CopyFileToDirectory(shareAssemblyInfoFile, tmpPath);

    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        UpdateAssemblyInfoFilePath = shareAssemblyInfoFile
    });

});

Teardown((context) =>
{
    CopyFile(shareAssemblyInfoFile, buildDir + shareAssemblyInfo);
    CopyFile(tmpPath + shareAssemblyInfo, shareAssemblyInfoFile);
    CleanDirectory(tmpPath);
    CopyDirectory(Directory("./src/TimeService/bin") + Directory(configuration), buildDir + Directory("TimeService"));
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solutionPath, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(solutionPath, settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
