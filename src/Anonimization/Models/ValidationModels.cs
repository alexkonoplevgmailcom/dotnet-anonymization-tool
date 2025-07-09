namespace Anonimization.Models;

/// <summary>
/// Validation results for a single file
/// </summary>
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

/// <summary>
/// Overall validation results for the anonymization process
/// </summary>
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
    public List<FileValidation> FileValidations { get; set; } = new();
}
