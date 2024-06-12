namespace FNPCollator;

public class StudentRecord
{
    public string Name { get; set; } = string.Empty;
    public string GovID { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string SchoolDAN { get; set; } = string.Empty;
    public string SourceSchoolFileName { get; set; } = string.Empty;
    public string MSSWithdrawlDate { get; set; } = string.Empty;

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