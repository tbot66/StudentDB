/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 - Professor Costerella & Students - Initial app entry point
// 2/28/2026 - Codex - Added scripted requirement test mode

using System;

namespace studentDB
{
    /// <summary>
    /// Entry point for the student database application.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Launches either scripted requirement testing or interactive mode.
        /// </summary>
        private static void Main(string[] args)
        {
            DbApp db = new DbApp();

            if (args != null && args.Length > 0 && string.Equals(args[0], "--scripted-test", StringComparison.OrdinalIgnoreCase))
            {
                db.RunScriptedRequirementTest();
                return;
            }

            db.GoDatabase();
        }
    }
}
