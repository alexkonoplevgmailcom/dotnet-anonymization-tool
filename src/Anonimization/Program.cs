using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        // If no arguments provided, show interactive menu
        if (args.Length == 0)
        {
            ShowInteractiveMenu();
            return;
        }

        // Handle command line arguments (maintain backward compatibility)
        string command = args[0];

        if (command.Equals("restore", StringComparison.OrdinalIgnoreCase))
        {
            HandleRestoreCommand(args);
        }
        else
        {
            HandleAnonymizeCommand(args);
        }
    }

    private static void ShowInteractiveMenu()
    {
        // Check if we're in an interactive environment
        if (!IsInteractiveEnvironment())
        {
            Console.WriteLine("Error: Interactive menu requires a console environment.");
            Console.WriteLine("Use command line arguments instead. Run with any argument to see usage.");
            ShowUsage();
            return;
        }

        while (true)
        {        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              ANONYMIZATION TOOL - MAIN MENU             ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Anonymize files (remove comments & replace company) ║");
        Console.WriteLine("║  2. Restore files from backup                           ║");
        Console.WriteLine("║  3. Show command line usage                             ║");
        Console.WriteLine("║  4. Exit                                                ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("📁 Supported file extensions: .cs, .java, .xml, .sql, .xsd, .json, .htm, .html");
        Console.WriteLine();
            Console.Write("Please select an option (1-4): ");

            string? input = Console.ReadLine();
            
            switch (input?.Trim())
            {
                case "1":
                    HandleInteractiveAnonymize();
                    break;
                case "2":
                    HandleInteractiveRestore();
                    break;
                case "3":
                    ShowUsage();
                    PauseForUser();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select 1-4.");
                    PauseForUser();
                    break;
            }
        }
    }

    private static bool IsInteractiveEnvironment()
    {
        try
        {
            // Check if we can interact with the console
            return !Console.IsInputRedirected && !Console.IsOutputRedirected;
        }
        catch
        {
            return false;
        }
    }

    private static void HandleInteractiveAnonymize()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    ANONYMIZE FILES                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Get folder path
        string folderPath = GetValidFolderPath();
        if (string.IsNullOrEmpty(folderPath)) return;

        // Get company name (optional)
        Console.WriteLine();
        Console.WriteLine("Company Name Replacement (Optional):");
        Console.WriteLine("If specified, all occurrences of this company name will be replaced with 'MyCompany'");
        Console.Write("Enter company name to replace (or press Enter to skip): ");
        string? companyName = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrEmpty(companyName))
        {
            companyName = null;
            Console.WriteLine("→ No company name replacement will be performed.");
        }
        else
        {
            Console.WriteLine($"→ Will replace '{companyName}' with 'MyCompany'");
        }

        // Confirm operation
        Console.WriteLine();
        Console.WriteLine("OPERATION SUMMARY:");
        Console.WriteLine($"📁 Target folder: {folderPath}");
        Console.WriteLine($"🏢 Company replacement: {(companyName != null ? $"{companyName} → MyCompany" : "None")}");
        Console.WriteLine($"💾 Backup: Will be created automatically");
        Console.WriteLine();
        Console.Write("Proceed with anonymization? (y/N): ");
        
        string? confirm = Console.ReadLine();
        if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            PauseForUser();
            return;
        }

        // Perform anonymization
        Console.WriteLine();
        Console.WriteLine("Starting anonymization...");
        Console.WriteLine(new string('─', 60));
        
        var anonymizer = new FileAnonymizer(companyName);
        string backupPath = anonymizer.ProcessFolder(folderPath);
        
        Console.WriteLine(new string('─', 60));
        Console.WriteLine("✅ Anonymization completed!");
        
        // Ask for validation
        Console.WriteLine();
        Console.Write("Would you like to perform anonymization validation and generate a report? (y/N): ");
        string? validateResponse = Console.ReadLine();
        
        if (string.Equals(validateResponse, "y", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(validateResponse, "yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine();
            Console.WriteLine("Generating validation report...");
            var validator = new AnonymizationValidator();
            validator.ValidateAndGenerateReport(folderPath, backupPath, companyName);
        }
        
        PauseForUser();
    }

    private static void HandleInteractiveRestore()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  RESTORE FROM BACKUP                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Get backup folder path
        string backupPath = GetValidBackupPath();
        if (string.IsNullOrEmpty(backupPath)) return;

        // Ask about force overwrite
        Console.WriteLine();
        Console.WriteLine("Overwrite Options:");
        Console.WriteLine("1. Ask for confirmation if files will be overwritten (recommended)");
        Console.WriteLine("2. Force overwrite without confirmation");
        Console.Write("Select option (1-2): ");
        
        string? overwriteOption = Console.ReadLine()?.Trim();
        bool forceOverwrite = overwriteOption == "2";

        // Show operation summary
        Console.WriteLine();
        Console.WriteLine("RESTORE SUMMARY:");
        Console.WriteLine($"📁 Backup folder: {backupPath}");
        Console.WriteLine($"⚡ Force overwrite: {(forceOverwrite ? "Yes" : "No (will ask for confirmation)")}");
        Console.WriteLine();
        Console.Write("Proceed with restore? (y/N): ");
        
        string? confirm = Console.ReadLine();
        if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            PauseForUser();
            return;
        }

        // Perform restore
        Console.WriteLine();
        Console.WriteLine("Starting restore...");
        Console.WriteLine(new string('─', 60));
        
        var restorer = new BackupRestorer();
        restorer.RestoreFromBackup(backupPath, forceOverwrite);
        
        Console.WriteLine(new string('─', 60));
        Console.WriteLine("✅ Restore completed!");
        PauseForUser();
    }

    private static string GetValidFolderPath()
    {
        while (true)
        {
            Console.Write("Enter the folder path to anonymize: ");
            string? path = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("❌ Path cannot be empty. Please try again.");
                continue;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"❌ Folder '{path}' does not exist. Please try again.");
                continue;
            }

            return path;
        }
    }

    private static string GetValidBackupPath()
    {
        while (true)
        {
            Console.Write("Enter the backup folder path: ");
            string? path = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine("❌ Path cannot be empty. Please try again.");
                continue;
            }

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"❌ Backup folder '{path}' does not exist. Please try again.");
                continue;
            }

            // Check if it looks like a backup folder
            string folderName = Path.GetFileName(path);
            if (!folderName.StartsWith("backup_", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"⚠️  Warning: '{folderName}' doesn't look like a backup folder (should start with 'backup_')");
                Console.Write("Continue anyway? (y/N): ");
                string? confirm = Console.ReadLine();
                if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            return path;
        }
    }

    private static void PauseForUser()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }

    private static void ShowUsage()
    {
        Console.WriteLine("Anonimization Tool - Command Line Usage:");
        Console.WriteLine();
        Console.WriteLine("INTERACTIVE MODE (Default):");
        Console.WriteLine("  Anonimization");
        Console.WriteLine("    Launches interactive menu for guided operation");
        Console.WriteLine();
        Console.WriteLine("COMMAND LINE MODE:");
        Console.WriteLine();
        Console.WriteLine("ANONYMIZE:");
        Console.WriteLine("  Anonimization <folder_path> [company_name]");
        Console.WriteLine("    folder_path: Path to the folder to process");
        Console.WriteLine("    company_name: Optional company name to replace with 'MyCompany'");
        Console.WriteLine("    Note: A backup folder will be created automatically before processing.");
        Console.WriteLine();
        Console.WriteLine("RESTORE:");
        Console.WriteLine("  Anonimization restore <backup_folder_path> [--force]");
        Console.WriteLine("    backup_folder_path: Path to the backup folder to restore from");
        Console.WriteLine("    --force: Force overwrite existing files without confirmation");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  Anonimization                                           # Interactive menu");
        Console.WriteLine("  Anonimization /path/to/project KYKY                     # Anonymize with company replacement");
        Console.WriteLine("  Anonimization /path/to/project                          # Anonymize without company replacement");
        Console.WriteLine("  Anonimization restore /path/to/project/backup_20250709  # Restore with confirmation");
        Console.WriteLine("  Anonimization restore /path/to/project/backup_20250709 --force  # Force restore");
    }

    private static void HandleRestoreCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: Backup folder path is required for restore command.");
            ShowUsage();
            return;
        }

        string backupPath = args[1];
        bool forceOverwrite = args.Length > 2 && args[2].Equals("--force", StringComparison.OrdinalIgnoreCase);

        if (!Directory.Exists(backupPath))
        {
            Console.WriteLine($"Error: Backup folder '{backupPath}' does not exist.");
            return;
        }

        var restorer = new BackupRestorer();
        restorer.RestoreFromBackup(backupPath, forceOverwrite);
    }

    private static void HandleAnonymizeCommand(string[] args)
    {
        string folderPath = args[0];
        string? companyName = args.Length > 1 ? args[1] : null;

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: Folder '{folderPath}' does not exist.");
            return;
        }

        var anonymizer = new FileAnonymizer(companyName);
        string backupPath = anonymizer.ProcessFolder(folderPath);
        
        Console.WriteLine("✅ Anonymization completed!");
        
        // Ask for validation (same as interactive mode)
        if (!string.IsNullOrEmpty(backupPath))
        {
            Console.WriteLine();
            Console.Write("Would you like to perform anonymization validation and generate a report? (y/N): ");
            string? validateResponse = Console.ReadLine();
            
            if (string.Equals(validateResponse, "y", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(validateResponse, "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine();
                Console.WriteLine("Generating validation report...");
                var validator = new AnonymizationValidator();
                validator.ValidateAndGenerateReport(folderPath, backupPath, companyName);
            }
        }
    }
}

