using System.Globalization;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FNPCollator;

public static class CSVFileGenerator
{
    public static string GenerateCSV(List<FandPSpreadsheet> files, bool IncludeHeaderLine, bool ShowFilenameInOutput)
    {
        StringBuilder output_csv = new StringBuilder();

        if (IncludeHeaderLine)
        {
            output_csv.Append($"LearningID,School,Assessment Date,Grade,Instructional Level");
            if (ShowFilenameInOutput)
            {
                output_csv.Append(",School File");
            }
            output_csv.Append("\n");
        }

        foreach(FandPSpreadsheet file in files)
        {
            foreach(StudentRecord record in file.Records.Where(x => x.hasWithdrawDate() == false))
            {
                output_csv.Append($"{record.GovID},{record.SchoolDAN},{(record.AssessmentDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))},{record.Grade},{record.Level}");
                if (ShowFilenameInOutput)
                {
                    output_csv.Append($",{record.SourceSchoolFileName}");
                }
                output_csv.Append("\n");
            }
        }

        return output_csv.ToString();
    }
}