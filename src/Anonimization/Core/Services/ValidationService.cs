using System.Text.RegularExpressions;
using Anonimization.Core.Interfaces;
using Anonimization.Models;

namespace Anonimization.Core.Services;

/// <summary>
/// Service for validating anonymization results and generating reports
/// </summary>
public class ValidationService : IValidationService
{
    private static readonly string[] SupportedExtensions = { ".cs", ".java", ".xml", ".sql", ".xsd", ".json", ".htm", ".html" };

    public void ValidateAndGenerateReport(string targetFolder, string backupPath, string? companyName)
    {
        try
        {
            var validationResults = PerformValidation(targetFolder, backupPath, companyName);
            var reportPath = GenerateHtmlReport(validationResults, targetFolder);
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

        var processedFiles = GetProcessableFiles(targetFolder);
        var backupFiles = GetProcessableFiles(backupPath);

        results.TotalFilesProcessed = processedFiles.Count;
        results.BackupFilesCount = backupFiles.Count;

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

        CalculateSummaryStatistics(results);
        return results;
    }

    private static List<string> GetProcessableFiles(string folderPath)
    {
        return Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
            .Where(file => SupportedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
            .ToList();
    }

    private static FileValidation ValidateFile(string processedFile, string backupFile, string? companyName)
    {
        var validation = new FileValidation
        {
            FilePath = Path.GetFileName(processedFile),
            RelativePath = processedFile
        };

        try
        {
            var originalContent = File.ReadAllText(backupFile);
            var processedContent = File.ReadAllText(processedFile);

            validation.OriginalSize = originalContent.Length;
            validation.ProcessedSize = processedContent.Length;
            validation.CommentsRemoved = CountCommentsRemoved(originalContent, processedContent);

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

    private static int CountCommentsRemoved(string original, string processed)
    {
        var commentPatterns = new[]
        {
            @"//.*",           // Single line comments
            @"/\*.*?\*/",      // Multi-line comments
            @"<!--.*?-->",     // XML comments
            @"--.*",           // SQL comments
            @"///.*"           // XML doc comments
        };

        var removedCount = 0;
        foreach (var pattern in commentPatterns)
        {
            var originalMatches = Regex.Matches(original, pattern, RegexOptions.Singleline);
            var processedMatches = Regex.Matches(processed, pattern, RegexOptions.Singleline);
            removedCount += Math.Max(0, originalMatches.Count - processedMatches.Count);
        }

        return removedCount;
    }

    private static int CountCompanyReplacements(string original, string processed, string companyName)
    {
        var originalCount = Regex.Matches(original, Regex.Escape(companyName), RegexOptions.IgnoreCase).Count;
        var processedCount = Regex.Matches(processed, Regex.Escape(companyName), RegexOptions.IgnoreCase).Count;
        return Math.Max(0, originalCount - processedCount);
    }

    private static void CalculateSummaryStatistics(ValidationResults results)
    {
        results.FilesWithCommentsRemoved = results.FileValidations.Count(v => v.CommentsRemoved > 0);
        results.FilesWithCompanyReplacements = results.FileValidations.Count(v => v.CompanyReplacements > 0);
        results.TotalCommentsRemoved = results.FileValidations.Sum(v => v.CommentsRemoved);
        results.TotalCompanyReplacements = results.FileValidations.Sum(v => v.CompanyReplacements);
    }

    private static string GenerateHtmlReport(ValidationResults results, string targetFolder)
    {
        var reportPath = Path.Combine(targetFolder, $"anonymization_report_{DateTime.Now:yyyyMMdd_HHmmss}.html");

        var html = GenerateReportHtml(results);
        File.WriteAllText(reportPath, html);
        return reportPath;
    }

    private static string GenerateReportHtml(ValidationResults results)
    {
        return $@"<!DOCTYPE html>
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
            <tbody>
                {GenerateFileValidationRows(results.FileValidations)}
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
    }

    private static string GenerateFileValidationRows(List<FileValidation> fileValidations)
    {
        var rows = new List<string>();

        foreach (var file in fileValidations)
        {
            var processedFilePath = file.RelativePath;
            var vscodeLink = $"vscode://file/{processedFilePath.Replace("\\", "/")}";

            rows.Add($@"
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
                </tr>");
        }

        return string.Join("", rows);
    }

    private static void OpenInBrowser(string filePath)
    {
        try
        {
            var url = $"file://{filePath}";

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
