/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 - Professor Costerella & Students - Initial undergrad class
// 2/28/2026 - Codex - Added rank/major persistence and display support

using System;
using System.Globalization;

namespace studentDB
{
    /// <summary>
    /// Enumerated year rank values allowed for undergraduates.
    /// </summary>
    public enum YearRank
    {
        Freshman = 1,
        Sophomore = 2,
        Junior = 3,
        Senior = 4
    }

    /// <summary>
    /// Represents an undergraduate student record.
    /// </summary>
    internal class Undergrad : Student
    {
        /// <summary>
        /// Gets or sets the undergraduate's school year rank.
        /// </summary>
        public YearRank Rank { get; set; }

        /// <summary>
        /// Gets or sets the undergraduate's major program.
        /// </summary>
        public string DegreeMajor { get; set; }

        /// <summary>
        /// Gets the discriminator used in output and persistence.
        /// </summary>
        public override string StudentKind { get { return "Undergrad"; } }

        /// <summary>
        /// Initializes a new undergraduate record.
        /// </summary>
        public Undergrad(string firstName, string lastName, string emailAddress, double gpa, YearRank rank, string degreeMajor)
            : base(firstName, lastName, emailAddress, gpa)
        {
            Rank = rank;
            DegreeMajor = degreeMajor;
        }

        /// <summary>
        /// Returns a text line for file persistence.
        /// </summary>
        public override string ToFileRecord()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "U|{0}|{1}|{2}|{3:F2}|{4}|{5}",
                FirstName,
                LastName,
                EmailAddress,
                Gpa,
                Rank,
                DegreeMajor);
        }

        /// <summary>
        /// Returns a readable row for the console.
        /// </summary>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture,
                " | Rank: {0} | Major: {1}",
                Rank,
                DegreeMajor);
        }
    }
}
