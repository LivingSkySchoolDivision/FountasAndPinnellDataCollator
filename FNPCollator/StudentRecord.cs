namespace FNPCollator;

/*
    Fields we care about in the final product
     - LearningID / GovID
     - School Number / DAN
     - Completed Assessment Date
     - Grade
     - Instructional Score
     - Name of the source file for this record
*/


public class StudentRecord
{
    public string Name { get; set; }
    public string GovID { get; set; }
    public string Grade { get; set; }
    public string SchoolDAN { get; set; }
    public string SourceSchoolFileName { get; set; }
    public string MSSWithdrawlDate { get; set; }

    public DateTime AssessmentDate { get; set; }

    public string Level { get; set; }

    public bool hasWithdrawDate()
    {
        return !string.IsNullOrEmpty(this.MSSWithdrawlDate);
    }

    public bool isValid()
    {
        if (
            (!string.IsNullOrEmpty(this.GovID)) &&
            (!string.IsNullOrEmpty(this.Name)) &&
            (!string.IsNullOrEmpty(this.Grade))
        )
        {
            if (
                ((this.GovID.Length > 0) && (this.GovID.Length <= 16)) &&
                ((this.Grade.Length > 0) && (this.Grade.Length <= 5)) &&
                ((this.Level.Length > 0) && (this.Level.Length <= 5))
                )
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        return $"StudentRecord \tGrade:'{this.Grade}' \tLID:'{this.GovID}' \tLVL:'{this.Level}' \tDate:'{this.AssessmentDate.ToShortDateString()}' \tDAN:'{this.SchoolDAN}' \tFile:'{this.SourceSchoolFileName}' \tWithdrawn:{this.hasWithdrawDate()}";
    }

}