/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026 - Professor Costerella & Students - Student database app creation
// 2/11/2026 - Professor Costerella & Students - Main menu and user selection
// 2/28/2026 - Codex - Completed polymorphic CRUD + file persistence + full test workflow output

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace studentDB
{
    /// <summary>
    /// Coordinates in-memory student records and file-based persistence.
    /// </summary>
    internal class DbApp
    {
        private const string StudentInputFile = "STUDENT_INPUT_FILE.txt";
        private const string StudentOutputFile = "STUDENT_OUTPUT_FILE.txt";

        // Runtime storage uses the required List<Student> collection.
        private readonly List<Student> students = new List<Student>();

        /// <summary>
        /// Initializes the app by loading records from input text file, or fallback test data.
        /// </summary>
        public DbApp()
        {
            LoadFromFile(StudentInputFile);
            if (students.Count == 0)
            {
                LoadRequiredTestData();
            }
        }

        /// <summary>
        /// Starts interactive CRUD menu loop.
        /// </summary>
        public void GoDatabase()
        {
            while (true)
            {
                DisplayMainMenu();
                char selection = GetUserSelection();

                switch (char.ToUpperInvariant(selection))
                {
                    case 'C':
                        CreateStudentRecordInteractive();
                        break;
                    case 'F':
                        FindStudentRecordInteractive();
                        break;
                    case 'U':
                        UpdateStudentRecordInteractive();
                        break;
                    case 'D':
                        DeleteStudentRecordInteractive();
                        break;
                    case 'P':
                        PrintAllRecords();
                        break;
                    case 'S':
                        SaveStudentDataToOutputFile();
                        break;
                    case 'E':
                        SaveStudentDataToOutputFile();
                        return;
                    case 'Q':
                        return;
                    default:
                        Console.WriteLine("\nERROR: Invalid menu selection.");
                        break;
                }
            }
        }

        /// <summary>
        /// Runs a full scripted test that executes CRUD and persistence requirements.
        /// </summary>
        public void RunScriptedRequirementTest()
        {
            Console.WriteLine("===== BEGIN SCRIPTED REQUIREMENT TEST =====");
            students.Clear();
            LoadRequiredTestData();

            Console.WriteLine("\nInitial in-memory list (4 records, 2 undergrads + 2 grad students):");
            PrintAllRecords();

            Console.WriteLine("\nREAD/FIND test: searching for existing and missing records.");
            PrintFindResult("liam.chen@uw.edu");
            PrintFindResult("missing.student@uw.edu");

            Console.WriteLine("\nCREATE test: adding new undergrad record.");
            CreateStudentRecord(new Undergrad("Elena", "Rios", "elena.rios@uw.edu", 3.73, YearRank.Sophomore, "Biology"));
            PrintAllRecords();

            Console.WriteLine("\nUPDATE test: modifying one grad record and one undergrad record.");
            Student gradToUpdate = FindStudentRecord("maya.patel@uw.edu");
            if (gradToUpdate is GradStudent)
            {
                UpdateStudentRecord("maya.patel@uw.edu", "Maya", "Patel", 3.95, "maya.patel@uw.edu", "Dr. Kline", 7200m);
            }

            Student undergradToUpdate = FindStudentRecord("liam.chen@uw.edu");
            if (undergradToUpdate is Undergrad)
            {
                UpdateStudentRecord("liam.chen@uw.edu", "Liam", "Chen", 3.46, "liam.chen@uw.edu", YearRank.Senior, "Computer Science");
            }
            PrintAllRecords();

            Console.WriteLine("\nDELETE test: removing one record.");
            DeleteStudentRecord("omar.khan@uw.edu");
            PrintAllRecords();

            Console.WriteLine("\nSAVE test: writing to output file.");
            SaveStudentDataToOutputFile();

            Console.WriteLine("\nRELOAD test: validating persistence from output file.");
            List<Student> reloaded = LoadFromLines(File.ReadAllLines(StudentOutputFile));
            foreach (Student student in reloaded)
            {
                Console.WriteLine(student);
            }

            Console.WriteLine("\nUnchanged record proof (record carried through unchanged):");
            Student unchanged = FindStudentRecord("zoe.bennett@uw.edu");
            if (unchanged != null)
            {
                Console.WriteLine(unchanged);
            }

            Console.WriteLine("===== END SCRIPTED REQUIREMENT TEST =====");
        }

        /// <summary>
        /// Adds a student to the database if email is unique.
        /// </summary>
        public bool CreateStudentRecord(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.EmailAddress))
            {
                return false;
            }

            if (FindStudentRecord(student.EmailAddress) != null)
            {
                return false;
            }

            students.Add(student);
            return true;
        }

        /// <summary>
        /// Finds a student by email and returns null when absent.
        /// </summary>
        public Student FindStudentRecord(string email)
        {
            foreach (Student student in students)
            {
                if (string.Equals(student.EmailAddress, email, StringComparison.OrdinalIgnoreCase))
                {
                    return student;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates a specific undergrad record by primary key email.
        /// </summary>
        public bool UpdateStudentRecord(string existingEmail, string firstName, string lastName, double gpa, string email, YearRank rank, string degreeMajor)
        {
            Student student = FindStudentRecord(existingEmail);
            if (!(student is Undergrad undergrad))
            {
                return false;
            }

            if (!CanUseEmail(existingEmail, email))
            {
                return false;
            }

            undergrad.FirstName = firstName;
            undergrad.LastName = lastName;
            undergrad.Gpa = gpa;
            undergrad.EmailAddress = email;
            undergrad.Rank = rank;
            undergrad.DegreeMajor = degreeMajor;
            return true;
        }

        /// <summary>
        /// Updates a specific grad student record by primary key email.
        /// </summary>
        public bool UpdateStudentRecord(string existingEmail, string firstName, string lastName, double gpa, string email, string advisor, decimal tuitionCredit)
        {
            Student student = FindStudentRecord(existingEmail);
            if (!(student is GradStudent gradStudent))
            {
                return false;
            }

            if (!CanUseEmail(existingEmail, email))
            {
                return false;
            }

            gradStudent.FirstName = firstName;
            gradStudent.LastName = lastName;
            gradStudent.Gpa = gpa;
            gradStudent.EmailAddress = email;
            gradStudent.FacultyAdvisor = advisor;
            gradStudent.TuitionCredit = tuitionCredit;
            return true;
        }

        /// <summary>
        /// Deletes a student by primary key email.
        /// </summary>
        public bool DeleteStudentRecord(string email)
        {
            Student student = FindStudentRecord(email);
            if (student == null)
            {
                return false;
            }

            students.Remove(student);
            return true;
        }

        /// <summary>
        /// Saves all in-memory records to plain text output file.
        /// </summary>
        public void SaveStudentDataToOutputFile()
        {
            using (StreamWriter outFile = new StreamWriter(StudentOutputFile, false))
            {
                foreach (Student student in students)
                {
                    outFile.WriteLine(student.ToFileRecord());
                }
            }

            Console.WriteLine("Saved {0} records to {1}", students.Count, StudentOutputFile);
        }

        /// <summary>
        /// Prints all records currently in memory.
        /// </summary>
        public void PrintAllRecords()
        {
            Console.WriteLine("\nCurrent records: {0}", students.Count);
            foreach (Student student in students)
            {
                Console.WriteLine(student);
            }
        }

        /// <summary>
        /// Displays the text menu for interactive mode.
        /// </summary>
        public void DisplayMainMenu()
        {
            Console.Write(@"
╔══════════════════════════════════════════╗
║        Student Database Main Menu        ║
╚══════════════════════════════════════════╝
 ■ [C]reate a new student record
 ■ [F]ind a single existing student record
 ■ [P]rint all student records
 ■ [U]pdate an existing student record
 ■ [D]elete an existing student record
 ■ [S]ave all changes and continue the app
 ■ [E]xit the app AFTER SAVING
 ■ [Q]uit the app WITHOUT SAVING
════════════════════════════════════════════
User Selection: ");
        }

        /// <summary>
        /// Reads and returns one menu selection key.
        /// </summary>
        private char GetUserSelection()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            return key.KeyChar;
        }

        /// <summary>
        /// Handles interactive create flow including student subtype selection.
        /// </summary>
        private void CreateStudentRecordInteractive()
        {
            Console.Write("Enter email address for new student: ");
            string email = Console.ReadLine();
            if (FindStudentRecord(email) != null)
            {
                Console.WriteLine("ERROR: Student already exists.");
                return;
            }

            Console.Write("Enter first name: ");
            string first = Console.ReadLine();
            Console.Write("Enter last name: ");
            string last = Console.ReadLine();
            Console.Write("Enter GPA: ");
            double gpa = ParseDouble(Console.ReadLine());

            Console.Write("Type [U]ndergrad or [G]radStudent: ");
            char kind = char.ToUpperInvariant(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (kind == 'U')
            {
                Console.Write("Enter rank (Freshman/Sophomore/Junior/Senior): ");
                YearRank rank = ParseRank(Console.ReadLine());
                Console.Write("Enter degree major: ");
                string major = Console.ReadLine();
                CreateStudentRecord(new Undergrad(first, last, email, gpa, rank, major));
            }
            else if (kind == 'G')
            {
                Console.Write("Enter faculty advisor: ");
                string advisor = Console.ReadLine();
                Console.Write("Enter tuition credit: ");
                decimal tuition = ParseDecimal(Console.ReadLine());
                CreateStudentRecord(new GradStudent(first, last, email, gpa, advisor, tuition));
            }
            else
            {
                Console.WriteLine("ERROR: invalid subtype.");
            }
        }

        /// <summary>
        /// Handles interactive read/find flow.
        /// </summary>
        private void FindStudentRecordInteractive()
        {
            Console.Write("Enter email address to find: ");
            string email = Console.ReadLine();
            PrintFindResult(email);
        }

        /// <summary>
        /// Handles interactive update flow.
        /// </summary>
        private void UpdateStudentRecordInteractive()
        {
            Console.Write("Enter email address to update: ");
            string email = Console.ReadLine();
            Student existing = FindStudentRecord(email);
            if (existing == null)
            {
                Console.WriteLine("ERROR: record not found.");
                return;
            }

            Console.Write("Enter new first name: ");
            string first = Console.ReadLine();
            Console.Write("Enter new last name: ");
            string last = Console.ReadLine();
            Console.Write("Enter new GPA: ");
            double gpa = ParseDouble(Console.ReadLine());
            Console.Write("Enter new email: ");
            string newEmail = Console.ReadLine();

            bool updated;
            if (existing is Undergrad undergrad)
            {
                Console.Write("Enter new rank: ");
                YearRank rank = ParseRank(Console.ReadLine());
                Console.Write("Enter new major: ");
                string major = Console.ReadLine();
                updated = UpdateStudentRecord(email, first, last, gpa, newEmail, rank, major);
            }
            else
            {
                Console.Write("Enter new faculty advisor: ");
                string advisor = Console.ReadLine();
                Console.Write("Enter new tuition credit: ");
                decimal tuition = ParseDecimal(Console.ReadLine());
                updated = UpdateStudentRecord(email, first, last, gpa, newEmail, advisor, tuition);
            }

            Console.WriteLine(updated ? "Record updated." : "ERROR: update failed.");
        }

        /// <summary>
        /// Handles interactive delete flow.
        /// </summary>
        private void DeleteStudentRecordInteractive()
        {
            Console.Write("Enter email address to delete: ");
            string email = Console.ReadLine();
            bool deleted = DeleteStudentRecord(email);
            Console.WriteLine(deleted ? "Record deleted." : "ERROR: record not found.");
        }

        /// <summary>
        /// Loads records from file path if available.
        /// </summary>
        private void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            List<Student> loaded = LoadFromLines(File.ReadAllLines(filePath));
            students.Clear();
            students.AddRange(loaded);
        }

        /// <summary>
        /// Parses plain-text lines into polymorphic student objects.
        /// </summary>
        private List<Student> LoadFromLines(string[] lines)
        {
            List<Student> parsed = new List<Student>();
            foreach (string rawLine in lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                string[] parts = rawLine.Split('|');
                if (parts.Length < 7)
                {
                    continue;
                }

                string kind = parts[0].Trim();
                string first = parts[1].Trim();
                string last = parts[2].Trim();
                string email = parts[3].Trim();
                double gpa = ParseDouble(parts[4]);

                if (kind == "U")
                {
                    YearRank rank = ParseRank(parts[5]);
                    string major = parts[6].Trim();
                    parsed.Add(new Undergrad(first, last, email, gpa, rank, major));
                }
                else if (kind == "G")
                {
                    string advisor = parts[5].Trim();
                    decimal tuition = ParseDecimal(parts[6]);
                    parsed.Add(new GradStudent(first, last, email, gpa, advisor, tuition));
                }
            }

            return parsed;
        }

        /// <summary>
        /// Loads required test seed with 4 records: 2 undergrads and 2 grad students.
        /// </summary>
        private void LoadRequiredTestData()
        {
            students.Add(new Undergrad("Liam", "Chen", "liam.chen@uw.edu", 3.41, YearRank.Junior, "Informatics"));
            students.Add(new Undergrad("Zoe", "Bennett", "zoe.bennett@uw.edu", 3.88, YearRank.Senior, "Mathematics"));
            students.Add(new GradStudent("Maya", "Patel", "maya.patel@uw.edu", 3.91, "Dr. Nguyen", 6800m));
            students.Add(new GradStudent("Omar", "Khan", "omar.khan@uw.edu", 3.67, "Dr. Roberts", 5400m));
        }

        /// <summary>
        /// Prints find result in a human-obvious format.
        /// </summary>
        private void PrintFindResult(string email)
        {
            Student found = FindStudentRecord(email);
            if (found == null)
            {
                Console.WriteLine("NOT FOUND: {0}", email);
                return;
            }

            Console.WriteLine("FOUND: {0}", found);
        }

        /// <summary>
        /// Checks if a replacement email value is valid and not already owned by another record.
        /// </summary>
        private bool CanUseEmail(string existingEmail, string replacementEmail)
        {
            if (string.Equals(existingEmail, replacementEmail, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return FindStudentRecord(replacementEmail) == null;
        }

        /// <summary>
        /// Parses a string into a double using invariant culture.
        /// </summary>
        private static double ParseDouble(string value)
        {
            if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double parsed))
            {
                return parsed;
            }

            return 0.0;
        }

        /// <summary>
        /// Parses a string into a decimal using invariant culture.
        /// </summary>
        private static decimal ParseDecimal(string value)
        {
            if (decimal.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out decimal parsed))
            {
                return parsed;
            }

            return 0m;
        }

        /// <summary>
        /// Parses a string into a YearRank enum with a freshman fallback.
        /// </summary>
        private static YearRank ParseRank(string value)
        {
            if (Enum.TryParse(value, true, out YearRank rank))
            {
                return rank;
            }

            return YearRank.Freshman;
        }
    }
}
