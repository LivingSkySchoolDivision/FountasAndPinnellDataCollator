namespace FNPCollator;

public class FandPSpreadsheet
{
    public string FileName { get; set; } = string.Empty;
    public string FullFilePath { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string SchoolDAN { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public DateTime AssessmentDate { get; set; }
    
    public List<StudentRecord> Records { get; set; } = new List<StudentRecord>();

    public bool IsValid() 
    {
        if (
            (!string.IsNullOrEmpty(this.FileName)) &&
            (!string.IsNullOrEmpty(this.SchoolName)) &&
            (!string.IsNullOrEmpty(this.Teacher)) &&
            (!string.IsNullOrEmpty(this.Grade)) &&
            (this.AssessmentDate > new DateTime(1900,01,01)) &&
            (this.Records.Count > 0)
        )
        {
            return true;
        }        

        return false;
    }

}