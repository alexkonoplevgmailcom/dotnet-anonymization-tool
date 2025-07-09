using Anonimization.Core.Interfaces;
using Anonimization.Core.Services;
using Anonimization.Models;

namespace Anonimization.UI;

/// <summary>
/// Console-based user interface for the anonymization tool
/// </summary>
public class ConsoleUserInterface : IUserInterface
{
    private readonly FileAnonymizationService _anonymizationService;
    private readonly IBackupService _backupService;
    private readonly IValidationService _validationService;

    public ConsoleUserInterface(
        FileAnonymizationService anonymizationService,
        IBackupService backupService,
        IValidationService validationService)
    {
        _anonymizationService = anonymizationService;
        _backupService = backupService;
        _validationService = validationService;
    }

    public void ShowInteractiveMenu()
    {
        if (!IsInteractiveEnvironment())
        {
            Console.WriteLine("Error: Interactive menu requires a console environment.");
            Console.WriteLine("Use command line arguments instead. Run with any argument to see usage.");
            ShowUsage();
            return;
        }

        while (true)
        {
            DisplayMainMenu();
            var input = Console.ReadLine();

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

    public bool IsInteractiveEnvironment()
    {
        try
        {
            return !Console.IsInputRedirected && !Console.IsOutputRedirected;
        }
        catch
        {
            return false;
        }
    }

    public void ShowUsage()
    {
        Console.WriteLine("Anonymization Tool - Command Line Usage:");
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

    private static void DisplayMainMenu()
    {
        Console.Clear();
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
    }

    private void HandleInteractiveAnonymize()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    ANONYMIZE FILES                      ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        var folderPath = GetValidFolderPath();
        if (string.IsNullOrEmpty(folderPath)) return;

        var companyName = GetCompanyName();

        if (!ConfirmOperation(folderPath, companyName)) return;

        ExecuteAnonymization(folderPath, companyName);
    }

    private void HandleInteractiveRestore()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  RESTORE FROM BACKUP                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        var backupPath = GetValidBackupPath();
        if (string.IsNullOrEmpty(backupPath)) return;

        var forceOverwrite = GetForceOverwriteOption();

        if (!ConfirmRestore(backupPath, forceOverwrite)) return;

        ExecuteRestore(backupPath, forceOverwrite);
    }

    private static string GetValidFolderPath()
    {
        while (true)
        {
            Console.Write("Enter the folder path to anonymize: ");
            var path = Console.ReadLine()?.Trim();

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

    private static string? GetCompanyName()
    {
        Console.WriteLine();
        Console.WriteLine("Company Name Replacement (Optional):");
        Console.WriteLine("If specified, all occurrences of this company name will be replaced with 'MyCompany'");
        Console.Write("Enter company name to replace (or press Enter to skip): ");
        var companyName = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(companyName))
        {
            Console.WriteLine("→ No company name replacement will be performed.");
            return null;
        }

        Console.WriteLine($"→ Will replace '{companyName}' with 'MyCompany'");
        return companyName;
    }

    private static bool ConfirmOperation(string folderPath, string? companyName)
    {
        Console.WriteLine();
        Console.WriteLine("OPERATION SUMMARY:");
        Console.WriteLine($"📁 Target folder: {folderPath}");
        Console.WriteLine($"🏢 Company replacement: {(companyName != null ? $"{companyName} → MyCompany" : "None")}");
        Console.WriteLine($"💾 Backup: Will be created automatically");
        Console.WriteLine();
        Console.Write("Proceed with anonymization? (y/N): ");

        var confirm = Console.ReadLine();
        return string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase);
    }

    private void ExecuteAnonymization(string folderPath, string? companyName)
    {
        Console.WriteLine();
        Console.WriteLine("Starting anonymization...");
        Console.WriteLine(new string('─', 60));

        var request = new AnonymizationRequest(folderPath, companyName);
        var result = _anonymizationService.ProcessFolder(request);

        Console.WriteLine(new string('─', 60));

        if (result.IsSuccess)
        {
            Console.WriteLine("✅ Anonymization completed!");
            OfferValidationReport(folderPath, result.BackupPath, companyName);
        }
        else
        {
            Console.WriteLine($"❌ Anonymization failed: {result.ErrorMessage}");
        }

        PauseForUser();
    }

    private void OfferValidationReport(string folderPath, string backupPath, string? companyName)
    {
        Console.WriteLine();
        Console.Write("Would you like to perform anonymization validation and generate a report? (y/N): ");
        var validateResponse = Console.ReadLine();

        if (string.Equals(validateResponse, "y", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(validateResponse, "yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine();
            Console.WriteLine("Generating validation report...");
            _validationService.ValidateAndGenerateReport(folderPath, backupPath, companyName);
        }
    }

    private static string GetValidBackupPath()
    {
        while (true)
        {
            Console.Write("Enter the backup folder path: ");
            var path = Console.ReadLine()?.Trim();

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

            var folderName = Path.GetFileName(path);
            if (!folderName.StartsWith("backup_", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"⚠️  Warning: '{folderName}' doesn't look like a backup folder (should start with 'backup_')");
                Console.Write("Continue anyway? (y/N): ");
                var confirm = Console.ReadLine();
                if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
            }

            return path;
        }
    }

    private static bool GetForceOverwriteOption()
    {
        Console.WriteLine();
        Console.WriteLine("Overwrite Options:");
        Console.WriteLine("1. Ask for confirmation if files will be overwritten (recommended)");
        Console.WriteLine("2. Force overwrite without confirmation");
        Console.Write("Select option (1-2): ");

        var overwriteOption = Console.ReadLine()?.Trim();
        return overwriteOption == "2";
    }

    private static bool ConfirmRestore(string backupPath, bool forceOverwrite)
    {
        Console.WriteLine();
        Console.WriteLine("RESTORE SUMMARY:");
        Console.WriteLine($"📁 Backup folder: {backupPath}");
        Console.WriteLine($"⚡ Force overwrite: {(forceOverwrite ? "Yes" : "No (will ask for confirmation)")}");
        Console.WriteLine();
        Console.Write("Proceed with restore? (y/N): ");

        var confirm = Console.ReadLine();
        return string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(confirm, "yes", StringComparison.OrdinalIgnoreCase);
    }

    private void ExecuteRestore(string backupPath, bool forceOverwrite)
    {
        Console.WriteLine();
        Console.WriteLine("Starting restore...");
        Console.WriteLine(new string('─', 60));

        _backupService.RestoreFromBackup(backupPath, forceOverwrite);

        Console.WriteLine(new string('─', 60));
        Console.WriteLine("✅ Restore completed!");
        PauseForUser();
    }

    private static void PauseForUser()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
    }
}
