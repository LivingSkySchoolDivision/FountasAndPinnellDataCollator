using System.Text;

namespace FNPCollator;

public class DataAnalysis
{
    static readonly Dictionary<string,int> targetLevelByGrade = new Dictionary<string, int>()
    {
        {"1", 8},   // G
        {"2", 12},  // L
        {"3", 15}   // O
    };

    static readonly Dictionary<string, int> levelValues = new Dictionary<string, int>()
    {
        {"NM", 0},
        {"A", 1},
        {"B", 2},
        {"C", 3},
        {"D", 4},
        {"E", 5},
        {"F", 6},
        {"G", 8},
        {"H", 9},
        {"I", 10},
        {"J", 11},
        {"K", 11},
        {"L", 12},
        {"M", 13},
        {"N", 14},
        {"O", 15},
        {"P", 16},
        {"Q", 17},
        {"R", 18},
        {"S", 19},
        {"T", 20},
        {"U", 21},
        {"V", 22},
        {"W", 23},
        {"X", 24},
        {"Y", 25},
        {"Z", 26}
    };

    private static bool isAtGradeLevel(string level, string grade)
    {
        if (targetLevelByGrade.ContainsKey(grade))
        {
            int target = targetLevelByGrade[grade];
            int given = levelValues[level];
            return given >= target;
        }
        throw new Exception("Unknown grade: " + grade);
    }

    private static string getPercent(double needle, double haystack)
    {
        return $"{(((double)needle/(double)haystack)):P3}";
    }

