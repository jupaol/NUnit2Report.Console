// -----------------------------------------------------------------------
// <copyright file="ParseArgumentsHelper.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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
    using CommandLine;

    public static class ParseArgumentsHelper
    {
        public static ParseArgumentsHelper<Options> New()
        {
            return new ParseArgumentsHelper<Options>(new Options());
        }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ParseArgumentsHelper<TOptions> where TOptions : class, new()
    {
        private ICommandLineParser CommandLineParser { get; set; }
        private List<string> Arguments { get; set; }
        private TOptions Options { get; set; }

        public ParseArgumentsHelper(TOptions options)
        {
            this.CommandLineParser = new CommandLineParser(new CommandLineParserSettings { MutuallyExclusive = true });
            this.Options = options;
            this.Arguments = new List<string>();
            this.AddRequiredArguments();
        }

        public ParseArgumentsHelper<TOptions> SetArguments(params string[] arguments)
        {
            this.Arguments.AddRange(arguments);

            return this;
        }

        public ParseArgumentsHelper<TOptions> ClearArguments()
        {
            this.Arguments.Clear();

            return this;
        }

        public bool ParseArguments()
        {
            return this.CommandLineParser.ParseArguments(this.Arguments.ToArray(), this.Options);
        }

        public bool ParseArguments(Action<TOptions> showingOptions)
        {
            bool res = this.CommandLineParser.ParseArguments(this.Arguments.ToArray(), this.Options);

            showingOptions(this.Options);

            return res;
        }

        public ParseArgumentsHelper<TOptions> AddFilesetDummyArgument()
        {
            this.Arguments.Add("--fileset=my opt 1;my opt 2");

            return this;
        }

        private void AddRequiredArguments()
        {
            this.AddFilesetDummyArgument();
        }
    }
}
