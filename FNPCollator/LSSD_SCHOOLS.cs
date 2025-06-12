namespace FNPCollator;

public static class LSSD_SCHOOLS
{
    private static readonly List<School> Schools = new List<School>() {
        new School() { Name = "Battelford Central Elementary School", DAN = "5810211", Identifiers = new List<string>() { "Battleford Central School", "BCS" }},
        new School() { Name = "Bready Elementary School", DAN = "5850201", Identifiers = new List<string>() { "Bready" }},
        new School() { Name = "Cando Community School", DAN = "5010213", Identifiers = new List<string>() { "Cando" }},
        new School() { Name = "Connaught Community School", DAN = "5850401", Identifiers = new List<string>() { "Connaught", "Connaught School" }},
        new School() { Name = "Cut Knife Community School", DAN = "5910123", Identifiers = new List<string>() { "CKCS" }},
        new School() { Name = "Hafford Central School", DAN = "5710213", Identifiers = new List<string>() { "Hafford" }},
        new School() { Name = "Hartley Clark School", DAN = "6410721", Identifiers = new List<string>() { "Hartley Clark", "HCES" }},
        new School() { Name = "Heritage Christian School", DAN = "5894003", Identifiers = new List<string>() { "Heritage Christian", "Heritage", "Heritage Christian " }},
        new School() { Name = "Hillsvale Colony School", DAN = "5910313", Identifiers = new List<string>() { "Hillsvale" }},
        new School() { Name = "Home Based", DAN = "2020500", Identifiers = new List<string>() { }},
        new School() { Name = "Kerrobert Composite School", DAN = "4410223", Identifiers = new List<string>() { "Kerrobert" }},
        new School() { Name = "Lakeview Colony School", DAN = "5911011", Identifiers = new List<string>() { "Lakeview" }},
        new School() { Name = "Lawrence Elementary School", DAN = "5850501", Identifiers = new List<string>() { "Lawrence School", "Lawrence" }},
        new School() { Name = "Leoville Central School", DAN = "6410313", Identifiers = new List<string>() { "Leoville" }},
        new School() { Name = "Luseland School", DAN = "4410323", Identifiers = new List<string>() { "Luseland" }},
        new School() { Name = "Macklin School", DAN = "4410413", Identifiers = new List<string>() { "Macklin" }},
        new School() { Name = "Manacowin School", DAN = "7350113", Identifiers = new List<string>() { }},
        new School() { Name = "Maymont Central School", DAN = "5810713", Identifiers = new List<string>() { "Maymont" }},
        new School() { Name = "McKitrick Community School", DAN = "5850601", Identifiers = new List<string>() { "McKitrick" }},
        new School() { Name = "McLurg High School", DAN = "5910923", Identifiers = new List<string>() { }},
        new School() { Name = "Meadow Lake Christian Academy", DAN = "6694003", Identifiers = new List<string>() { "MLCA" }},
        new School() { Name = "Medstead Central School", DAN = "6410513", Identifiers = new List<string>() { "Medstead" }},
        new School() { Name = "Newmark Colony School", DAN = "6710722", Identifiers = new List<string>() { "Newmark" }},
        new School() { Name = "Norman Carter Elementary School", DAN = "5910911", Identifiers = new List<string>() { "Norman Carter", "Norman Carter School" }},
        new School() { Name = "North Battleford Comprehensive High School", DAN = "5850904", Identifiers = new List<string>() { }},
        new School() { Name = "Scott Colony School", DAN = "5911113", Identifiers = new List<string>() { "Scott" }},
        new School() { Name = "Spiritwood High School", DAN = "6410713", Identifiers = new List<string>() { }},
        new School() { Name = "St. Vital Catholic School", DAN = "5810221", Identifiers = new List<string>() { "St. Vital", "St Vital" }},
        new School() { Name = "Unity Composite High School", DAN = "5910813", Identifiers = new List<string>() { }},
        new School() { Name = "Unity Public School", DAN = "5910711", Identifiers = new List<string>() { "UPS" }}
    };

    public static string FindSchoolDAN(string input)
    {
        // Check exact name matches
        foreach (School school in LSSD_SCHOOLS.Schools)
        {
            if (String.Equals(school.Name.ToLower(), input.ToLower()))
            {
                return school.DAN;
            }
        }

        // Check identifier matches
        foreach (School school in LSSD_SCHOOLS.Schools)
        {
            foreach (string term in school.Identifiers)
            {
                if (String.Equals(term.ToLower(), input.ToLower()))
                {
                    return school.DAN;
                }
            }
        }

        throw new Exception("School not found: " + input);
    }

}