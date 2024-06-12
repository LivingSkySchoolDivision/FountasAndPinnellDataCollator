using CommandLine; // https://github.com/commandlineparser/commandline

namespace FNPCollator;


class Program
{
    static void Main(string[] args)
    {
        try
        {
            // https://github.com/commandlineparser/commandline
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(Options =>
                {
                    // Sanity Checks
                    // Check the input folder path to make sure it exists and has files in it
                    string actualFolderPath = Path.GetFullPath(Options.InputFolderPath);
                    Options.InputFolderPath = actualFolderPath;

                    // Check the output file path
                    string actualOutputFilePath = Path.GetFullPath(Options.OutFile);
                    Options.OutFile = actualOutputFilePath;

                    if (!string.IsNullOrEmpty(Options.InputFilter))
                    {
                        if (Directory.Exists(actualFolderPath))
                        {
                            if (Options.Verbose)
                            {
                                Console.WriteLine("Input folder is: " + Options.InputFolderPath);
                                Console.WriteLine("Filter is " + Options.InputFilter);
                                Console.WriteLine("Output file is " + Options.OutFile);
                            }

                            if (File.Exists(Options.OutFile))
                            {
                                if (Options.Verbose)
                                {
                                    Console.WriteLine("WARNING: Output file exists - will be overridden");
                                }
                            }

                            // Attempt to load files from the directory
                            // Show data if we need to
                            List<FandPSpreadsheet> inputFiles = new List<FandPSpreadsheet>();

                            if (Options.Verbose)
                            {
                                Console.WriteLine("");
                            }

                            int failed_files = 0;
                            foreach(string file in Directory.GetFiles(Options.InputFolderPath, Options.InputFilter, SearchOption.TopDirectoryOnly))
                            {
                                if (Options.Verbose)
                                {
                                    Console.Write($"Loading: {Path.GetFileName(file)}... ");
                                }

                                try {
                                    FandPSpreadsheet importedFile = FandPSpreadsheetParser.ParseFandPExcelSpreadsheet(file);
                                    inputFiles.Add(importedFile);

                                    if (Options.Verbose)
                                    {
                                        ConsoleColor origColor = Console.ForegroundColor;
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("SUCCESS");
                                        Console.ForegroundColor = origColor;
                                    }

                                } catch (Exception ex)
                                {
                                    failed_files++;

                                    if (Options.Verbose)
                                    {
                                        ConsoleColor origColor = Console.ForegroundColor;
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("FAILED (" + ex.Message + ")");
                                        Console.ForegroundColor = origColor;
                                    }
                                }
                            }

                            if (Options.Verbose)
                            {
                                Console.WriteLine("Files:\n Success: " + inputFiles.Count + "\n Failure: " + failed_files);

                                Console.WriteLine("DAN\t\tDate\t\tRecords\tWithdrawn\tFile");
                                foreach(FandPSpreadsheet file in inputFiles)
                                {
                                    Console.WriteLine($"{file.SchoolDAN} \t{file.AssessmentDate.ToShortDateString()} \t{file.Records.Count} \t{file.Records.Where(x => x.hasWithdrawDate()).Count()} \t{file.FileName}");
                                }
                            }
                            
                            // Generate the output file
                            string file_output = CSVFileGenerator.GenerateCSV(inputFiles, Options.ShowHeaderOnOutput, Options.ShowFileNameOnOutput);

                            // Check to see if we need to delete an existing file
                            if (File.Exists(Options.OutFile))
                            {
                                Console.WriteLine("WARNING: Deleting existing file '" + Options.OutFile + "'!");
                                File.Delete(Options.OutFile);
                            }

                            using (StreamWriter outFile = new StreamWriter(Options.OutFile))
                            {
                                outFile.Write(file_output);
                            }

                        } else {
                            throw new Exception("Input folder does not exist.");
                        }
                    } else {
                        throw new Exception("Filter can't be empty.");
                    }
                });
        }
        catch(Exception ex)
        {
            Console.WriteLine("FAILED: " + ex.Message);
        }
    }
}