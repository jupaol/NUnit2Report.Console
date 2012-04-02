// -----------------------------------------------------------------------
// <copyright file="Options.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using CommandLine;

    /// <summary>
    /// Command line options used by the application
    /// </summary>
    public class Options
    {
        /// <summary>
        /// If <c>true</c> the report will be created using frames, default value is <c>false</c>
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option("f", "frames", MutuallyExclusiveSet = "Format", HelpText = "[--frames] ([-f] short version) Indicates the report will be built using frames. Default value is: 'false'")]
        public bool Frames = false;

        /// <summary>
        /// If <c>true</c> the report will be created without frames, default value is <c>true</c>
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option("n", "noframes", MutuallyExclusiveSet = "Format", HelpText = "[--noframes] ([-n] short version) Indicates the report will be built without using frames. Default value is: 'true'")]
        public bool NoFrames = false;

        /// <summary>
        /// Specifies the output language of the report
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option(null, "lang", HelpText = "[--lang <en>|<fr>] Indicates the language used by the report generator. Default value is: 'en'")]
        public string Language = string.Empty;

        /// <summary>
        /// Indicates if the Description methods should be opened. The default value is <c>false</c>
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option(null, "opendesc", HelpText = "[--opendesc] Indicates if all the Description methods should be opened. Default value is: 'false'")]
        public bool OpenDescription = false;

        /// <summary>
        /// Indicates the output directory used to place all the report files
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option(null, "todir", HelpText = "[--todir <\"output dir\">] Indicates the output directory where all the report files will be placed. If it is not specified, the default valuw will be: '.\\DefaultReport'")]
        public string OutputDirectory = string.Empty;

        /// <summary>
        /// Indicates the name of the report file. The default value is: index.htm
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [Option("o", "out", HelpText = "[--out <filename>] (-o short version) Indicates the file name used to create the report. If it is not specified, the default value is: 'index.htm'")]
        public string OutFilename = "index.htm";

        /// <summary>
        /// Sets all the NUnit Xml files that will be used to create the report
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = JustificationForTheFieldsMustBePrivate)]
        [OptionList(null, "fileset", Required = true, Separator = ';', HelpText = "[--fileset=<xml file 1;xml file 2;>] Indicates the xml files containing the NUnit results, the files must be separated by: ';'")]
        public IList<string> XmlFiles = null;

        /// <summary>
        /// Justification for the supress message attributes
        /// </summary>
        private const string JustificationForTheFieldsMustBePrivate = "Required by the Command Line Parser framework";
    }
}
