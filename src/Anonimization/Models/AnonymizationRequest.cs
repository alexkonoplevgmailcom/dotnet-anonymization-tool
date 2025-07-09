namespace Anonimization.Models;

/// <summary>
/// Represents a request to anonymize files
/// </summary>
public class AnonymizationRequest
{
    public string FolderPath { get; }
    public string? CompanyName { get; }

    public AnonymizationRequest(string folderPath, string? companyName = null)
    {
        if (string.IsNullOrWhiteSpace(folderPath))
            throw new ArgumentException("Folder path cannot be null or empty", nameof(folderPath));

        FolderPath = folderPath;
        CompanyName = companyName;
    }
}
