# Fountas and Pinnell Data Collator
For collecting F&P data from multiple files and collating that data into a single CSV file for ingestion/upload. 

This utility is very specific to how Living Sky School Division No. 202 collects F&P data from our teachers. An example file can be found in the `ExampleFiles` directory of this repository.

You can obtain the EXE for this utility [HERE](https://github.com/LivingSkySchoolDivision/FountasAndPinnellDataCollator/releases/latest/download/FNPCollator.exe).

# Command line options
| Command                  | Description                                                                                    |
|--------------------------|------------------------------------------------------------------------------------------------|
| `-o`, `--out`                | Filename for the output CSV file.                                                              |
| `-i`, `--infolder`           | The path to a folder containing Excel files that each have F&P data for a single class/teacher.|
| `-f`, `--filter`             | File filter for the input folder. Example: `L_23-24*.xlsx`                                     |
| `-h`, `--header`             | Show header on the output CSV file. This will cause the import to fail, but is useful for troubleshooting. |
| `--analyze`            | Instead of creating a collated CSV, analyze the data instead and display information that may help with troubleshooting or data checking. More data is displayed when paired with `--verbose` or `-v`. |
| `--showsourcefilenames`    | Show source (Excel) filenames in the output CSV.  This will cause the import to fail, but is useful for troubleshooting. |
| `-v`, `--verbose`            | Show extra information on the command line regarding file processing.                          |


# Building from source
```
dotnet publish -r win-x64 -p:PublishSingleFile=true -c Release --self-contained true -p:PublishTrimmed=true
```

The finished exe can be found under `bin/Release/net8.0/winx64/FNPCollator.exe`

## Running without building, using the `dotnet` command
```
dotnet run -- -i "path\to\excel\file\folder\" --header -o "C:\my_output_file.csv"
```