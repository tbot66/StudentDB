


using StudentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studentDB
{
    internal class GradStudent : Student
    {
        public decimal TuitionCredit { get; set; }

        public string FacultyAdvisor { get; set; }

        public GradStudent(string first, string last, double gpa, string email, decimal credit, string advisor)
            : base(first, last, gpa, email)
        {
            TuitionCredit = credit;
            FacultyAdvisor = advisor;
        }

        public override string ToString()
        {
            // This declares a String that builds using the data from the student project
            string str = base.ToString();
            str += $"Credit: {TuitionCredit:C}\n";
            str += $"Advisor: {FacultyAdvisor}\n";

            // Returns the built string
            return str;
        }

        public string ToStringForOutputFile()
        {
            //This declares a String that builds using the data from the student project

            string str = string.Empty;
            str += $"{Firstname}\n";
            str += $"{Lastname}\n";
            str += $"{EmailAddress}\n";
            str += $"{GradeptAvg:F2}\n";

            // Returns the built string
            return str;
        }
    }
}
