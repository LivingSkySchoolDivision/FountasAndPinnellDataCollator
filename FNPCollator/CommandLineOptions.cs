
using CommandLine; // https://github.com/commandlineparser/commandline

namespace FNPCollator;

public class CommandLineOptions
{
        [Option('i',"infolder", Required = true, HelpText = "The folder that contains the F&P data spreadsheets to collate.")]
        public string InputFolderPath { get;set; } = string.Empty;

        [Option('o',"out", Required = true, HelpText = "The filename for the output CSV file.")]
        public string OutFile { get;set; } = string.Empty;

        [Option('f',"filter", Required = false, HelpText = "A filter to use for reading files. Defaults to '*.xlsx'.")]
        public string InputFilter { get;set; } = "*.xlsx";

        [Option('h', "header", Required = false, HelpText = "Include a header on the output file, which shows you field names. The final file does not need one, but it is helpful for troubleshooting or sanity checks on the data.")]
        public bool ShowHeaderOnOutput { get; set; }

        [Option("showsourcefilenames", Required = false, HelpText = "Include the source filenames in the output CSV. The final file does not need this, but it is helpful for troubleshooting or sanity checks on the data.")]
        public bool ShowFileNameOnOutput { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Show extra information on the console as the program runs.")]
        public bool Verbose { get; set; }
}