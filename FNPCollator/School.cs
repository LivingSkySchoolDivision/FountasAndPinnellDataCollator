namespace FNPCollator;

public class School
{
    public string Name { get; set; } = string.Empty;
    public string DAN { get; set; } = string.Empty;
    public List<String> Identifiers { get; set; } = new List<string>();

    public override string ToString()
    {
        return this.Name;
    }
}