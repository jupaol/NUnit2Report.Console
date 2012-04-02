// -----------------------------------------------------------------------
// <copyright file="ReportLanguageExtensions.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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

    /// <summary>
    /// Extension methods for the ReportLanguage enumeration
    /// </summary>
    public static class ReportLanguageExtensions
    {
        /// <summary>
        /// Gets the language string.
        /// </summary>
        /// <param name="currentLanguage">The current language.</param>
        /// <returns>
        /// The language string representation
        /// </returns>
        public static string GetLanguageString(this ReportLanguage currentLanguage)
        {
            string language = string.Empty;

            switch (currentLanguage)
            {
                case ReportLanguage.English:
                    language = string.Empty;
                    break;
                case ReportLanguage.French:
                    language = "fr";
                    break;
            }

            return language;
        }
    }
}
