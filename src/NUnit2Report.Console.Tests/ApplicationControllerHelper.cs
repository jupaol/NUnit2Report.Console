// -----------------------------------------------------------------------
// <copyright file="ApplicationControllerHelper.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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

    public static class ApplicationControllerBuilder
    {
        public static ApplicationController<Options> New()
        {
            return new ApplicationControllerBuilder<Options>()
                .AddOptions(new Options());
        }

        public static ApplicationController<Options> New(Action<ApplicationControllerBuilder<Options>> configuring)
        {
            var t= new ApplicationControllerBuilder<Options>()
                .AddOptions(new Options());

            configuring(t);

            return t;
        }
    }

    /// <summary>
    /// Applicaiton controller builder used in the tests methods only
    /// </summary>
    public class ApplicationControllerBuilder<TOptions> where TOptions : Options, new()
    {
        private TOptions options = null;
        private List<string> arguments = null;

        public ApplicationControllerBuilder()
        {
            this.arguments = new List<string>();

            this.AddDummyValidArguments();
        }

        public ApplicationControllerBuilder<TOptions> AddOptions(TOptions options)
        {
            this.options = options;

            return this;
        }

        public ApplicationControllerBuilder<TOptions> AddArguments(params string[] arguments)
        {
            this.arguments.AddRange(arguments);

            return this;
        }

        public ApplicationControllerBuilder<TOptions> ClearArguments()
        {
            this.arguments.Clear();

            return this;
        }

        public ApplicationControllerBuilder<TOptions> AddDummyFileSetValidArguments()
        {
            this.arguments.Add("--fileset=my opt 1;my opt 2");

            return this;
        }

        public ApplicationControllerBuilder<TOptions> AddDummyValidArguments()
        {
            this.AddDummyFileSetValidArguments();

            return this;
        }

        public ApplicationControllerBuilder<TOptions> AddDummyInvalidArguments()
        {
            this.arguments.Add("--invalid argument");

            return this;
        }

        public static implicit operator ApplicationController<TOptions>(ApplicationControllerBuilder<TOptions> builder)
        {
            return new ApplicationController<TOptions>(builder.arguments.ToArray(), builder.options);
        }
    }
}
