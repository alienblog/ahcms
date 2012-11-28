using System;

namespace AHCMS.Core.Logging
{
    /// <summary>
    /// The 7 possible logging levels
    /// </summary>
    /// <author>Gilles Bayon</author>
    public enum LogLevel
    {
        /// <summary>
        /// All logging levels
        /// </summary>
        All = 0,
        /// <summary>
        /// A trace logging level
        /// </summary>
        Trace = 1,
        /// <summary>
        /// A debug logging level
        /// </summary>
        Debug = 2,
        /// <summary>
        /// A info logging level
        /// </summary>
        Info = 3,
        /// <summary>
        /// A warn logging level
        /// </summary>
        Warn = 4,
        /// <summary>
        /// An error logging level
        /// </summary>
        Error = 5,
        /// <summary>
        /// A fatal logging level
        /// </summary>
        Fatal = 6,
        /// <summary>
        /// Do not log anything.
        /// </summary>
        Off = 7,
    }
}