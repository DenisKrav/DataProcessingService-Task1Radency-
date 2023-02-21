# DataProcessingService-Task1Radency-
A data processing service that allows you to process files with payment transactions that different users store in a specific folder 
(A) on the disk (the path is specified in the configuration and checked for availability). Formats that are processed: TXT or CSV.
Saving goes to a certain folder (B) on the disk (the path is specified in the configuration and is checked for availability). 
The format of the generated files is json files with the following structure:
[{
   "city": "string",
   "services": [{
     "name": "string",
     "payers": [{
       "name": "string",
       "payment": "decimal",
       "date": "date",
       "account_number": "long"
     }],
     "total": "decimal"
   }],
   "total": "decimal"
}]
An example file that will be processed by the application, lines with errors are not processed:
John, Doe, “Lviv, Kleparivska 35, 4”, 500.0, 2022-27-01, 1234567, Water
Mike, Wiksen, “Lviv, Kleparivska 40, 1”, 720.0, 2022-27-05, 7654321, Heat
After processing the file, the service saves the results in a separate folder (B) (the path is specified in the configuration) in a
subfolder (C) with the current date (for example, 09/21/2022). Also, at midnight, the application saves a file called "meta.log" in the (C) 
folder, which contains the following information about the service's operation for the day:
parsed_files: N
parsed_lines: K
found_errors: Z
invalid_files: [path1, path2, path3]
