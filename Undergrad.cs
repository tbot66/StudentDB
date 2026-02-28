/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026

using StudentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace studentDB
{
    // Enumerated type that keeps track of the students year in the school
    public enum YearRank
    {
        Freshman = 1,
        Sophomore = 2,
        Junior = 3,
        Senior = 4
    }

    internal class Undergrad : Student
    {
        public YearRank Rank { get; set; }

        public string DegreeMajor { get; set; }

        public Undergrad(string first, string last, double gpa, string email, YearRank rank, string degreeMajor)
            : base(first, last, gpa, email)
        {
            Rank = rank;

            DegreeMajor = degreeMajor;
        }
    }
}
