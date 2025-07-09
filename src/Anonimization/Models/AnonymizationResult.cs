namespace Anonimization.Models;

/// <summary>
/// Represents the result of an anonymization operation
/// </summary>
public class AnonymizationResult
{
    public bool IsSuccess { get; }
    public string BackupPath { get; }
    public int FilesProcessed { get; }
    public string? ErrorMessage { get; }

    private AnonymizationResult(bool isSuccess, string backupPath, int filesProcessed, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        BackupPath = backupPath;
        FilesProcessed = filesProcessed;
        ErrorMessage = errorMessage;
    }

    public static AnonymizationResult Success(string backupPath, int filesProcessed)
        => new(true, backupPath, filesProcessed);

    public static AnonymizationResult Failure(string errorMessage)
        => new(false, string.Empty, 0, errorMessage);
}
