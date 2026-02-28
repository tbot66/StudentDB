/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 - Professor Costerella & Students - Student database app creation
// 2/12/2026 - Professor Costerella & Students - Continued development
// 2/28/2026 - Codex - Refactored Student model to support inheritance and file serialization

using System;
using System.Globalization;

namespace studentDB
{
    /// <summary>
    /// Base class for all student records in the database.
    /// </summary>
    internal abstract class Student
    {
        /// <summary>
        /// Gets or sets the student's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the student's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the student's primary key email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the student's grade point average.
        /// </summary>
        public double Gpa { get; set; }

        /// <summary>
        /// Gets the kind of student for persistence and display.
        /// </summary>
        public abstract string StudentKind { get; }

        /// <summary>
        /// Initializes a new student object.
        /// </summary>
        protected Student(string firstName, string lastName, string emailAddress, double gpa)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Gpa = gpa;
        }

        /// <summary>
        /// Builds a single-line record for saving to the text file.
        /// </summary>
        public abstract string ToFileRecord();

        /// <summary>
        /// Creates a readable display string for this student.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0,-11} | {1,-10} {2,-12} | GPA: {3:F2} | Email: {4}",
                StudentKind,
                FirstName,
                LastName,
                Gpa,
                EmailAddress);
        }
    }
}
