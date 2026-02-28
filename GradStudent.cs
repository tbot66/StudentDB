/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 - Professor Costerella & Students - Initial grad student class
// 2/28/2026 - Codex - Added advisor/tuition persistence and display support

using System;
using System.Globalization;

namespace studentDB
{
    /// <summary>
    /// Represents a graduate student record.
    /// </summary>
    internal class GradStudent : Student
    {
        /// <summary>
        /// Gets or sets the graduate advisor.
        /// </summary>
        public string FacultyAdvisor { get; set; }

        /// <summary>
        /// Gets or sets the tuition credit amount for teaching support.
        /// </summary>
        public decimal TuitionCredit { get; set; }

        /// <summary>
        /// Gets the discriminator used in output and persistence.
        /// </summary>
        public override string StudentKind { get { return "GradStudent"; } }

        /// <summary>
        /// Initializes a new graduate student record.
        /// </summary>
        public GradStudent(string firstName, string lastName, string emailAddress, double gpa, string facultyAdvisor, decimal tuitionCredit)
            : base(firstName, lastName, emailAddress, gpa)
        {
            FacultyAdvisor = facultyAdvisor;
            TuitionCredit = tuitionCredit;
        }

        /// <summary>
        /// Returns a text line for file persistence.
        /// </summary>
        public override string ToFileRecord()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "G|{0}|{1}|{2}|{3:F2}|{4}|{5:F2}",
                FirstName,
                LastName,
                EmailAddress,
                Gpa,
                FacultyAdvisor,
                TuitionCredit);
        }

        /// <summary>
        /// Returns a readable row for the console.
        /// </summary>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture,
                " | Advisor: {0} | Tuition Credit: {1:C}",
                FacultyAdvisor,
                TuitionCredit);
        }
    }
}
