/////////////////////////////////////////////////////////////////////////////////
//change history
//2/10/2026---------Professor Costerella & Students--------T Info 200 Calc


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDB
{
    internal class Program
    {

        // Constant flag to turn debugging on and off easily
        private const bool _DEBUG_MODE_ = true;

        static void Main(string[] args)
        {
            if (Program._DEBUG_MODE_) TestMain();

            // There will only be one database app
            DbApp db = new DbApp();
            db.GoDatabase();


        }

        // This code was just a test to look at the student data.
        static void TestMain()
        {
            // We want to be able to make students as objects 
            Student stu01 = new Student();
            Student stu02 = new Student("Alice", "Anderson", 3.9, "aanderson@uw.edu");
            Student stu03 = new Student("Bob", "Bradshaw", 2.9, "bbradshaw@uw.edu");


            // Test the output for the strage in the objects 
            Console.WriteLine(stu01);
            Console.WriteLine(stu02);
            Console.WriteLine(stu03);


		}
    }
}
