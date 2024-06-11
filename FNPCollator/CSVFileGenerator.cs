using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FNPCollator;

public static class CSVFileGenerator
{
    public static string GenerateCSV(FandPSpreadsheet file, string filename) 
    {
        return GenerateCSV(new List<FandPSpreadsheet>() { file }, filename);
    }

    public static string GenerateCSV(List<FandPSpreadsheet> files,string filename)
    {
        StringBuilder output_csv = new StringBuilder();

        output_csv.Append($"LID,School,Assessment Date,Grade,Instructional Level, School File");
        
        foreach(FandPSpreadsheet file in files)
        {
            foreach(StudentRecord record in file.Records) 
            {
                
            }
        }


        return output_csv.ToString();
    }
}