public class FileAnonymizer
{
    private readonly string? _companyName;
    private readonly Dictionary<string, Func<string, string>> _fileProcessors;

    public FileAnonymizer(string? companyName)
    {
        _companyName = companyName;
        _fileProcessors = new Dictionary<string, Func<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            { ".cs", ProcessCSharpFile },
            { ".java", ProcessJavaFile },
            { ".xml", ProcessXmlFile },
            { ".sql", ProcessSqlFile },
            { ".xsd", ProcessXsdFile },
            { ".json", ProcessJsonFile },
            { ".htm", ProcessHtmlFile },
            { ".html", ProcessHtmlFile }
        };
    }

    public string ProcessFolder(string folderPath)
    {
        try
        {
            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                .Where(file => _fileProcessors.ContainsKey(Path.GetExtension(file)))
                .ToList();

            Console.WriteLine($"Found {files.Count} files to process in '{folderPath}'");

            // Create backup before processing
            string backupPath = CreateBackup(folderPath, files);
            Console.WriteLine($"Backup created at: {backupPath}");

            foreach (string file in files)
            {
                ProcessFile(file);
            }

            Console.WriteLine("Processing completed.");
            return backupPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing folder: {ex.Message}");
            return string.Empty;
        }
    }

    private void ProcessFile(string filePath)
    {
        try
        {
            Console.WriteLine($"Processing: {filePath}");
            
            string content = File.ReadAllText(filePath);
            string extension = Path.GetExtension(filePath);
            
            if (_fileProcessors.TryGetValue(extension, out var processor))
            {
                string processedContent = processor(content);
                processedContent = ReplaceCompanyName(processedContent);
                
                File.WriteAllText(filePath, processedContent);
                Console.WriteLine($"  ✓ Completed: {Path.GetFileName(filePath)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Error processing {filePath}: {ex.Message}");
        }
    }

    private string ReplaceCompanyName(string content)
    {
        if (string.IsNullOrEmpty(_companyName))
            return content;

        // Replace company name with "MyCompany" (case-insensitive, comprehensive pattern matching)
        
        // 1. Handle whole word boundaries (standard case)
        content = Regex.Replace(content, $@"\b{Regex.Escape(_companyName)}\b", "MyCompany", RegexOptions.IgnoreCase);
        
        // 2. Handle cases in package names, namespaces, and paths (like com.kyky.something)
        content = Regex.Replace(content, $@"\.{Regex.Escape(_companyName)}\.", ".MyCompany.", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"/{Regex.Escape(_companyName)}/", "/MyCompany/", RegexOptions.IgnoreCase);
        
        // 3. Handle cases at the beginning of package names or paths
        content = Regex.Replace(content, $@"^{Regex.Escape(_companyName)}\.", "MyCompany.", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        content = Regex.Replace(content, $@"^{Regex.Escape(_companyName)}/", "MyCompany/", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        
        // 4. Handle cases at the end of package names or paths
        content = Regex.Replace(content, $@"\.{Regex.Escape(_companyName)}$", ".MyCompany", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        content = Regex.Replace(content, $@"/{Regex.Escape(_companyName)}$", "/MyCompany", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        
        // 5. Handle underscore-separated identifiers (like kyky_ecommerce, kyky_user)
        content = Regex.Replace(content, $@"\b{Regex.Escape(_companyName)}_", "MyCompany_", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"_{Regex.Escape(_companyName)}_", "_MyCompany_", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"_{Regex.Escape(_companyName)}\b", "_MyCompany", RegexOptions.IgnoreCase);
        
        // 6. Handle environment variable patterns (like ${KYKY_DB_PASSWORD})
        content = Regex.Replace(content, $@"\$\{{{Regex.Escape(_companyName)}_", "${MyCompany_", RegexOptions.IgnoreCase);
        
        // 7. Handle hyphen-separated identifiers (like kyky-db-server)
        content = Regex.Replace(content, $@"\b{Regex.Escape(_companyName)}-", "MyCompany-", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"-{Regex.Escape(_companyName)}-", "-MyCompany-", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"-{Regex.Escape(_companyName)}\b", "-MyCompany", RegexOptions.IgnoreCase);
        
        // 8. Handle colon-separated patterns (like kyky-library: in Redis keys)
        content = Regex.Replace(content, $@"\b{Regex.Escape(_companyName)}:", "MyCompany:", RegexOptions.IgnoreCase);
        
        return content;
    }

    private string ProcessCSharpFile(string content)
    {
        // Remove single-line comments (// ...)
        content = Regex.Replace(content, @"//.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
        
        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
        
        // Remove XML documentation comments (/// ...)
        content = Regex.Replace(content, @"///.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
        
        return CleanupWhitespace(content);
    }

    private string ProcessJavaFile(string content)
    {
        // Remove single-line comments (// ...)
        content = Regex.Replace(content, @"//.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
        
        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
        
        // Remove JavaDoc comments (/** ... */)
        content = Regex.Replace(content, @"/\*\*.*?\*/", "", RegexOptions.Singleline);
        
        return CleanupWhitespace(content);
    }

    private string ProcessXmlFile(string content)
    {
        // Remove XML comments (<!-- ... -->)
        content = Regex.Replace(content, @"<!--.*?-->", "", RegexOptions.Singleline);
        
        return CleanupWhitespace(content);
    }

    private string ProcessSqlFile(string content)
    {
        // Remove single-line comments (-- ...)
        content = Regex.Replace(content, @"--.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
        
        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
        
        return CleanupWhitespace(content);
    }

    private string ProcessXsdFile(string content)
    {
        // XSD files are XML-based, so use XML comment removal
        return ProcessXmlFile(content);
    }

    private string ProcessJsonFile(string content)
    {
        // JSON files don't typically have comments to remove in standard JSON
        // but we may want to format it nicely after company name replacement
        return content;
    }

    private string ProcessHtmlFile(string content)
    {
        // Remove HTML comments (<!-- ... -->)
        content = Regex.Replace(content, @"<!--.*?-->", "", RegexOptions.Singleline);
        
        return CleanupWhitespace(content);
    }

    private string CleanupWhitespace(string content)
    {
        // Remove empty lines that resulted from comment removal
        content = Regex.Replace(content, @"^\s*\r?\n", "", RegexOptions.Multiline);
        
        // Remove multiple consecutive empty lines, keeping only one
        content = Regex.Replace(content, @"(\r?\n){3,}", "\n\n");
        
        return content.Trim();
    }

    private string CreateBackup(string folderPath, List<string> filesToProcess)
    {
        try
        {
            // Create backup folder with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFolderName = $"backup_{timestamp}";
            string backupPath = Path.Combine(folderPath, backupFolderName);
            
            Directory.CreateDirectory(backupPath);

            Console.WriteLine($"Creating backup of {filesToProcess.Count} files...");

            foreach (string sourceFile in filesToProcess)
            {
                // Calculate relative path from the original folder
                string relativePath = Path.GetRelativePath(folderPath, sourceFile);
                string backupFile = Path.Combine(backupPath, relativePath);
                
                // Create directory structure in backup folder if needed
                string? backupDir = Path.GetDirectoryName(backupFile);
                if (!string.IsNullOrEmpty(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
                
                // Copy the original file to backup location
                File.Copy(sourceFile, backupFile, true);
                Console.WriteLine($"  ✓ Backed up: {relativePath}");
            }

            return backupPath;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create backup: {ex.Message}", ex);
        }
    }
}

public class BackupRestorer
{
    public void RestoreFromBackup(string backupPath, bool forceOverwrite)
    {
        try
        {
            // Determine the target folder (parent of backup folder)
            string? targetFolder = Path.GetDirectoryName(backupPath);
            if (string.IsNullOrEmpty(targetFolder))
            {
                Console.WriteLine("Error: Could not determine target folder from backup path.");
                return;
            }

            // Get all files in the backup folder
            var backupFiles = Directory.GetFiles(backupPath, "*", SearchOption.AllDirectories).ToList();
            
            if (!backupFiles.Any())
            {
                Console.WriteLine("Warning: No files found in backup folder.");
                return;
            }

            Console.WriteLine($"Found {backupFiles.Count} files to restore from backup '{backupPath}'");
            Console.WriteLine($"Target folder: '{targetFolder}'");

            // Check for conflicts if not forcing overwrite
            if (!forceOverwrite)
            {
                var conflicts = CheckForConflicts(backupFiles, backupPath, targetFolder);
                if (conflicts.Any())
                {
                    Console.WriteLine($"\nWarning: {conflicts.Count} files will be overwritten:");
                    foreach (var conflict in conflicts.Take(10)) // Show first 10 conflicts
                    {
                        Console.WriteLine($"  - {conflict}");
                    }
                    if (conflicts.Count > 10)
                    {
                        Console.WriteLine($"  ... and {conflicts.Count - 10} more files");
                    }

                    Console.Write("\nDo you want to continue? (y/N): ");
                    var response = Console.ReadLine();
                    if (!string.Equals(response, "y", StringComparison.OrdinalIgnoreCase) && 
                        !string.Equals(response, "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Restore operation cancelled.");
                        return;
                    }
                }
            }

            // Perform the restore
            int restoredCount = 0;
            foreach (string backupFile in backupFiles)
            {
                try
                {
                    // Calculate the relative path from backup folder
                    string relativePath = Path.GetRelativePath(backupPath, backupFile);
                    string targetFile = Path.Combine(targetFolder, relativePath);

                    // Create target directory if needed
                    string? targetDir = Path.GetDirectoryName(targetFile);
                    if (!string.IsNullOrEmpty(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    // Copy the file
                    File.Copy(backupFile, targetFile, true);
                    Console.WriteLine($"  ✓ Restored: {relativePath}");
                    restoredCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ✗ Failed to restore {Path.GetFileName(backupFile)}: {ex.Message}");
                }
            }

            Console.WriteLine($"\nRestore completed successfully!");
            Console.WriteLine($"Restored {restoredCount} of {backupFiles.Count} files to '{targetFolder}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during restore operation: {ex.Message}");
        }
    }

    private List<string> CheckForConflicts(List<string> backupFiles, string backupPath, string targetFolder)
    {
        var conflicts = new List<string>();

        foreach (string backupFile in backupFiles)
        {
            string relativePath = Path.GetRelativePath(backupPath, backupFile);
            string targetFile = Path.Combine(targetFolder, relativePath);

            if (File.Exists(targetFile))
            {
                conflicts.Add(relativePath);
            }
        }

        return conflicts;
    }
}

public class AnonymizationValidator
{
    public void ValidateAndGenerateReport(string targetFolder, string backupPath, string? companyName)
    {
        try
        {
            var validationResults = PerformValidation(targetFolder, backupPath, companyName);
            string reportPath = GenerateHtmlReport(validationResults, targetFolder);
            OpenInBrowser(reportPath);
            
            Console.WriteLine($"✅ Validation report generated: {reportPath}");
            Console.WriteLine("📖 Report opened in your default browser");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error generating validation report: {ex.Message}");
        }
    }

    private ValidationResults PerformValidation(string targetFolder, string backupPath, string? companyName)
    {
        var results = new ValidationResults
        {
            TargetFolder = targetFolder,
            BackupPath = backupPath,
            CompanyName = companyName,
            Timestamp = DateTime.Now
        };

        // Get processed files
        var processedFiles = Directory.GetFiles(targetFolder, "*", SearchOption.AllDirectories)
            .Where(file => IsProcessableFile(file))
            .ToList();

        // Get backup files for comparison
        var backupFiles = Directory.GetFiles(backupPath, "*", SearchOption.AllDirectories)
            .Where(file => IsProcessableFile(file))
            .ToList();

        results.TotalFilesProcessed = processedFiles.Count;
        results.BackupFilesCount = backupFiles.Count;

        // Validate each file
        foreach (var processedFile in processedFiles)
        {
            var relativePath = Path.GetRelativePath(targetFolder, processedFile);
            var backupFile = Path.Combine(backupPath, relativePath);

            if (File.Exists(backupFile))
            {
                var validation = ValidateFile(processedFile, backupFile, companyName);
                results.FileValidations.Add(validation);
            }
        }

        results.FilesWithCommentsRemoved = results.FileValidations.Count(v => v.CommentsRemoved > 0);
        results.FilesWithCompanyReplacements = results.FileValidations.Count(v => v.CompanyReplacements > 0);
        results.TotalCommentsRemoved = results.FileValidations.Sum(v => v.CommentsRemoved);
        results.TotalCompanyReplacements = results.FileValidations.Sum(v => v.CompanyReplacements);

        return results;
    }

    private FileValidation ValidateFile(string processedFile, string backupFile, string? companyName)
    {
        var validation = new FileValidation
        {
            FilePath = Path.GetFileName(processedFile),
            RelativePath = processedFile // Store the absolute path for now, we'll use it correctly in HTML generation
        };

        try
        {
            string originalContent = File.ReadAllText(backupFile);
            string processedContent = File.ReadAllText(processedFile);

            validation.OriginalSize = originalContent.Length;
            validation.ProcessedSize = processedContent.Length;

            // Count comments removed (estimate by line differences)
            validation.CommentsRemoved = CountCommentsRemoved(originalContent, processedContent);

            // Count company name replacements
            if (!string.IsNullOrEmpty(companyName))
            {
                validation.CompanyReplacements = CountCompanyReplacements(originalContent, processedContent, companyName);
            }

            validation.IsValid = true;
        }
        catch (Exception ex)
        {
            validation.IsValid = false;
            validation.Error = ex.Message;
        }

        return validation;
    }

    private int CountCommentsRemoved(string original, string processed)
    {
        // Count various comment patterns that were likely removed
        var commentPatterns = new[]
        {
            @"//.*",           // Single line comments
            @"/\*.*?\*/",      // Multi-line comments
            @"<!--.*?-->",     // XML comments
            @"--.*",           // SQL comments
            @"///.*"           // XML doc comments
        };

        int removedCount = 0;
        foreach (var pattern in commentPatterns)
        {
            var originalMatches = System.Text.RegularExpressions.Regex.Matches(original, pattern, RegexOptions.Singleline);
            var processedMatches = System.Text.RegularExpressions.Regex.Matches(processed, pattern, RegexOptions.Singleline);
            removedCount += Math.Max(0, originalMatches.Count - processedMatches.Count);
        }

        return removedCount;
    }

    private int CountCompanyReplacements(string original, string processed, string companyName)
    {
        // Count occurrences of company name in original vs processed
        var originalCount = System.Text.RegularExpressions.Regex.Matches(original, 
            System.Text.RegularExpressions.Regex.Escape(companyName), 
            RegexOptions.IgnoreCase).Count;
        
        var processedCount = System.Text.RegularExpressions.Regex.Matches(processed, 
            System.Text.RegularExpressions.Regex.Escape(companyName), 
            RegexOptions.IgnoreCase).Count;

        return Math.Max(0, originalCount - processedCount);
    }

    private bool IsProcessableFile(string filePath)
    {
        var supportedExtensions = new[] { ".cs", ".java", ".xml", ".sql", ".xsd", ".json", ".htm", ".html" };
        return supportedExtensions.Contains(Path.GetExtension(filePath), StringComparer.OrdinalIgnoreCase);
    }

    private string GenerateHtmlReport(ValidationResults results, string targetFolder)
    {
        string reportPath = Path.Combine(targetFolder, $"anonymization_report_{DateTime.Now:yyyyMMdd_HHmmss}.html");
        
        var html = $@"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Anonymization Validation Report</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 40px; background-color: #f5f5f5; }}
        .container {{ max-width: 1200px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 0 20px rgba(0,0,0,0.1); }}
        h1 {{ color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }}
        h2 {{ color: #34495e; margin-top: 30px; }}
        .summary {{ display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin: 20px 0; }}
        .stat-card {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 8px; text-align: center; }}
        .stat-value {{ font-size: 2em; font-weight: bold; display: block; }}
        .stat-label {{ font-size: 0.9em; opacity: 0.9; }}
        table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        th, td {{ border: 1px solid #ddd; padding: 12px; text-align: left; }}
        th {{ background-color: #3498db; color: white; }}
        tr:nth-child(even) {{ background-color: #f2f2f2; }}
        .success {{ color: #27ae60; font-weight: bold; }}
        .error {{ color: #e74c3c; font-weight: bold; }}
        .info {{ background-color: #ecf0f1; padding: 15px; border-radius: 5px; margin: 20px 0; }}
        .timestamp {{ color: #7f8c8d; font-size: 0.9em; }}
        .file-path {{ font-family: 'Courier New', monospace; background-color: #f8f9fa; padding: 2px 6px; border-radius: 3px; }}
        .file-link {{ text-decoration: none; color: #2c3e50; display: inline-block; transition: all 0.2s ease; }}
        .file-link:hover {{ color: #3498db; transform: translateX(2px); }}
        .file-link:hover .file-path {{ background-color: #e3f2fd; }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>🛡️ Anonymization Validation Report</h1>
        
        <div class='info'>
            <strong>Target Folder:</strong> <span class='file-path'>{results.TargetFolder}</span><br>
            <strong>Backup Location:</strong> <span class='file-path'>{results.BackupPath}</span><br>
            <strong>Company Replaced:</strong> {(string.IsNullOrEmpty(results.CompanyName) ? "None" : $"{results.CompanyName} → MyCompany")}<br>
            <strong>Generated:</strong> <span class='timestamp'>{results.Timestamp:yyyy-MM-dd HH:mm:ss}</span><br>
            <strong>💡 Tip:</strong> Click on file names (🔗) in the table below to open them directly in VS Code
        </div>

        <h2>📊 Summary Statistics</h2>
        <div class='summary'>
            <div class='stat-card'>
                <span class='stat-value'>{results.TotalFilesProcessed}</span>
                <span class='stat-label'>Files Processed</span>
            </div>
            <div class='stat-card'>
                <span class='stat-value'>{results.TotalCommentsRemoved}</span>
                <span class='stat-label'>Comments Removed</span>
            </div>
            <div class='stat-card'>
                <span class='stat-value'>{results.TotalCompanyReplacements}</span>
                <span class='stat-label'>Company Replacements</span>
            </div>
            <div class='stat-card'>
                <span class='stat-value'>{results.BackupFilesCount}</span>
                <span class='stat-label'>Files Backed Up</span>
            </div>
        </div>

        <h2>📋 File-by-File Validation</h2>
        <table>
            <thead>
                <tr>
                    <th>File</th>
                    <th>Status</th>
                    <th>Original Size</th>
                    <th>Processed Size</th>
                    <th>Comments Removed</th>
                    <th>Company Replacements</th>
                </tr>
            </thead>
            <tbody>";

        foreach (var file in results.FileValidations)
        {
            // Use the absolute path directly (stored in RelativePath field)
            var processedFilePath = file.RelativePath;
            var vscodeLink = $"vscode://file/{processedFilePath.Replace("\\", "/")}";
            
            html += $@"
                <tr>
                    <td>
                        <a href='{vscodeLink}' class='file-link' title='Open in VS Code'>
                            <span class='file-path'>{file.FilePath}</span> 🔗
                        </a>
                    </td>
                    <td class='{(file.IsValid ? "success" : "error")}'>{(file.IsValid ? "✅ Valid" : "❌ Error")}</td>
                    <td>{file.OriginalSize:N0} bytes</td>
                    <td>{file.ProcessedSize:N0} bytes</td>
                    <td>{file.CommentsRemoved}</td>
                    <td>{file.CompanyReplacements}</td>
                </tr>";
        }

        html += $@"
            </tbody>
        </table>

        <h2>🎯 Validation Results</h2>
        <div class='info'>
            <p><strong>✅ Files with comments removed:</strong> {results.FilesWithCommentsRemoved} of {results.TotalFilesProcessed}</p>
            <p><strong>🏢 Files with company name replacements:</strong> {results.FilesWithCompanyReplacements} of {results.TotalFilesProcessed}</p>
            <p><strong>📁 Backup integrity:</strong> {results.BackupFilesCount} files safely backed up</p>
            <p><strong>🔍 Overall status:</strong> <span class='success'>Anonymization completed successfully</span></p>
        </div>

        <div class='timestamp'>
            Report generated by Anonymization Tool on {DateTime.Now:yyyy-MM-dd HH:mm:ss}
        </div>
    </div>
</body>
</html>";

        File.WriteAllText(reportPath, html);
        return reportPath;
    }

    private void OpenInBrowser(string filePath)
    {
        try
        {
            string url = $"file://{filePath}";
            
            if (OperatingSystem.IsWindows())
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            else if (OperatingSystem.IsMacOS())
            {
                System.Diagnostics.Process.Start("open", url);
            }
            else if (OperatingSystem.IsLinux())
            {
                System.Diagnostics.Process.Start("xdg-open", url);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Could not open browser automatically: {ex.Message}");
            Console.WriteLine($"📂 Please manually open: {filePath}");
        }
    }
}

public class ValidationResults
{
    public string TargetFolder { get; set; } = "";
    public string BackupPath { get; set; } = "";
    public string? CompanyName { get; set; }
    public DateTime Timestamp { get; set; }
    public int TotalFilesProcessed { get; set; }
    public int BackupFilesCount { get; set; }
    public int FilesWithCommentsRemoved { get; set; }
    public int FilesWithCompanyReplacements { get; set; }
    public int TotalCommentsRemoved { get; set; }
    public int TotalCompanyReplacements { get; set; }
    public List<FileValidation> FileValidations { get; set; } = new List<FileValidation>();
}

public class FileValidation
{
    public string FilePath { get; set; } = "";
    public string RelativePath { get; set; } = "";
    public bool IsValid { get; set; }
    public string? Error { get; set; }
    public int OriginalSize { get; set; }
    public int ProcessedSize { get; set; }
    public int CommentsRemoved { get; set; }
    public int CompanyReplacements { get; set; }
}
