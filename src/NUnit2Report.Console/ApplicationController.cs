// -----------------------------------------------------------------------
// <copyright file="ApplicationController.cs" company="Juan Pablo Olmos Lara (Jupaol)">
//
// jupaol@hotmail.com
// http://jupaol.blogspot.com/
// 
// Copyright (c) 2012, Juan Pablo Olmos Lara (Jupaol)
// All rights reserved.
// 
// </copyright>
// -----------------------------------------------------------------------

namespace NUnit2Report.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CommandLine;
    using CommandLine.Text;
    using CuttingEdge.Conditions;

    /// <summary>
    /// Application main controller
    /// </summary>
    /// <typeparam name="TOptions">The type of the options.</typeparam>
    public class ApplicationController<TOptions> where TOptions : Options, new()
    {
        /// <summary>
        /// The parser object used to parser the command line arguments
        /// </summary>
        private ICommandLineParser parser = null;

        /// <summary>
        /// The arguments specified by the user.
        /// </summary>
        private string[] arguments = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationController&lt;TOptions&gt;"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="options">The options.</param>
        public ApplicationController(string[] arguments, TOptions options)
        {
            this.arguments = arguments;
            this.parser = new CommandLineParser(new CommandLineParserSettings { MutuallyExclusive = true });
            this.Options = options;
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        public IEnumerable<string> Arguments 
        {
            get { return this.arguments.AsEnumerable(); } 
        }

        /// <summary>
        /// Gets the options used by the application (Command line arguments)
        /// </summary>
        public TOptions Options { get; private set; }

        /// <summary>
        /// Parses the arguments.
        /// </summary>
        /// <returns>A reference to the current <c>ApplicationController</c> object</returns>
        public ApplicationController<TOptions> ParseArguments()
        {
            try
            {
                bool parsedRes = this.parser.ParseArguments(this.arguments, this.Options);

                if (parsedRes)
                {
                    ((List<string>)this.Options.XmlFiles).RemoveAll(x => string.IsNullOrWhiteSpace(x));
                    this.ValidateOptions();
                }
                else
                {
                    throw new ParsingArgumentsException();
                }
            }
            catch (Exception ex)
            {
                throw new ParsingArgumentsException("An error ocurred while parsing the arguments", ex);
            }

            return this;
        }

        /// <summary>
        /// Gets the applicaiton help.
        /// </summary>
        /// <returns>
        /// The application help
        /// </returns>
        public string GetHelp()
        {
            var help = new HelpText("NUnit2Report Console version help", "Copyright (c) 2012, Juan Pablo Olmos Lara (Jupaol)" + Environment.NewLine + "All rights reserved.", this.Options);

            return help.ToString();
        }

        /// <summary>
        /// Executes the report process
        /// </summary>
        public void Execute()
        {
            this.ParseArguments();

            var reportGenerator = this.BuildReportGenerator();

            reportGenerator.Execute();
        }

        /// <summary>
        /// Validates the options.
        /// </summary>
        private void ValidateOptions()
        {
            Condition.Requires(this.Options.XmlFiles)
                .IsNotNull()
                .IsNotEmpty("The XmlFiles (fileset) should contain at least one NUnit Xml results file to be processed");

            if (!string.IsNullOrWhiteSpace(this.Options.Language))
            {
                Condition.Requires(this.Options.Language.ToLower().Trim())
                    .Evaluate(x => x.Contains("en") || x.Contains("fr"));
            }
        }

        /// <summary>
        /// Builds the report generator object
        /// </summary>
        /// <returns>
        /// The NUnit2Report object
        /// </returns>
        private NUnit2Report BuildReportGenerator()
        {
            NUnit2Report reportGenerator = new NUnit2Report(this.Options.XmlFiles)
            {
                OpenDescription = this.Options.OpenDescription,
                OutputFilename = this.Options.OutFilename,
                OutputDirectory = this.Options.OutputDirectory
            };

            switch (this.Options.Language)
            {
                case "en":
                    reportGenerator.Language = ReportLanguage.English;
                    break;
                case "fr":
                    reportGenerator.Language = ReportLanguage.French;
                    break;
            }

            if (this.Options.Frames) 
            { 
                reportGenerator.Format = ReportFormat.Frames; 
            }

            if (this.Options.NoFrames)
            {
                reportGenerator.Format = ReportFormat.NoFrames;
            }

            return reportGenerator;
        }
    }
}
