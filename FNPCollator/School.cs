namespace FNPCollator;

public class School
{
    public string Name { get; set; }
    public string DAN { get; set; }
    public List<String> Identifiers { get; set; } = new List<string>();

    public override string ToString()
    {
        return this.Name;
    }
}