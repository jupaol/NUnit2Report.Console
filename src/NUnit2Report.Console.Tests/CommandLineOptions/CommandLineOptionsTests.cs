// -----------------------------------------------------------------------
// <copyright file="CommandLineOptions_FormatSet_Tests.cs" company="Juan Pablo Olmos Lara (Jupaol)">
//
// jupaol@hotmail.com
// http://jupaol.blogspot.com/
// 
// Copyright (c) 2012, Juan Pablo Olmos Lara (Jupaol)
// All rights reserved.
// 
// </copyright>
// -----------------------------------------------------------------------

namespace NUnit2Report.Console.Tests.CommandLineOptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using CommandLine;
    using FluentAssertions;

    /// <summary>
    /// Tests to verify the parsing process of the  command line arguments
    /// </summary>
    [TestFixture]
    public class CommandLineOptionsTests
    {
        [Test]
        public void when_validating_the_command_line_arguments_when_the_Frames_and_NoFrames_arguments_are_not_specified_the_ParseArguments_method_Should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_validating_the_command_line_arguments_with_the_Frames_argument_specified_the_ParseArguments_method_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("--frames")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_validating_the_command_line_arguments_with_the_NoFrames_arguemnt_specified_the_ParseArguments_method_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("-n")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_validating_the_command_line_arguments_with_the_Frames_and_NoFrames_arguments_specified_the_ParseArguments_method_should_return_false()
        {
            ParseArgumentsHelper.New()
                .SetArguments("-f", "-n")
                .ParseArguments()
                .Should().BeFalse();
        }

        [Test]
        public void when_calling_ParseArguments_when_the_Language_argument_was_specified_it_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("--lang", "en")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_calling_ParseArguments_when_the_OpenDesc_argument_was_specified_it_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("--opendesc")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_calling_ParseArguments_when_the_todir_argument_was_specified_it_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("--todir", "my dir")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_calling_ParseArguments_when_the_Out_argument_was_specified_it_should_return_true()
        {
            ParseArgumentsHelper.New()
                .SetArguments("--out", "my file.htm")
                .ParseArguments()
                .Should().BeTrue();
        }

        [Test]
        public void when_calling_ParseArguments_when_the_Fileset_argument_was_specified_it_should_return_true()
        {
            ParseArgumentsHelper.New()
                .ClearArguments()
                .SetArguments("--fileset=my file 1;my file 2")
                .ParseArguments(x =>
                    {
                        x.XmlFiles
                            .Should().NotBeNull()
                            .And.HaveCount(2)
                            .And.HaveElementAt(0, "my file 1")
                            .And.HaveElementAt(1, "my file 2");
                    })
                .Should().BeTrue();
        }
    }
}
