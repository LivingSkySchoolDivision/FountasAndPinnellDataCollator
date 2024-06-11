using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FNPCollator;

public static class FandPSpreadsheetParser
{
    private const int maxRecordsToAttemptPerFile = 100;

    private static readonly List<School> _schools = new List<School>() {
        new School() { Name = "Battelford Central Elementary School", DAN = "5810211", Identifiers = new List<string>() { "Battleford Central School", "BCS" }},
        new School() { Name = "Bready Elementary School", DAN = "5850201", Identifiers = new List<string>() { "Bready" }},
        new School() { Name = "Cando Community School", DAN = "5010213", Identifiers = new List<string>() { }},
        new School() { Name = "Connaught Community School", DAN = "5850401", Identifiers = new List<string>() { "Connaught", "Connaught School" }},
        new School() { Name = "Cut Knife Community School", DAN = "5910123", Identifiers = new List<string>() { "CKCS" }},
        new School() { Name = "Hafford Central School", DAN = "5710213", Identifiers = new List<string>() { "Hafford" }},
        new School() { Name = "Hartley Clark School", DAN = "6410721", Identifiers = new List<string>() { "Hartley Clark", "HCES" }},
        new School() { Name = "Heritage Christian School", DAN = "5894003", Identifiers = new List<string>() { }},
        new School() { Name = "Hillsvale Colony School", DAN = "5910313", Identifiers = new List<string>() { }},
        new School() { Name = "Home Based", DAN = "2020500", Identifiers = new List<string>() { }},
        new School() { Name = "Kerrobert Composite School", DAN = "4410223", Identifiers = new List<string>() { "Kerrobert" }},

        new School() { Name = "Lakeview Colony School", DAN = "5911011", Identifiers = new List<string>() { "Lakeview" }},
        new School() { Name = "Lawrence Elementary School", DAN = "5850501", Identifiers = new List<string>() { "Lawrence School", "Lawrence" }},
        new School() { Name = "Leoville Central School", DAN = "6410313", Identifiers = new List<string>() { }},
        new School() { Name = "Luseland School", DAN = "4410323", Identifiers = new List<string>() { "Luseland" }},
        new School() { Name = "Macklin School", DAN = "4410413", Identifiers = new List<string>() { "Macklin" }},
        new School() { Name = "Manacowin School", DAN = "7350113", Identifiers = new List<string>() { }},
        new School() { Name = "Maymont Central School", DAN = "5810713", Identifiers = new List<string>() { "Maymont" }},
        new School() { Name = "McKitrick Community School", DAN = "5850601", Identifiers = new List<string>() { "McKitrick" }},
        new School() { Name = "McLurg High School", DAN = "5910923", Identifiers = new List<string>() { }},
        new School() { Name = "Meadow Lake Christian Academy", DAN = "6694003", Identifiers = new List<string>() { }},
        new School() { Name = "Medstead Central School", DAN = "6410513", Identifiers = new List<string>() { "Medstead" }},
        new School() { Name = "Newmark Colony School", DAN = "6710722", Identifiers = new List<string>() { }},
        new School() { Name = "Norman Carter Elementary School", DAN = "5910911", Identifiers = new List<string>() { "Norman Carter", "Norman Carter School" }},
        new School() { Name = "North Battleford Comprehensive High School", DAN = "5850904", Identifiers = new List<string>() { }},
        new School() { Name = "Scott Colony School", DAN = "5911113", Identifiers = new List<string>() { }},
        new School() { Name = "Spiritwood High School", DAN = "6410713", Identifiers = new List<string>() { }},
        new School() { Name = "St. Vital Catholic School", DAN = "5810221", Identifiers = new List<string>() { "St. Vital" }},
        new School() { Name = "Unity Composite High School", DAN = "5910813", Identifiers = new List<string>() { }},
        new School() { Name = "Unity Public School", DAN = "5910711", Identifiers = new List<string>() { "UPS" }}
    };

    private static string getSchoolDAN(string input)
    {
        // Check exact name matches
        foreach(School school in _schools)
        {
            if (String.Equals(school.Name.ToLower(), input.ToLower()))
            {
                return school.DAN;
            }
        }

        // Check identifier matches
        foreach(School school in _schools)
        {
            foreach (string term in school.Identifiers)
            if (String.Equals(term.ToLower(), input.ToLower()))
            {
                return school.DAN;
            }
        }

        throw new Exception("School not found: " + input);
    }

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

    private static StudentRecord parseStudentRecord(IRow row, FandPSpreadsheet parentFile)
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
                    record.MSSWithdrawlDate = row.GetCell(8)?.ToString() ?? string.Empty;

                    ICell cell_LevelInstruct = row.GetCell(11); // L
                    record.Level = parseCellValue(cell_LevelInstruct);

                    // Convert the score if we need to

                    // Convert blanks to "NM" (formerly NA)
                    // UPDATE: This used to be "NA" but now should be "NM" as per the ministry
                    if (string.IsNullOrEmpty(record.Level))
                    {
                        record.Level = "NB";
                    }

                    // Convert "PRE" and "EM" to "NM"
                    if (
                        (record.Level.ToUpper().Trim() == "PRE") ||
                        (record.Level.ToUpper().Trim() == "EM")
                    )
                    {
                        record.Level = "NM";
                    }

                    record.AssessmentDate = parentFile.AssessmentDate;
                    record.SchoolDAN = parentFile.SchoolDAN;
                    record.SourceSchoolFileName = parentFile.FileName;
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
                    throw new Exception("Unable to parse - file does not appear to be formatted correctly (Missing 'Fall' and 'Spring' sheets).");
                } else {
                    parsedFile.Records = new List<StudentRecord>();



                    parsedFile.Teacher = parseCellValue(sheet_Spring?.GetRow(3)?.GetCell(6)) ?? "UNKNOWN";
                    parsedFile.Grade = parseCellValue(sheet_Spring?.GetRow(0)?.GetCell(9)) ?? "UNKNOWN";
                    parsedFile.SchoolName = parseCellValue(sheet_Spring?.GetRow(2)?.GetCell(6)) ?? "UNKNOWN";
                    parsedFile.AssessmentDate = DateTime.Parse(sheet_Spring?.GetRow(4)?.GetCell(8)?.ToString() ?? "1900-01-01 00:00:00");


                    if ((parsedFile.Teacher == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher)))
                    {
                        throw new Exception("Unable to parse - file does not appear to be formatted correctly (Missing teacher, got '" + parsedFile.Teacher + "').");
                    } else if ((parsedFile.Grade == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher)))
                    {
                        throw new Exception("Unable to parse - file does not appear to be formatted correctly (Missing grade).");
                    } else if ((parsedFile.SchoolName == "UNKNOWN") || (string.IsNullOrEmpty(parsedFile.Teacher)))
                    {
                        throw new Exception("Unable to parse - file does not appear to be formatted correctly (Missing School Name).");
                    } else if ((parsedFile.AssessmentDate < new DateTime(2010,01,01)))
                    {
                        throw new Exception("Unable to parse - file does not appear to be formatted correctly (Missing or invalid assessment date).");
                    } else {
                        // Try to parse out the school from the school name so
                        // we have it's DAN
                        parsedFile.SchoolDAN = getSchoolDAN(parsedFile.SchoolName);

                        // Parse out any student records in the file
                        for(int x = 0; x <= maxRecordsToAttemptPerFile; x++ )
                        {
                            IRow row = sheet_Spring.GetRow(x+9);
                            StudentRecord record = parseStudentRecord(row, parsedFile);
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