    public static string GetFileAnalysis(List<FandPSpreadsheet> files, bool verbose)
    {
        StringBuilder returnMe = new StringBuilder();

        returnMe.Append("DATA ANALYSIS\n");
        returnMe.Append("-------------\n");

        int totalStudents = 0;

        Dictionary<string, List<StudentRecord>> studentsByGrade = new Dictionary<string, List<StudentRecord>>();
        Dictionary<string, List<string>> observedStudentNumbers = new Dictionary<string, List<string>>();
        List<string> duplicateStudentIDs = new List<string>();

        Dictionary<string, List<StudentRecord>> skipped_EAL = new Dictionary<string, List<StudentRecord>>();
        Dictionary<string, List<StudentRecord>> skipped_IIP = new Dictionary<string, List<StudentRecord>>();
        Dictionary<string, List<StudentRecord>> skipped_NA = new Dictionary<string, List<StudentRecord>>();

        foreach(FandPSpreadsheet file in files)
        {
            foreach(StudentRecord record in file.Records)
            {
                totalStudents++;

                bool shouldSkipThisStudent = false;

                // Check if we should skip beacuse the student is withdrawn
                if (record.hasWithdrawDate())
                {
                    shouldSkipThisStudent = true;
                }

                // Check if we should skip based on EAL
                if ((record.EAL.Trim().ToLower() == "a1.1") || (record.EAL.Trim().ToLower() == "a1.2"))
                {
                    shouldSkipThisStudent = true;
                    if (!skipped_EAL.ContainsKey(record.Grade))
                    {
                        skipped_EAL.Add(record.Grade, new List<StudentRecord>());
                    }
                    skipped_EAL[record.Grade].Add(record);
                }

                // Check if we should skip based on IIP
                if ((record.IIP.ToLower().Trim() == "2a") || (record.IIP.ToLower().Trim() == "2b"))
                {
                    shouldSkipThisStudent = true;
                    if (!skipped_IIP.ContainsKey(record.Grade))
                    {
                        skipped_IIP.Add(record.Grade, new List<StudentRecord>());
                    }
                    skipped_IIP[record.Grade].Add(record);
                }

                // Check if we should skip based on NA (exempted)
                if (record.Level == "NA")
                {
                    shouldSkipThisStudent = true;
                    if (!skipped_NA.ContainsKey(record.Grade))
                    {
                        skipped_NA.Add(record.Grade, new List<StudentRecord>());
                    }
                    skipped_NA[record.Grade].Add(record);
                }



                if (!shouldSkipThisStudent)
                {
                    if (!studentsByGrade.ContainsKey(record.Grade))
                    {
                        studentsByGrade.Add(record.Grade, new List<StudentRecord>());
                    }
                    studentsByGrade[record.Grade].Add(record);
                }

                if (!observedStudentNumbers.ContainsKey(record.GovID))
                {
                    observedStudentNumbers.Add(record.GovID, new List<string>());
                } else {
                    duplicateStudentIDs.Add(record.GovID);
                }
                observedStudentNumbers[record.GovID].Add($"{file.FileName} {record.GovID} {record.Name} {record.Level}");
            }
        }

        returnMe.Append($"Files analyzed: {files.Count}\n");
        returnMe.Append($"Student records analyzed: {totalStudents}\n");
        returnMe.Append($"Unique students seen: {observedStudentNumbers.Count} ({getPercent(observedStudentNumbers.Count,totalStudents)})\n");
        returnMe.Append($"Duplicate students seen: {duplicateStudentIDs.Count} ({getPercent(duplicateStudentIDs.Count,totalStudents)})\n");

        // returnMe.Append($"\n");

        returnMe.Append($"\nObserved duplicate student records:\n");
        foreach(string govid in duplicateStudentIDs)
        {
            foreach(string meta in observedStudentNumbers[govid])
            {
                returnMe.Append($" {meta}\n");
            }
        }

        returnMe.Append($"\nGrades observed: {studentsByGrade.Count} (");
        foreach(string grade in studentsByGrade.Keys)
        {
            returnMe.Append($"{grade},");
        }
        returnMe.Remove(returnMe.Length-1,1);
        returnMe.Append($")\n\n");

        foreach(string grade in studentsByGrade.Keys)
        {
            returnMe.Append($"\nGrade {grade}\n--------\n");
            List<StudentRecord> recordsAtOrExceedingGradeLevel = new List<StudentRecord>();
            List<StudentRecord> recordsBelowGradeLevel = new List<StudentRecord>();

            foreach(StudentRecord record in studentsByGrade[grade])
            {
                bool atLevel = isAtGradeLevel(record.Level, record.Grade);
                if (atLevel)
                {
                    recordsAtOrExceedingGradeLevel.Add(record);
                } else {
                    recordsBelowGradeLevel.Add(record);
                }
            }

            int thisgrade_skipped_eal = 0;
            int thisgrade_skipped_iip = 0;
            int thisgrade_skipped_na = 0;

            if (skipped_EAL.ContainsKey(grade))
            {
                thisgrade_skipped_eal += skipped_EAL[grade].Count;
            }

            if (skipped_IIP.ContainsKey(grade))
            {
                thisgrade_skipped_iip += skipped_IIP[grade].Count;
            }

            if (skipped_NA.ContainsKey(grade))
            {
                thisgrade_skipped_na += skipped_NA[grade].Count;
            }

            int totalSkippedThisGrade = thisgrade_skipped_eal + thisgrade_skipped_iip;

            returnMe.Append($" Total records: {studentsByGrade[grade].Count + thisgrade_skipped_eal + thisgrade_skipped_iip + thisgrade_skipped_na}\n");
            returnMe.Append($" Total records after exemptions: {studentsByGrade[grade].Count}\n");
            returnMe.Append($" Total at or exceeding grade level: {recordsAtOrExceedingGradeLevel.Count} ({getPercent(recordsAtOrExceedingGradeLevel.Count,studentsByGrade[grade].Count)})\n");
            returnMe.Append($" Total not yet meeting grade level: {recordsBelowGradeLevel.Count} ({getPercent(recordsBelowGradeLevel.Count,studentsByGrade[grade].Count)})\n");

            // exempted
            returnMe.Append($" Total exempted (EAL/IIP): {totalSkippedThisGrade} ({getPercent(totalSkippedThisGrade,studentsByGrade[grade].Count + thisgrade_skipped_eal + thisgrade_skipped_iip + thisgrade_skipped_na)})\n");
            returnMe.Append($"  EAL: {thisgrade_skipped_eal} ({getPercent(thisgrade_skipped_eal,studentsByGrade[grade].Count + thisgrade_skipped_eal + thisgrade_skipped_iip + thisgrade_skipped_na)})\n");
            returnMe.Append($"  IIP: {thisgrade_skipped_iip} ({getPercent(thisgrade_skipped_iip,studentsByGrade[grade].Count + thisgrade_skipped_eal + thisgrade_skipped_iip + thisgrade_skipped_na)})\n");
            returnMe.Append($" Total not participating (NA): {thisgrade_skipped_na} ({getPercent(thisgrade_skipped_na,studentsByGrade[grade].Count + thisgrade_skipped_eal + thisgrade_skipped_iip + thisgrade_skipped_na)})\n");

            if (verbose)
            {
                returnMe.Append($"\n");
                returnMe.Append($" Grade {grade} meeting or exceeding: \n");
                foreach(StudentRecord record in recordsAtOrExceedingGradeLevel)
                {
                    returnMe.Append($"  {record.GovID}\t{record.Level}\t{record.Name}\t{record.SourceSchoolFileName}\n");
                }

                returnMe.Append($" Grade {grade} not yet meeting: \n");
                foreach(StudentRecord record in recordsBelowGradeLevel)
                {
                    returnMe.Append($"  {record.GovID}\t{record.Level}\t{record.Name}\t{record.SourceSchoolFileName}\n");
                }

                returnMe.Append($" Grade {grade} skipped due to IIP (2a/2b) ({thisgrade_skipped_iip}): \n");
                foreach(string g in skipped_IIP.Keys)
                {
                    foreach(StudentRecord record in skipped_IIP[g])
                    {
                        returnMe.Append($"  {record.GovID}\t{record.Level}\t{record.Name}\t{record.SourceSchoolFileName}\n");
                    }
                }

                returnMe.Append($" Grade {grade} skipped due to EAL (A1.1/A1.2) ({thisgrade_skipped_eal}): \n");
                foreach(string g in skipped_EAL.Keys)
                {
                    foreach(StudentRecord record in skipped_EAL[g])
                    {
                        returnMe.Append($"  {record.GovID}\t{record.Level}\t{record.Name}\t{record.SourceSchoolFileName}\n");
                    }
                }

                returnMe.Append($" Grade {grade}  skipped due to not participating (NA) ({thisgrade_skipped_na}): \n");
                foreach(string g in skipped_NA.Keys)
                {
                    foreach(StudentRecord record in skipped_NA[g])
                    {
                        returnMe.Append($"  {record.GovID}\t{record.Level}\t{record.Name}\t{record.SourceSchoolFileName}\n");
                    }
                }
            }
        }

        returnMe.Append("\n");
        return returnMe.ToString();;
    }
}