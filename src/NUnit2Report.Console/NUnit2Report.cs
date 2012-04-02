// -----------------------------------------------------------------------
// <copyright file="NUnit2Report.cs" company="Juan Pablo Olmos Lara (Jupaol)">
//
// NUnit2ReportTask.cs
// 
// Author:
//    Gilles Bayon (gilles.bayon@laposte.net)
// Updated Author:
//    Thang Chung (email: thangchung@ymail.com, website: weblogs.asp.net/thangchung)
//    Juan Pablo Olmos (email: jupaol@hotmail.com, website: http://jupaol.blogspot.com/)
// 
// Copyright (C) 2010 ThangChung
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
// 
// </copyright>
// -----------------------------------------------------------------------

namespace NUnit2Report.Console
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;
    using CuttingEdge.Conditions;

    /// <summary>
    /// Creates NUnit reports using XSL files. The reports can be created using Frames or without frames
    /// </summary>
    public class NUnit2Report
    {
        /// <summary>
        /// Default output file name to be used if <see cref="OutputFilename"/> is nto specified
        /// </summary>
        private const string DefaultOutputFileName = "index.htm";

        /// <summary>
        /// Default directory name to be used if <see cref="OutputDirectory"/> is not specified
        /// </summary>
        private const string DefaultOutputDirectoryName = ".\\DefaultReport";

        /// <summary>
        /// XSL Frame definition file name
        /// </summary>
        private const string XslFrameDefinitionFileName = "NUnit-Frame.xsl";

        /// <summary>
        /// Xsl NoFrame definition file name
        /// </summary>
        private const string XslNoFrameDefinitionFileName = "NUnit-NoFrame.xsl";

        /// <summary>
        /// XSL Globalization definition file name
        /// </summary>
        /// <remarks>
        /// Used to load the translations from the "Traductions.xml"
        /// </remarks>
        private const string XslGlobalizationDefinitionFileName = "i18n.xsl";

        /// <summary>
        /// Represents the XML summary document to be used in all transformations
        /// </summary>
        private XmlDocument xmlSummaryDocument;

        /// <summary>
        /// Represents the XSL Globalization definitions file path
        /// </summary>
        private string xslGlobalizationDefinitionFilePath;

        /// <summary>
        /// Rrepresents the common XSLT arguments sued in all transformations
        /// </summary>
        private XsltArgumentList commonXsltArguments;

        /// <summary>
        /// Represents the tool path - the current executing assembly path
        /// </summary>
        private string toolPath;

        /// <summary>
        /// Represents the Xsl Frames definiton file path (Full path)
        /// </summary>
        private string xslFrameDefintionFilePath;

        /// <summary>
        /// Rrepresents the Xsl NoFrames definition file path (full path)
        /// </summary>
        private string xslNoFrameDefinitionFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnit2Report"/> class.
        /// </summary>
        /// <param name="xmlNUnitResultFiles">The XML N unit result files.</param>
        public NUnit2Report(IEnumerable<string> xmlNUnitResultFiles)
        {
            Condition.Requires(xmlNUnitResultFiles).IsNotNull().IsNotEmpty();

            this.Format = ReportFormat.NoFrames;
            this.Language = ReportLanguage.English;
            this.XmlNUnitResultFiles = xmlNUnitResultFiles;
            this.OpenDescription = false;

            this.toolPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            this.xslGlobalizationDefinitionFilePath = Path.Combine(this.toolPath, "xsl\\" + XslGlobalizationDefinitionFileName);
        }

        /// <summary>
        /// Gets or sets the format of the generated report.
        /// Default to "noframes".
        /// </summary>
        public ReportFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the output language.
        /// </summary>
        public ReportLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the description in the transformed html files will be opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the description in the transformed html files will be opened; otherwise, <c>false</c>.
        /// </value>
        public bool OpenDescription { get; set; }

        /// <summary>
        /// Gets or sets the directory where the files resulting from the transformation should be written to.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the index of the Output HTML file(s).
        /// Default to "index.htm".
        /// </summary>
        public string OutputFilename { get; set; }

        /// <summary>
        /// Gets the NUnit XML result files to use as input
        /// </summary>
        public IEnumerable<string> XmlNUnitResultFiles { get; private set; }

        /// <summary>
        /// This is where the work is done
        /// </summary>
        public void Execute()
        {
            this.ConfigureOptionslSettings();
            this.CreateOutputDirectory();

            this.commonXsltArguments = this.GetCommonXsltProperties();
            this.xmlSummaryDocument = this.CreateSummaryXmlDoc();

            switch (this.Format)
            {
                case ReportFormat.Frames:
                    this.CreateFramesReport();
                    break;
                case ReportFormat.NoFrames:
                    this.CreateNoFramesReport();
                    break;
            }
        }

        /// <summary>
        /// Creates the frames report.
        /// </summary>
        private void CreateFramesReport()
        {
#if ECHO_MODE
			Console.WriteLine ("Initializing execution ...");
#endif

            // create the index.html
            var stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                "<xsl:template match=\"test-results\">" +
                "   <xsl:call-template name=\"index.html\"/>" +
                " </xsl:template>" +
                " </xsl:stylesheet>");
            this.Write(stream, Path.Combine(this.OutputDirectory, this.OutputFilename));

            // create the stylesheet.css
            stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                "<xsl:template match=\"test-results\">" +
                "   <xsl:call-template name=\"stylesheet.css\"/>" +
                " </xsl:template>" +
                " </xsl:stylesheet>");
            this.Write(stream, Path.Combine(this.OutputDirectory, "stylesheet.css"));

            // create the overview-summary.html at the root
            stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                "<xsl:template match=\"test-results\">" +
                "    <xsl:call-template name=\"overview.packages\"/>" +
                " </xsl:template>" +
                " </xsl:stylesheet>");
            this.Write(stream, Path.Combine(this.OutputDirectory, "overview-summary.html"));

            // create the allclasses-frame.html at the root
            stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                "<xsl:template match=\"test-results\">" +
                "    <xsl:call-template name=\"all.classes\"/>" +
                " </xsl:template>" +
                " </xsl:stylesheet>");
            this.Write(stream, Path.Combine(this.OutputDirectory, "allclasses-frame.html"));

            // create the overview-frame.html at the root
            stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                "<xsl:template match=\"test-results\">" +
                "    <xsl:call-template name=\"all.packages\"/>" +
                " </xsl:template>" +
                " </xsl:stylesheet>");
            this.Write(stream, Path.Combine(this.OutputDirectory, "overview-frame.html"));

            // Create directory
            string path;

            // --- Change 11/02/2003 -- remove
            ////XmlDocument doc = new XmlDocument();
            ////doc.Load("result.xml"); _FileSetSummary
            // ---

            ////doc.CreateNavigator();
            var xpathNavigator = this.xmlSummaryDocument.CreateNavigator();

            // Get All the test suite containing test-case.
            if (xpathNavigator != null)
            {
                var expr = xpathNavigator.Compile("//test-suite[(child::results/test-case)]");
                var iterator = xpathNavigator.Select(expr);
                string directory;

                while (iterator.MoveNext())
                {
                    var xpathNavigator2 = iterator.Current;
                    var testSuiteName = iterator.Current.GetAttribute("name", string.Empty);

                    // Get get the path for the current test-suite.
                    var iterator2 = xpathNavigator2.SelectAncestors(string.Empty, string.Empty, true);
                    path = string.Empty;
                    var parent = string.Empty;
                    var parentIndex = -1;

                    while (iterator2.MoveNext())
                    {
                        directory = iterator2.Current.GetAttribute("name", string.Empty);
                        if (directory != string.Empty && directory.IndexOf(".dll") < 0)
                        {
                            path = directory + "/" + path;
                        }

                        if (parentIndex == 1)
                        {
                            parent = directory;
                        }

                        parentIndex++;
                    }

                    // path = xx/yy/zz
                    Directory.CreateDirectory(Path.Combine(this.OutputDirectory, path));

                    // Build the "testSuiteName".html file
                    // Correct MockError duplicate testName !
                    // test-suite[@name='MockTestFixture' and ancestor::test-suite[@name='Assemblies'][position()=last()]]
                    stream = new StringReader("<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' >" +
                        "<xsl:output method='html' indent='yes' encoding='ISO-8859-1'/>" +
                        "<xsl:include href=\"" + this.xslFrameDefintionFilePath + "\"/>" +
                        "<xsl:template match=\"/\">" +
                        "	<xsl:for-each select=\"//test-suite[@name='" + testSuiteName + "' and ancestor::test-suite[@name='" + parent + "'][position()=last()]]\">" +
                        "		<xsl:call-template name=\"test-case\">" +
                        "			<xsl:with-param name=\"dir.test\">" + string.Join(".", path.Split('/')) + "</xsl:with-param>" +
                        "		</xsl:call-template>" +
                        "	</xsl:for-each>" +
                        " </xsl:template>" +
                        " </xsl:stylesheet>");
                    this.Write(stream, Path.Combine(Path.Combine(this.OutputDirectory, path), testSuiteName + ".html"));
                }
            }
        }

        /// <summary>
        /// Configures the optional settings.
        /// </summary>
        private void ConfigureOptionslSettings()
        {
            this.OutputFilename = string.IsNullOrWhiteSpace(this.OutputFilename) ? DefaultOutputFileName : this.OutputFilename;
            this.OutputDirectory = string.IsNullOrWhiteSpace(this.OutputDirectory) ? DefaultOutputDirectoryName : this.OutputDirectory;

            switch (this.Format)
            {
                case ReportFormat.NoFrames:
                    this.xslNoFrameDefinitionFilePath = Path.Combine(this.toolPath, "xsl\\" + XslNoFrameDefinitionFileName);
                    break;
                case ReportFormat.Frames:
                    this.xslFrameDefintionFilePath = Path.Combine(this.toolPath, "xsl\\" + XslFrameDefinitionFileName);
                    break;
            }
        }

        /// <summary>
        /// Creates the output directory.
        /// </summary>
        private void CreateOutputDirectory()
        {
            if (!Directory.Exists(this.OutputDirectory))
            {
                Directory.CreateDirectory(this.OutputDirectory);
            }
        }

        /// <summary>
        /// Initializes the XmlDocument instance
        /// used to summarize the test results
        /// </summary>
        /// <returns>
        /// The Xml representing the Sumamry to be used in all transformations
        /// </returns>
        private XmlDocument CreateSummaryXmlDoc()
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement("testsummary");

            root.SetAttribute("created", DateTime.Now.ToString());
            doc.AppendChild(root);

            foreach (var file in this.XmlNUnitResultFiles)
            {
                var source = new XmlDocument();

                source.Load(file);
                if (source.DocumentElement == null)
                {
                    continue;
                }

                var node = doc.ImportNode(source.DocumentElement, true);

                if (doc.DocumentElement != null)
                {
                    doc.DocumentElement.AppendChild(node);
                }
            }

            return doc;
        }

        /// <summary>
        /// Builds an XsltArgumentList with all
        /// the properties defined in the
        /// current project as XSLT parameters.
        /// </summary>
        /// <returns>
        /// The <c>XsltArgumentList</c> containing the common Xslt arguments
        /// </returns>
        private XsltArgumentList GetCommonXsltProperties()
        {
            var args = new XsltArgumentList();

            args.AddParam("sys.os", string.Empty, Environment.OSVersion.ToString());
            args.AddParam("sys.clr.version", string.Empty, Environment.Version.ToString());
            args.AddParam("sys.machine.name", string.Empty, Environment.MachineName);
            args.AddParam("sys.username", string.Empty, Environment.UserName);

            // Add argument to the C# XML comment file
            args.AddParam("summary.xml", string.Empty, string.Empty);

            // Add open.description argument
            args.AddParam("open.description", string.Empty, this.OpenDescription ? "yes" : "no");

            return args;
        }

        /// <summary>
        /// Creates the no frames report.
        /// </summary>
        private void CreateNoFramesReport()
        {
            var xslTransform = new XslCompiledTransform();

            xslTransform.Load(this.xslNoFrameDefinitionFilePath, new XsltSettings(true, true), new XmlUrlResolver());

            // tmpFirstTransformPath hold the first transformation
            var tmpFirstTransformPath = Path.GetTempFileName();
            var firstTransformationStream = new XmlTextWriter(tmpFirstTransformPath, System.Text.ASCIIEncoding.UTF8);
            xslTransform.Transform(new XmlNodeReader(this.xmlSummaryDocument), this.commonXsltArguments, firstTransformationStream);
            firstTransformationStream.Flush();
            firstTransformationStream.Close();

            // ---------- i18n --------------------------
            var xsltI18NArgs = new XsltArgumentList();
            xsltI18NArgs.AddParam("lang", string.Empty, this.Language.GetLanguageString());

            var xslt = new XslCompiledTransform();

            xslt.Load(this.xslGlobalizationDefinitionFilePath, new XsltSettings(true, true), new XmlUrlResolver());

            var xmlDoc = new XPathDocument(tmpFirstTransformPath);
            var writerFinal = new XmlTextWriter(Path.Combine(this.OutputDirectory, this.OutputFilename), System.Text.Encoding.GetEncoding("ISO-8859-1"));

            // Apply the second transform to xmlReader to final ouput
            xslt.Transform(xmlDoc, xsltI18NArgs, writerFinal);
            writerFinal.Close();
        }

        /// <summary>
        /// Writes the specified stream containing the Xslt fragment of code.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        private void Write(TextReader stream, string fileName)
        {
            // Load the XmlTextReader from the stream
            var reader = new XmlTextReader(stream);
            var xslTransform = new XslCompiledTransform();

            // Load the stylesheet from the stream.
            xslTransform.Load(reader, new XsltSettings(true, true), new XmlUrlResolver());

            // xmlDoc = new XPathDocument("result.xml");

            // tmpFirstTransformPath hold the first transformation
            var tmpFirstTransformPath = Path.GetTempFileName();
            var firstTransformationStream = new XmlTextWriter(tmpFirstTransformPath, System.Text.ASCIIEncoding.UTF8);

            xslTransform.Transform(new XmlNodeReader(this.xmlSummaryDocument), this.commonXsltArguments, firstTransformationStream);
            firstTransformationStream.Flush();
            firstTransformationStream.Close();

            if (fileName.EndsWith(".css"))
            {
                File.Copy(tmpFirstTransformPath, fileName, true);
                return;
            }

            // ---------- i18n --------------------------
            var xsltI18NArgs = new XsltArgumentList();

            xsltI18NArgs.AddParam("lang", string.Empty, this.Language.GetLanguageString());

            var xslt = new XslCompiledTransform();

            // Load the stylesheet.
            xslt.Load(this.xslGlobalizationDefinitionFilePath, new XsltSettings(true, true), new XmlUrlResolver());

            var xmlDoc = new XPathDocument(tmpFirstTransformPath);
            var writerFinal = new XmlTextWriter(fileName, System.Text.Encoding.GetEncoding("ISO-8859-1"));

            // Apply the second transform to xmlReader to final ouput
            xslt.Transform(xmlDoc, xsltI18NArgs, writerFinal);
            writerFinal.Close();
        }
    } // class NUnit2ReportTask
} // namespace  NAnt.NUnit2ReportTasks