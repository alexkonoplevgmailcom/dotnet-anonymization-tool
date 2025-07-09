using Anonimization.Core.FileProcessors;
using Anonimization.Core.Interfaces;
using Anonimization.Core.Services;
using Anonimization.Models;

namespace Anonimization.Core.Services;

/// <summary>
/// Main service for file anonymization operations
/// </summary>
public class FileAnonymizationService
{
    private readonly ICompanyNameReplacer _companyNameReplacer;
    private readonly IBackupService _backupService;
    private readonly Dictionary<string, IFileProcessor> _fileProcessors;

    public FileAnonymizationService(
        ICompanyNameReplacer companyNameReplacer,
        IBackupService backupService)
    {
        _companyNameReplacer = companyNameReplacer;
        _backupService = backupService;
        _fileProcessors = CreateFileProcessors();
    }

    private static Dictionary<string, IFileProcessor> CreateFileProcessors()
    {
        var processors = new IFileProcessor[]
        {
            new CSharpFileProcessor(),
            new JavaFileProcessor(),
            new XmlFileProcessor(),
            new SqlFileProcessor(),
            new HtmlFileProcessor(),
            new JsonFileProcessor()
        };

        var processorMap = new Dictionary<string, IFileProcessor>(StringComparer.OrdinalIgnoreCase);

        foreach (var processor in processors)
        {
            foreach (var extension in processor.SupportedExtensions)
            {
                processorMap[extension] = processor;
            }
        }

        return processorMap;
    }

    public AnonymizationResult ProcessFolder(AnonymizationRequest request)
    {
        try
        {
            if (!Directory.Exists(request.FolderPath))
            {
                return AnonymizationResult.Failure($"Folder '{request.FolderPath}' does not exist.");
            }

            var files = GetSupportedFiles(request.FolderPath);
            Console.WriteLine($"Found {files.Count} files to process in '{request.FolderPath}'");

            if (!files.Any())
            {
                return AnonymizationResult.Failure("No supported files found to process.");
            }

            var backupPath = _backupService.CreateBackup(request.FolderPath, files);
            Console.WriteLine($"Backup created at: {backupPath}");

            var processedCount = 0;
            foreach (var file in files)
            {
                if (ProcessFile(file, request.CompanyName))
                {
                    processedCount++;
                }
            }

            Console.WriteLine("Processing completed.");
            return AnonymizationResult.Success(backupPath, processedCount);
        }
        catch (Exception ex)
        {
            return AnonymizationResult.Failure($"Error processing folder: {ex.Message}");
        }
    }

    private List<string> GetSupportedFiles(string folderPath)
    {
        return Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
            .Where(file => _fileProcessors.ContainsKey(Path.GetExtension(file)))
            .ToList();
    }

    private bool ProcessFile(string filePath, string? companyName)
    {
        try
        {
            Console.WriteLine($"Processing: {filePath}");

            var content = File.ReadAllText(filePath);
            var extension = Path.GetExtension(filePath);

            if (_fileProcessors.TryGetValue(extension, out var processor))
            {
                var processedContent = processor.ProcessContent(content);

                if (!string.IsNullOrEmpty(companyName))
                {
                    processedContent = _companyNameReplacer.ReplaceCompanyName(processedContent, companyName);
                }

                File.WriteAllText(filePath, processedContent);
                Console.WriteLine($"  ✓ Completed: {Path.GetFileName(filePath)}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Error processing {filePath}: {ex.Message}");
            return false;
        }
    }

    public IEnumerable<string> GetSupportedExtensions()
    {
        return _fileProcessors.Keys;
    }
}
