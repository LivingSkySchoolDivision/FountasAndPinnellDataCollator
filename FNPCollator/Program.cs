

namespace FNPCollator;


class Program
{
    private static void showFileInfo(FandPSpreadsheet fandpfile, bool showRecords)
    {
            Console.WriteLine("Full File: \t" + fandpfile.FullFilePath);
            Console.WriteLine("File: \t\t" + fandpfile.FileName);
            Console.WriteLine("School: \t" + fandpfile.SchoolName); 
            Console.WriteLine("SchoolDAN: \t" + fandpfile.SchoolDAN);       
            Console.WriteLine("Date: \t\t" + fandpfile.AssessmentDate.ToLongDateString());
            Console.WriteLine("Teacher: \t" + fandpfile.Teacher);
            Console.WriteLine("Grade: \t\t" + fandpfile.Grade);
            Console.WriteLine($"Records ({fandpfile.Records.Count})");

            if (showRecords)
            {
                foreach(StudentRecord record in fandpfile.Records)
                {
                    Console.WriteLine(" " + record.ToString());
                }
            }
    }

    static void Main(string[] args)
    {        
        // TODO: Output to a CSV file

        //string folderPath = @"C:\Users\mark.strendin\code\FountasAndPinnellDataCollator\ExampleData";
        string folderPath = @"C:\Users\mark.strendin\Living Sky School Division\Student Data - 2023-24 Spring\Submitted";        

        List<FandPSpreadsheet> parsedFiles = new List<FandPSpreadsheet>();

        try 
        {
            int failed_files = 0;
            foreach(string file in Directory.GetFiles(folderPath, "L_23-24*.xlsx", SearchOption.TopDirectoryOnly))
            {
                // Try to load the file
                Console.WriteLine($"Loading: {file} ");

                //FandPSpreadsheet importedFile = FandPSpreadsheetParser.ParseFandPExcelSpreadsheet("../ExampleData/L_23-24_Bready_3W.xlsx");
                try {
                    Console.Write(" > ");
                    FandPSpreadsheet importedFile = FandPSpreadsheetParser.ParseFandPExcelSpreadsheet(file);                    
                    parsedFiles.Add(importedFile);
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SUCCESS");
                    Console.ForegroundColor = origColor;

                } catch (Exception ex) 
                {   
                    failed_files++;      
                    Console.Write(" > ");                               
                    ConsoleColor origColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FAILED (" + ex.Message + ")");
                    Console.ForegroundColor = origColor;
                }
            }

            Console.WriteLine("Files:\n\tSuccess: " + parsedFiles.Count + "\n\tFailure: " + failed_files);
            Console.WriteLine("DAN\t\tDate\t\tRecords\tWithdrawn\tFile");
            foreach(FandPSpreadsheet file in parsedFiles) 
            {                
                Console.WriteLine($"{file.SchoolDAN} \t{file.AssessmentDate.ToShortDateString()} \t{file.Records.Count} \t{file.Records.Where(x => x.hasWithdrawDate()).Count()} \t{file.FileName}");
            }
/*
            Console.WriteLine("");
            Console.WriteLine("Records by school:");
            
            foreach(FandPSpreadsheet file in parsedFiles) 
            {
                Console.WriteLine($"{file.FileName} ({file.SchoolName})");
                foreach(StudentRecord stu in file.Records)
                {
                    Console.WriteLine(stu);
                }
            }
*/

        }
        catch (Exception ex) {
            Console.WriteLine("Failed to load file(s): " + ex.Message);
        } 

    }
}