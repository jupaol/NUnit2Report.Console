// -----------------------------------------------------------------------
// <copyright file="ReportFormat.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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
    /// Report format
    /// </summary>
    public enum ReportFormat
    {
        /// <summary>
        /// Indicate the report will be generated using frames
        /// </summary>
        Frames,

        /// <summary>
        /// Indicates the report will be generated without frames, in a single file
        /// </summary>
        NoFrames
    }
}
