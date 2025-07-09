namespace Anonimization.Core.Interfaces;

/// <summary>
/// Interface for processing different file types
/// </summary>
public interface IFileProcessor
{
    /// <summary>
    /// Supported file extensions (including the dot)
    /// </summary>
    IEnumerable<string> SupportedExtensions { get; }

    /// <summary>
    /// Process the content of a file to remove comments
    /// </summary>
    /// <param name="content">Original file content</param>
    /// <returns>Processed content with comments removed</returns>
    string ProcessContent(string content);
}

/// <summary>
/// Interface for company name replacement
/// </summary>
public interface ICompanyNameReplacer
{
    /// <summary>
    /// Replace company name occurrences with "MyCompany"
    /// </summary>
    /// <param name="content">Content to process</param>
    /// <param name="companyName">Company name to replace</param>
    /// <returns>Content with company name replaced</returns>
    string ReplaceCompanyName(string content, string companyName);
}

/// <summary>
/// Interface for backup operations
/// </summary>
public interface IBackupService
{
    /// <summary>
    /// Create a backup of specified files
    /// </summary>
    /// <param name="folderPath">Source folder path</param>
    /// <param name="filesToBackup">List of files to backup</param>
    /// <returns>Path to the created backup folder</returns>
    string CreateBackup(string folderPath, IEnumerable<string> filesToBackup);

    /// <summary>
    /// Restore files from a backup
    /// </summary>
    /// <param name="backupPath">Path to backup folder</param>
    /// <param name="forceOverwrite">Whether to overwrite without confirmation</param>
    void RestoreFromBackup(string backupPath, bool forceOverwrite = false);
}

/// <summary>
/// Interface for validation services
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// Validate anonymization results and generate a report
    /// </summary>
    /// <param name="targetFolder">Target folder that was processed</param>
    /// <param name="backupPath">Path to backup folder</param>
    /// <param name="companyName">Company name that was replaced</param>
    void ValidateAndGenerateReport(string targetFolder, string backupPath, string? companyName);
}

/// <summary>
/// Interface for user interaction
/// </summary>
public interface IUserInterface
{
    /// <summary>
    /// Show the interactive menu
    /// </summary>
    void ShowInteractiveMenu();

    /// <summary>
    /// Show command line usage information
    /// </summary>
    void ShowUsage();

    /// <summary>
    /// Check if running in an interactive environment
    /// </summary>
    bool IsInteractiveEnvironment();
}
