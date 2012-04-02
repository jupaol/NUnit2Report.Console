<?xml version="1.0" encoding="utf-8"?>

<!--
"Your Stuff"

Initial settings
-->

<Project DefaultTargets="All" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">

  <!--TODO: Update the paths if needed-->
  
  <PropertyGroup>
    <GlobalRootPath>$(MSBuildProjectDirectory)\..\..</GlobalRootPath>
    <SolutionsPath>$(GlobalRootPath)\Solutions</SolutionsPath>
    <!--TODO: Set the name of your solution-->
    <SolutionName></SolutionName>

    <NugetPackagesPath>$(SolutionsPath)\Packages</NugetPackagesPath>
    <!--TODO: Update the NCastor path if different from the following path-->
    <NCastorPath>$(NugetPackagesPath)\NCastor.AutoBuilder.Runner.1.1.0.0</NCastorPath>

    <!--TODO: Version properties-->
    <MajorVersion>1</MajorVersion>
    <MinorVersion>0</MinorVersion>

    <!--TODO: Build properties-->
    <Platform>Any CPU</Platform>

    <!--TODO: Set the assembly title-->
    <AssemblyTitle>$RootNamespace$</AssemblyTitle>
    <AssemblyProduct>$DefaultNamespace$</AssemblyProduct>
    
    <!--TODO: Assembly info properties-->
    <AssemblyDescription></AssemblyDescription>
    <AssemblyCompany></AssemblyCompany>
    <AssemblyCopyright></AssemblyCopyright>
    <AssemblyTrademark></AssemblyTrademark>
  </PropertyGroup>

  <!-- TODO: Place your Init settings here -->

</Project>

