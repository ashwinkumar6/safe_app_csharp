
// --------------------------------------------------------------------------------
// Desktop Build and Test
// --------------------------------------------------------------------------------

var coreTestProject = File("SafeApp.Tests.Core/SafeApp.Tests.Core.csproj");
var coreTestBin = Directory("SafeApp.Tests.Core/bin/Release");
var Desktop_TESTS_RESULT_PATH = "SafeAuth.Tests.Core/DesktopTestResult.xml";

Task("Build-Desktop-Project")
  .IsDependentOn("Restore-NuGet")
  .Does(() => {
    var dotnetBuildArgument = string.Empty;
    var osFamily = (int)Context.Environment.Platform.Family;
    if(osFamily == 1)
      dotnetBuildArgument = @"--runtime win10-x64";
    else if(osFamily == 2)
      dotnetBuildArgument = @"--runtime linux-x64";
    else if (osFamily == 3)
      dotnetBuildArgument = @"--runtime win-x64";
    
    DotNetCoreBuild(coreTestProject,
    new DotNetCoreBuildSettings() {
      Configuration = configuration,
      ArgumentCustomization = args => args.Append(dotnetBuildArgument)
      });
  });

Task("Run-Desktop-Tests")
  .IsDependentOn("Build-Desktop-Project")
  .Does(() => {
      DotNetCoreTest(
        coreTestProject.Path.FullPath,
        new DotNetCoreTestSettings()
        {
          Configuration = configuration,
          ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=DesktopTestResult.xml\"")
        });
  })
  .Finally(() =>
  {  
    var resultsFile = File("SafeApp.Tests.Core/DesktopTestResult.xml");
    if(AppVeyor.IsRunningOnAppVeyor)
    {
      AppVeyor.UploadTestResults(resultsFile.Path.FullPath, AppVeyorTestResultsType.MSTest);
    }
  });
