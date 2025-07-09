using Anonimization.Core.Interfaces;
using Anonimization.Core.Services;
using Anonimization.Models;
using Anonimization.UI;

namespace Anonimization.Application;

/// <summary>
/// Orchestrates the application flow and command line argument handling
/// </summary>
public class ApplicationOrchestrator
{
    private readonly ConsoleUserInterface _userInterface;
    private readonly FileAnonymizationService _anonymizationService;
    private readonly IBackupService _backupService;
    private readonly IValidationService _validationService;

    public ApplicationOrchestrator(
        ConsoleUserInterface userInterface,
        FileAnonymizationService anonymizationService,
        IBackupService backupService,
        IValidationService validationService)
    {
        _userInterface = userInterface;
        _anonymizationService = anonymizationService;
        _backupService = backupService;
        _validationService = validationService;
    }

    public void Run(string[] args)
    {
        if (args.Length == 0)
        {
            _userInterface.ShowInteractiveMenu();
            return;
        }

        var command = args[0];

        if (command.Equals("restore", StringComparison.OrdinalIgnoreCase))
        {
            HandleRestoreCommand(args);
        }
        else
        {
            HandleAnonymizeCommand(args);
        }
    }

    private void HandleRestoreCommand(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Error: Backup folder path is required for restore command.");
            _userInterface.ShowUsage();
            return;
        }

        var backupPath = args[1];
        var forceOverwrite = args.Length > 2 && args[2].Equals("--force", StringComparison.OrdinalIgnoreCase);

        if (!Directory.Exists(backupPath))
        {
            Console.WriteLine($"Error: Backup folder '{backupPath}' does not exist.");
            return;
        }

        _backupService.RestoreFromBackup(backupPath, forceOverwrite);
    }

    private void HandleAnonymizeCommand(string[] args)
    {
        var folderPath = args[0];
        var companyName = args.Length > 1 ? args[1] : null;

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: Folder '{folderPath}' does not exist.");
            return;
        }

        var request = new AnonymizationRequest(folderPath, companyName);
        var result = _anonymizationService.ProcessFolder(request);

        if (result.IsSuccess)
        {
            Console.WriteLine("✅ Anonymization completed!");
            OfferValidationReport(folderPath, result.BackupPath, companyName);
        }
        else
        {
            Console.WriteLine($"❌ Anonymization failed: {result.ErrorMessage}");
        }
    }

    private void OfferValidationReport(string folderPath, string backupPath, string? companyName)
    {
        if (string.IsNullOrEmpty(backupPath)) return;

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
}
