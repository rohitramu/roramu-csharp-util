namespace RoRamu.Utils.Logging
{
    /// <summary>
    /// Represents the severity level of a log statement.
    /// </summary>
    public enum LogLevel
    {
        ///<summary>
        /// Debugging.  This is the most verbose of all log levels.
        ///</summary>
        Debug = 0,

        ///<summary>
        /// Informational.  Logs at this level would typically be useful for monitoring behavior and performance.
        ///</summary>
        Info = 1,

        ///<summary>
        /// Warning.  Signals a potential problem, that requires investigation.  May also indicate a state
        /// which is undesirable or indicative of impending problems.
        ///</summary>
        Warning = 2,

        ///<summary>
        /// Error.  A known bad state which should be rectified.
        ///</summary>
        Error = 3,
    }
}
