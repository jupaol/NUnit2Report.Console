# NCastor Auto Builder #

## Introduction ##

Every serious project today usually needs to be integrated with a Continuous Integration server, so every time a project starts, you have to configure it to run within the CI server of your choice (Hudson, CCNET, TeamCity, TFS, etc), so usually you copy the MSBuild scripts you just used in the last project and then you start to re-configure them again (hopefully, you did created those scripts to be reusable right?) so you start the development of the new application and you have to start running the scripts to see "if they still work with the new settings", and what happens if now, your team decides that you are not going to use NUnit, maybe in this project you don't have a TestDriven.Net license so you decide to use MSTest because it is already integrated with VisualStudio, well then you just have to learn how to integrate MSTest with your MSBuild scripts to modify your old scripts.... and this is over and over with every project and every technology you want to integrate.

The build process in every project is a boring task, and usually takes more time than estimated funny ha? But on the other hand, having an automated build process since the beginning of the project helps a lot, it can be the difference between "this way... or the highway"

## This is where NCastor appears to the rescue ##

**NCastor contains a set of pre-defined MSBuild scripts to allow you quickly INTEGRATE your application with a Continuous Integration server.**

**NCastor integrates popular free third party tools** by wrapping them in MSBuild scripts exposing a simple configuration for you. If you follow the established convention, the configuration required is minimal and intuitive, and since NCastor actually exposes the MSBuild scripts, you will be able to update them to fit your specific needs. 

## This is a list of currently supported features: ##

### Building: ###

- Allowing you to set assembly information to your assemblies
- Auto increment build and revision version (if using Hudson)

### Specify custom building paths for individual projects ###

### Running tests with: ###

- NUnit
- MSTest
- MSpec

### Running tests code coverage with: ###

- OpenCover

### Generate Html code coverage reports with: ###

- ReportGenerator

### Generate test results reports ###

### Automatically package (zip format): ###

- Source code
- Test results and reports
- Build result
- Testing assemblies

### Automatically package (Nuget format) ###


 

## This is a list of features I am currently working on: ##

**Code analysis with:**

- StyleCop

**Assembly analysis with:**

- FxCop

**Generate analysis reports**

**Pack your application using ClickOnce**

**Pack your application using WXI (Windows XML Installer)**

**Create XML Documentation with**

- SandCastle
- NDoc
