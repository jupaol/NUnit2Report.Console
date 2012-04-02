// -----------------------------------------------------------------------
// <copyright file="ParsingArgumentsException.cs" company="Juan Pablo Olmos Lara (Jupaol)">
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
    /// Exception to represent a parsing arguments error
    /// </summary>
    public class ParsingArgumentsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingArgumentsException"/> class.
        /// </summary>
        public ParsingArgumentsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingArgumentsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ParsingArgumentsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingArgumentsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ParsingArgumentsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
