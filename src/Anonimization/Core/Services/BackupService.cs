using Anonimization.Core.Interfaces;

namespace Anonimization.Core.Services;

/// <summary>
/// Service for creating and managing backups
/// </summary>
public class BackupService : IBackupService
{
    public string CreateBackup(string folderPath, IEnumerable<string> filesToBackup)
    {
        try
        {
            var fileList = filesToBackup.ToList();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFolderName = $"backup_{timestamp}";
            var backupPath = Path.Combine(folderPath, backupFolderName);

            Directory.CreateDirectory(backupPath);

            Console.WriteLine($"Creating backup of {fileList.Count} files...");

            foreach (var sourceFile in fileList)
            {
                CreateFileBackup(sourceFile, folderPath, backupPath);
            }

            return backupPath;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create backup: {ex.Message}", ex);
        }
    }

    private static void CreateFileBackup(string sourceFile, string folderPath, string backupPath)
    {
        try
        {
            var relativePath = Path.GetRelativePath(folderPath, sourceFile);
            var backupFile = Path.Combine(backupPath, relativePath);

            var backupDir = Path.GetDirectoryName(backupFile);
            if (!string.IsNullOrEmpty(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            File.Copy(sourceFile, backupFile, true);
            Console.WriteLine($"  ✓ Backed up: {relativePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Failed to backup {Path.GetFileName(sourceFile)}: {ex.Message}");
            throw;
        }
    }

    public void RestoreFromBackup(string backupPath, bool forceOverwrite = false)
    {
        try
        {
            var targetFolder = Path.GetDirectoryName(backupPath);
            if (string.IsNullOrEmpty(targetFolder))
            {
                Console.WriteLine("Error: Could not determine target folder from backup path.");
                return;
            }

            var backupFiles = Directory.GetFiles(backupPath, "*", SearchOption.AllDirectories).ToList();

            if (!backupFiles.Any())
            {
                Console.WriteLine("Warning: No files found in backup folder.");
                return;
            }

            Console.WriteLine($"Found {backupFiles.Count} files to restore from backup '{backupPath}'");
            Console.WriteLine($"Target folder: '{targetFolder}'");

            if (!forceOverwrite)
            {
                HandleRestoreConflicts(backupFiles, backupPath, targetFolder);
            }

            RestoreFiles(backupFiles, backupPath, targetFolder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during restore operation: {ex.Message}");
        }
    }

    private static void HandleRestoreConflicts(List<string> backupFiles, string backupPath, string targetFolder)
    {
        var conflicts = GetConflictingFiles(backupFiles, backupPath, targetFolder);
        if (conflicts.Any())
        {
            Console.WriteLine($"\nWarning: {conflicts.Count} files will be overwritten:");
            foreach (var conflict in conflicts.Take(10))
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

    private static List<string> GetConflictingFiles(List<string> backupFiles, string backupPath, string targetFolder)
    {
        var conflicts = new List<string>();

        foreach (var backupFile in backupFiles)
        {
            var relativePath = Path.GetRelativePath(backupPath, backupFile);
            var targetFile = Path.Combine(targetFolder, relativePath);

            if (File.Exists(targetFile))
            {
                conflicts.Add(relativePath);
            }
        }

        return conflicts;
    }

    private static void RestoreFiles(List<string> backupFiles, string backupPath, string targetFolder)
    {
        var restoredCount = 0;
        foreach (var backupFile in backupFiles)
        {
            try
            {
                var relativePath = Path.GetRelativePath(backupPath, backupFile);
                var targetFile = Path.Combine(targetFolder, relativePath);

                var targetDir = Path.GetDirectoryName(targetFile);
                if (!string.IsNullOrEmpty(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

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
}
