// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;

    /// <summary>
    /// Class to run the application
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Starting point of the applciation
        /// </summary>
        /// <param name="args">The argsuments</param>
        internal static void Main(string[] args)
        {
            ApplicationController<Options> controller = null;

            try
            {
                controller = new ApplicationController<Options>(args, new Options());

                controller.ParseArguments();
                controller.Execute();

                Environment.Exit(0);
            }
            catch (ParsingArgumentsException)
            {
                Console.Out.WriteLine(controller.GetHelp());
                Environment.Exit(5);
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine("The following error ocurred: " + exc.Message);
                Environment.Exit(9);
            }
        }
    }
}
