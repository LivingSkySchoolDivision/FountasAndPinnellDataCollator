using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FNPCollator;

public static class FandPSpreadsheetParser
{
    private const int maxRecordsToAttemptPerFile = 100;

    private static string parseCellValue(ICell cell)
    {
        if (cell != null)
        {
            if (cell.CellType == CellType.Formula)
            {
                switch (cell.GetCachedFormulaResultTypeEnum()) {
                    case CellType.Boolean:
                        return cell.BooleanCellValue.ToString() ?? string.Empty;
                        break;
                    case CellType.Numeric:
                        return cell.NumericCellValue.ToString() ?? string.Empty;
                        break;
                    case CellType.String:
                        return cell.RichStringCellValue.ToString() ?? string.Empty;
                        break;
                    default:
                        return cell?.ToString() ?? string.Empty;
                }
            } else {
                return cell?.ToString() ?? string.Empty;

            }
        } else {
            return string.Empty;
        }
    }

    private static StudentRecord parseStudentRecord(IRow row, DateTime fileDate)
    {
        StudentRecord record = new StudentRecord();

        if (row != null) {
            if (row.GetCell(0) != null)
            {
                record.GovID = row.GetCell(0).ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(record.GovID))
                {
                    record.Grade = row.GetCell(1)?.ToString() ?? string.Empty;
                    record.Name = row.GetCell(2)?.ToString() ?? string.Empty;

                    ICell cell_LevelInstruct = row.GetCell(11); // L
                    record.Level = parseCellValue(cell_LevelInstruct);

                    // Convert the score if we need to
                    
                    // Convert blanks to "NA"
                    if (string.IsNullOrEmpty(record.Level))
                    {
                        record.Level = "NA";
                    }

                    // Convert "PRE" and "EM" to "NM"
                    if (
                        (record.Level.ToLower().Trim() == "PRE") ||
                        (record.Level.ToLower().Trim() == "EM")
                    )
                    {
                        record.Level = "NM";
                    }

                    record.AssessmentDate = fileDate;
                }
            }
        }

        return record;
    }

    public static FandPSpreadsheet ParseFandPExcelSpreadsheet(string FileName)
    {
        FandPSpreadsheet parsedFile = new FandPSpreadsheet();

        if (File.Exists(FileName))
        {
            // Try to open the file and get some basic info from it

            FileStream file = new FileStream(FileName, FileMode.Open);

            parsedFile.FullFilePath = file.Name;
            parsedFile.FileName = Path.GetFileName(file.Name);

            using (IWorkbook workbook = new XSSFWorkbook(file))
            {
                // The "Fall" sheet has the main info on it like school name
                ISheet sheet_Fall = workbook.GetSheet("Fall");                

                // The "Spring" sheet contains the student records we care about
                ISheet sheet_Spring = workbook.GetSheet("Spring");

                if (
                    (sheet_Fall == null) ||
                    (sheet_Spring == null)
                )
                {
                    throw new Exception("Unable to parse - file does not appear to be formatted correctly.");
                } else {
                    parsedFile.Records = new List<StudentRecord>();

                    parsedFile.Teacher = sheet_Fall?.GetRow(3)?.GetCell(6)?.ToString() ?? "UNKNOWN";
                    parsedFile.Grade = sheet_Fall?.GetRow(0)?.GetCell(9)?.ToString() ?? "UNKNOWN";
                    parsedFile.SchoolName = sheet_Fall?.GetRow(2)?.GetCell(6)?.ToString() ?? "UNKNOWN";
                    parsedFile.AssessmentDate = DateTime.Parse(sheet_Fall?.GetRow(4)?.GetCell(8)?.ToString() ?? "1900-01-01 00:00:00");

                    if (
                        ((parsedFile.Teacher == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher))) &&
                        ((parsedFile.Grade == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher))) &&
                        ((parsedFile.SchoolName == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher))) &&
                        (parsedFile.AssessmentDate > new DateTime(1900,01,01))
                        )
                    {
                        throw new Exception("Unable to parse - file does not appear to be formatted correctly.");
                    } else {
                        for(int x = 0; x <= maxRecordsToAttemptPerFile; x++ )
                        {
                            IRow row = sheet_Spring.GetRow(x+9);
                            StudentRecord record = parseStudentRecord(row, parsedFile.AssessmentDate);
                            if (record.isValid())
                            {
                                parsedFile.Records.Add(record);
                            }
                        }
                    }

                }
                
            }

        } else {
            throw new Exception("File does not exist.");            
        }


        if (parsedFile.IsValid())
        {
            return parsedFile;
        } else {
            throw new Exception("Could not parse file.");
        }
        
    }

}