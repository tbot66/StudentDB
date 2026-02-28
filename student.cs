/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 --------- Professor Costerella & Students -------- T Info 200 student database app creation
// 2/12/2026 - Costerella & Students - Continued development of the student.cs

using System.Dynamic;

namespace StudentDB
{

    // This POCO class allows us to get information on said students 
    internal class Student
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public double GradeptAvg { get; set; }

        // We will use this as the primary key in the database 
		public string EmailAddress { get; set; }
        public string First { get; }
        public string Last { get; }
        public double Gpa { get; }
        public string Email { get; }

        public Student(string first, string last, double gpa, string email)
        {
            First = first;
            Last = last;
            Gpa = gpa;
            Email = email;
        }

        public override string ToString()
        {
            // This declares a String that builds using the data from the student project
    
            string str = "******* Student Record *******\n";
            str += $"First:{Firstname}\n";
			str += $" Last:{Lastname}\n";
			str += $"Email:{EmailAddress}\n";
			str += $"  GPA:{GradeptAvg:F2}\n";

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
    