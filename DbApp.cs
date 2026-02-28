/////////////////////////////////////////////////////////////////////////////////
// Change History
// 2/10/2026--------- Professor Costerella & Students -------- T Info 200 student database app creation
// 2/10/2026--------- Professor Costerella & Students -------- Creation of CRUD operations for the student database app
// 2/11/2026--------- Professor Costerella & Students -------- Creation of the main menu and user selection for the student database app


using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace StudentDB
{
	internal class DbApp
	{
		// Raw storage of the student objects representing students in the school
		private List<Student> students = new List<Student>(); 
		public DbApp()
		{
			// Temporary code that loads 4 students into the database 
			LoadTestDataIntoList();
			ReadStudentDataFromInputFile();

		}

		private const string STUDENT_INPUT_FILE = "STUDENT_INPUT_FILE.txt";
		private void ReadStudentDataFromInputFile()
		{
			// If the file doesn't exist, nothing to read
			if (!File.Exists(STUDENT_INPUT_FILE))
			{
				return;
			}

			// Create the file object and point to the real file on disk
			using (var inFile = new StreamReader(STUDENT_INPUT_FILE))
			{
				string first;
				// Use the fileobj to read in the data; records are expected as 5 lines each:
				// First, Last, GPA, Email, Rank
				while ((first = inFile.ReadLine()) != null && first != string.Empty)
				{
					string last = inFile.ReadLine();
					string gpaLine = inFile.ReadLine();
					string email = inFile.ReadLine();
					string rankLine = inFile.ReadLine();

					if (last == null || gpaLine == null || email == null || rankLine == null)
					{
						// Incomplete record at end of file — skip and log
						Console.WriteLine("WARNING: Incomplete student record encountered in input file; skipping.");
						break;
					}

					// Parse GPA using invariant culture to avoid locale issues
					if (!double.TryParse(gpaLine, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double gpa))
					{
						Console.WriteLine($"WARNING: Invalid GPA value '{gpaLine}' for student {first} {last}; skipping record.");
						continue;
					}

                    // Parse rank (allow case-insensitive parsing)
                    if (!Enum.TryParse<YearRank>(rankLine, true, out YearRank rank))
					{
						Console.WriteLine($"WARNING: Invalid YearRank value '{rankLine}' for student {first} {last}; skipping record.");
						continue;
					}

					// Create a new student object utilizing the data and add it to the list
					Student stu = new Student(first, last, gpa, email);
					students.Add(stu);
				}
			}

			// Close the file reference - using ensures disposal
		}



		// Main loop running the typical CRUD operations 
		public void GoDatabase()
		{
			while (true)
			{
				// This displays a main menu and asks for a user selection 
				DisplayMainMenu();

				// Caputures the users selection 
				char selection = GetUserSelection();


				// This uses the user selection here to "farm out" the Crud operations and 
				// Other db app options

				switch (selection)
				{

					case 'C':
					case 'c':
						// [C] reate a new student record
						CreatNewStudentRecord();
						break;

					case 'F':
					case 'f':
						//[ F]ind a single existing student record
						// TODO: this util method will also return the stu reference or NULL
						// Indicating the student was not found 
						FindStudentRecord();
						break;

					case 'U':
					case 'u':
						// [U]pdate an existing student record
						UpdateStudentRecord();
						break;

					case 'D':
					case 'd':
						// [D]elete an existing student record
						DeleteStudentRecord();
						break;

					case 'P':
					case 'p':
						// [P]rint all student records
						PrintAllRecords();
						break; 
						
					case 'E':
					case 'e':
						// [E]xit the app AFTER SAVING
						SaveStudentDataToOutputFile();
						Environment.Exit(0);
						break;

					case 'Q':
					case 'q':
						// [Q]uit the app WITHOUT SAVING
						Environment.Exit(0);
						break;

					case 'S':
					case 's':
						// [S]ave all changes to the outputfile and continue the app
						SaveStudentDataToOutputFile();
						break;

					default:
						Console.WriteLine($"  ERROR: {selection} is not a valid choice. Please select again: ");
						break;
				}



				
			}
		}

		private void FindStudentRecord()
		{
			throw new NotImplementedException();
		}

		// This allows the creation of a new student 
		// BUT will only be done if the student isnt in already 
		private void CreatNewStudentRecord()
		{     
		// Use the util method find to see if the student to add is not
		// Already in the database. if so print an error and return
		string email = string.Empty;
		Student stu = FindStudentRecord(out email);

			if (stu == null)
			{
				// If the student isnt in the database - we can add them 
				Console.Write($"Creating new student record for email: {email}");
				Console.Write("ENTER first name: ");
				string firstname = Console.ReadLine();
				Console.Write("ENTER last name: ");
				string lastname = Console.ReadLine();
				Console.Write("ENTER grade point average: ");
				double gpa = double.Parse(Console.ReadLine());
				Console.WriteLine("[1]Freshman [2]Sophomore [3]Junior [4]Senior");
				Console.WriteLine("ENTER year rank in school: ");
				YearRank rank = (YearRank)int.Parse(Console.ReadLine());
				// Create  the new student object and add it to the list 
				stu = new Student(firstname, lastname, gpa, email);
				students.Add(stu);

				// NOTE: removed duplicate creation/add that previously added the new student twice
			}
			else
			{
				// Student is alreadt in the database then reports back to the user and return 
				Console.WriteLine($"ERROR: Student with the email {email} already exists. Cannot create Duplicate record");

			}
		}


		// This find operation will search the current list to see if the given email 
		// Is present and return the student record if its found, otherwise return no 
		private Student FindStudentRecord(out string email)
		{
			// This takes the desired email address from the user 
			Console.WriteLine("\nENTER the email address (Primary Key) to search for: ");
			email = Console.ReadLine();

			// This will iterate through the database and look for the email 
			foreach (Student stu in students)
			{
				if (email == stu.EmailAddress)
				{
					// This states the email was found and reports back to the user and returns the stu object 
					Console.Write($"FOUND the email address: {stu.EmailAddress}");
					return stu;
				}

			}
			// This states the email wasnt found and notifies the user 
			Console.WriteLine($"{email} NOT FOUND");
			return null;
		}

		private void UpdateStudentRecord()
		{
		   
		}

		private void DeleteStudentRecord()
		{
			throw new NotImplementedException();
		}

		private string STUDENT_OUTPUTFILE = "STUDENT_OUTPUT_FILE.txt";
		private void SaveStudentDataToOutputFile()
		{
			// Make the file and associated objects
			using (var outFile = new StreamWriter(STUDENT_OUTPUTFILE, false))
			{

				// Use a stable, machine-friendly field order: First, Last, GPA, Email, Rank
				foreach (Student stu in students)
				{
					outFile.WriteLine(stu.Firstname);
					outFile.WriteLine(stu.Lastname);
					outFile.WriteLine(stu.Gpa.ToString("F2", CultureInfo.InvariantCulture));
					outFile.WriteLine(stu.EmailAddress);
					outFile.WriteLine(stu.Rank.ToString());
					// Keep console output human-readable
					Console.WriteLine(stu);
				}

			}

		}

		// This prints out each and every student that is in the database
		private void PrintAllRecords()
		{
			foreach (Student stu in students)
			{
				Console.WriteLine(stu);
			}
		}

		// This accepts the users selection and places it in the main menu
		private char GetUserSelection()
		{
		   ConsoleKeyInfo key =Console.ReadKey();
			return key.KeyChar;
		}

		// This displays the menu for the user to see what they can press. each character to press is []
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
 ■ [E]xit the app AFTER SAVING
 ■ [Q]uit the app WITHOUT SAVING
 ■ [S]ave all changes and continue the app
════════════════════════════════════════════
User Selection: ");

		}


		// This is a test for the data in the array list (until we do another such as a file
		private void LoadTestDataIntoList()
		{
			students.Add(new Student("Alice", "Anderson", 3.9, "aanderson@uw.edu"));
			students.Add(new Student("Bob", "Bradshaw", 2.9, "bbradshaw@uw.edu"));
			students.Add(new Student("Johnny", "Dylan", 3.3, "jdylan@uw.edu"));
			students.Add(new Student("Samantha", "Cook", 1.5, "scook@uw.edu"));
		}
	}
}