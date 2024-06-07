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


public class FandPSpreadsheet
{
    public string FileName { get; set; }
    public string FullFilePath { get; set; }
    public string SchoolName { get; set; }
    public string SchoolDAN { get; set; }
    public string Teacher { get; set; }
    public string Grade { get; set; }
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