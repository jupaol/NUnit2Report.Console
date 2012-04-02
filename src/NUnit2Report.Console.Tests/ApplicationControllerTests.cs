// -----------------------------------------------------------------------
// <copyright file="ApplicationControllerTests.cs" company="Juan Pablo Olmos Lara (Jupaol)">
//
// jupaol@hotmail.com
// http://jupaol.blogspot.com/
// 
// Copyright (c) 2012, Juan Pablo Olmos Lara (Jupaol)
// All rights reserved.
// 
// </copyright>
// -----------------------------------------------------------------------

namespace NUnit2Report.Console.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// Application controller tests
    /// </summary>
    [TestFixture]
    public class ApplicationControllerTests
    {
        [Test]
        public void can_create_a_new_ApplicaitonController_object()
        {
            var controller = ApplicationControllerBuilder.New();

            controller.Should().NotBeNull();
        }

        [Test]
        public void calling_AreOptionsValid_with_valid_arguments_it_should_not_Throw_any_exception()
        {
            ApplicationControllerBuilder.New().Invoking(x => x.ParseArguments())
                .ShouldNotThrow();
        }

        [Test]
        public void calling_AreOptionsValid_with_invalid_arguments_it_hsould_throw_an_exception()
        {
            ApplicationControllerBuilder.New(x =>
                {
                    x.ClearArguments();
                    x.AddDummyInvalidArguments();
                })
                .Invoking(x => x.ParseArguments())
                .ShouldThrow<ParsingArgumentsException>()
                .WithMessage("An error ocurred while parsing the arguments", FluentAssertions.Assertions.ComparisonMode.Substring);
        }

        [Test]
        public void calling_GetHelp_from_the_ApplicationController_it_should_return_TheoryAttribute_help_of_the_application()
        {
            ApplicationControllerBuilder.New().GetHelp()
                .Should().NotBeNullOrEmpty()
                .And.NotBeBlank()
                .And.Contain("frames")
                .And.Contain("noframes")
                .And.Contain("lang")
                .And.Contain("opendesc")
                .And.Contain("todir")
                .And.Contain("out")
                .And.Contain("fileset");
        }

        [Test]
        public void getting_the_Arguments_property_from_the_ApplicationController_it_should_return_the_current_arguments()
        {
            var args = ApplicationControllerBuilder.New().Arguments;

            args
                .Should().NotBeNull()
                .And.NotContainNulls();

            args.ToList().Count.Should().BeGreaterThan(0);
        }

        [Test]
        public void calling_ParseArguments_without_specifying_at_least_one_xml_nunit_results_file_it_should_throw_an_exception()
        {
            var controlelr = ApplicationControllerBuilder.New(x =>
                {
                    x.ClearArguments();
                    x.AddArguments("--fileset=    ;");
                });

            controlelr.Invoking(x => x.ParseArguments())
                .ShouldThrow<ParsingArgumentsException>()
                .WithMessage("An error ocurred while parsing the arguments", FluentAssertions.Assertions.ComparisonMode.Substring);
        }

        [Test]
        public void calling_Execute_with_options_NoFrames_it_should_create_the_tests_report_using_the_default_output_path()
        {
            var controller = ApplicationControllerBuilder.New(x=>
            {
                x.ClearArguments();
                x.AddArguments("--fileset=NUnitReport.xml", "-n");
            });

            controller.Invoking(x => x.Execute())
                .ShouldNotThrow();

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var reportPath = Path.Combine(dir, "DefaultReport\\index.htm");

            reportPath = reportPath.Replace("file:\\", string.Empty);

            File.Exists(reportPath).Should().BeTrue();
        }

        [Test]
        public void calling_Execute_with_options_Frames_it_should_create_the_test_report_using_the_default_output_path()
        {
            var controller = ApplicationControllerBuilder.New(x =>
                {
                    x.ClearArguments();
                    x.AddArguments("--fileset=NUnitReport.xml", "--frames", "--todir", ".\\WithFrames", "--lang", "en", "--opendesc", "-o", "my report.html");
                });

            controller.Invoking(x => x.Execute())
                .ShouldNotThrow();

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var reportPath = Path.Combine(dir, "WithFrames\\my report.html");

            reportPath = reportPath.Replace("file:\\", string.Empty);

            File.Exists(reportPath).Should().BeTrue();
        }

        [Test]
        public void calling_Execute_specifying_the_French_language_it_should_create_the_report_in_French()
        {
            ApplicationControllerBuilder.New(x =>
                {
                    x.ClearArguments();
                    x.AddArguments("--fileset=NUnitReport.xml;", "--lang", "fr", "--todir", ".\\french", "-o", "french language.htm");
                }).Invoking(x => x.Execute())
                .ShouldNotThrow();

            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var reportPath = Path.Combine(dir, "french\\french language.htm");

            reportPath = reportPath.Replace("file:\\", string.Empty);

            File.Exists(reportPath).Should().BeTrue();
        }
    }
}
