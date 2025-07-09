using Anonimization.Application;
using Anonimization.Core.Interfaces;
using Anonimization.Core.Services;
using Anonimization.UI;

namespace Anonimization.Configuration;

/// <summary>
/// Simple dependency injection container for the application
/// </summary>
public static class ServiceContainer
{
    /// <summary>
    /// Configure and create the application orchestrator with all dependencies
    /// </summary>
    public static ApplicationOrchestrator CreateApplication()
    {
        // Core services
        var companyNameReplacer = new CompanyNameReplacer();
        var backupService = new BackupService();
        var validationService = new ValidationService();

        // Main anonymization service
        var anonymizationService = new FileAnonymizationService(companyNameReplacer, backupService);

        // User interface
        var userInterface = new ConsoleUserInterface(anonymizationService, backupService, validationService);

        // Application orchestrator
        return new ApplicationOrchestrator(userInterface, anonymizationService, backupService, validationService);
    }
}
