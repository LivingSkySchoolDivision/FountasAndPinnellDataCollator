

namespace FNPCollator;


class Program
{
    static void Main(string[] args)
    {
        try 
        {
            FandPSpreadsheet importedFile = FandPSpreadsheetParser.ParseFandPExcelSpreadsheet("../ExampleData/L_23-24_Bready_3W.xlsx");

            Console.WriteLine("Full File: \t" + importedFile.FullFilePath);
            Console.WriteLine("File: \t\t" + importedFile.FileName);
            Console.WriteLine("SchoolN: \t" + importedFile.SchoolName);        
            Console.WriteLine("Date: \t\t" + importedFile.AssessmentDate.ToLongDateString());
            Console.WriteLine("Teacher: \t" + importedFile.Teacher);
            Console.WriteLine("Grade: \t\t" + importedFile.Grade);
            Console.WriteLine("");
            Console.WriteLine("School: \t" + importedFile.School);
            Console.WriteLine("");
            Console.WriteLine($"Records ({importedFile.Records.Count}):");

            foreach(StudentRecord record in importedFile.Records)
            {
                Console.WriteLine(" " + record.ToString());
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Failed to load file(s): " + ex.Message);
        } 

    }
}