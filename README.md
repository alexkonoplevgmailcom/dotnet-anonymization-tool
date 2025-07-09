# 🛡️ Anonymization Tool

A powerful .NET console application that removes comments and replaces company names in source code files to anonymize codebases for sharing, demos, or security purposes.

## ✨ Features

- **Multi-language Support**: Handles C#, Java, XML, SQL, XSD, JSON, HTML files
- **Comment Removal**: Removes single-line, multi-line, and documentation comments
- **Company Name Replacement**: Replaces specified company names with "MyCompany"
- **Automatic Backup**: Creates timestamped backups before processing
- **Interactive Menu**: User-friendly menu system for guided operation
- **Validation Reports**: Generates detailed HTML reports with validation results
- **Restore Functionality**: Easy restoration from backup folders
- **Cross-platform**: Runs on Windows, macOS, and Linux
- **Clean Architecture**: Modular, testable design following SOLID principles

## 🏗️ Architecture

The tool has been completely refactored to follow clean code principles with a modular, maintainable architecture:

### Core Components

```
src/Anonimization/
├── Program.cs                          # Minimal entry point
├── Application/
│   └── ApplicationOrchestrator.cs      # CLI argument handling & flow control
├── Configuration/
│   └── ServiceContainer.cs             # Dependency injection container
├── Models/
│   ├── AnonymizationRequest.cs         # Request data transfer object
│   ├── AnonymizationResult.cs          # Result data transfer object
│   └── ValidationModels.cs             # Validation-related models
├── Core/
│   ├── Interfaces/
│   │   └── IServices.cs                # Core service contracts
│   ├── FileProcessors/
│   │   └── FileProcessors.cs           # File-type specific processors
│   └── Services/
│       ├── CompanyNameReplacer.cs      # Company name replacement logic
│       ├── BackupService.cs            # Backup and restore operations
│       ├── FileAnonymizationService.cs # Main orchestration service
│       └── ValidationService.cs       # Validation and reporting
└── UI/
    └── ConsoleUserInterface.cs         # User interaction handling
```

### Design Principles Applied

- **Single Responsibility**: Each class has one clear, focused purpose
- **Open/Closed**: Extensible through interfaces (e.g., adding new file processors)
- **Dependency Inversion**: Services depend on abstractions, not concrete implementations
- **Interface Segregation**: Role-specific, focused interfaces
- **Clean Separation**: Clear boundaries between UI, application logic, and core services

### Key Interfaces

- `IFileProcessor`: Extensible file processing for different languages/formats
- `ICompanyNameReplacer`: Company name replacement abstraction
- `IBackupService`: Backup and restore operations contract
- `IValidationService`: Validation and reporting functionality
- `IUserInterface`: User interaction abstraction for testability

## 🚀 Quick Start

### Prerequisites

- .NET 9.0 or later
- Any operating system (Windows, macOS, Linux)

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd Anonymization
```

2. Build the project:
```bash
cd src/Anonimization
dotnet build
```

3. Run the application:
```bash
dotnet run
```

## 📖 Usage

### Interactive Mode (Recommended)

Simply run the application without arguments to launch the interactive menu:

```bash
dotnet run
```

The interactive menu provides:
- 🔒 **Anonymize files** - Remove comments and replace company names
- 📂 **Restore files** - Restore from backup folders
- ❓ **Show usage** - Display command line help
- 🚪 **Exit** - Close the application

### Command Line Mode

#### Anonymize Files
```bash
# Anonymize files in a folder (with automatic backup)
dotnet run /path/to/project

# Anonymize files and replace company name
dotnet run /path/to/project "ACME"
```

#### Restore from Backup
```bash
# Restore with confirmation prompts
dotnet run restore /path/to/backup_folder

# Force restore without confirmation
dotnet run restore /path/to/backup_folder --force
```

## 📁 Supported File Types

| File Type | Extensions | Comment Types Removed |
|-----------|------------|----------------------|
| **C#** | `.cs` | `//`, `/* */`, `///` |
| **Java** | `.java` | `//`, `/* */`, `/** */` |
| **XML/XSD** | `.xml`, `.xsd` | `<!-- -->` |
| **SQL** | `.sql` | `--`, `/* */` |
| **HTML** | `.htm`, `.html` | `<!-- -->` |
| **JSON** | `.json` | No comments (formatting only) |

## 🔄 How It Works

1. **Scan**: Recursively scans the target folder for supported file types
2. **Backup**: Creates a timestamped backup folder with original files
3. **Process**: Removes comments and replaces company names
4. **Validate**: Optionally generates a detailed validation report
5. **Report**: Creates an HTML report with clickable file links for VS Code

## 📊 Example Output

```
Found 45 files to process in '/Users/alexk/MyProject'
Backup created at: /Users/alexk/MyProject/backup_20250709_143022

Processing: /Users/alexk/MyProject/src/Program.cs
  ✓ Completed: Program.cs
Processing: /Users/alexk/MyProject/src/Models/User.java
  ✓ Completed: User.java
...

Processing completed.
✅ Anonymization completed!

Would you like to perform anonymization validation and generate a report? (y/N): y

Generating validation report...
✅ Validation report generated: /Users/alexk/MyProject/anonymization_report_20250709_143045.html
📖 Report opened in your default browser
```

## 📋 Company Name Replacement

The tool performs comprehensive company name replacement using multiple patterns:

- **Word boundaries**: `CompanyName` → `MyCompany`
- **Package names**: `com.companyname.app` → `com.MyCompany.app`
- **File paths**: `/companyname/src/` → `/MyCompany/src/`
- **Underscores**: `companyname_database` → `MyCompany_database`
- **Hyphens**: `companyname-service` → `MyCompany-service`
- **Environment variables**: `${COMPANYNAME_API_KEY}` → `${MyCompany_API_KEY}`

## 🔧 Advanced Features

### Validation Reports

After anonymization, the tool can generate detailed HTML validation reports that include:

- 📊 Summary statistics (files processed, comments removed, replacements made)
- 📁 File-by-file validation results
- 🔗 Clickable links to open files directly in VS Code
- 💾 Backup integrity verification

### Backup and Restore

- **Automatic Backups**: Every anonymization creates a timestamped backup
- **Selective Restore**: Restore specific backup folders
- **Conflict Detection**: Warns about file overwrites during restore
- **Structure Preservation**: Maintains original folder structure

## 🛠️ Development

### Project Structure

```
Anonymization/
├── src/
│   └── Anonimization/
│       ├── Anonimization.csproj
│       └── Program.cs
├── .gitignore
├── LICENSE
└── README.md
```

### Building from Source

```bash
# Clone and build
git clone <repository-url>
cd Anonymization/src/Anonimization
dotnet build

# Run tests (if available)
dotnet test

# Create release build
dotnet build -c Release
```

### Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## ⚠️ Important Notes

- **Always review** the generated backup before sharing anonymized code
- **Test thoroughly** on a copy of your codebase first
- **Backup verification** is recommended before deleting original files
- **Company name replacement** is case-insensitive but comprehensive
- **Large codebases** may take some time to process

## 🐛 Troubleshooting

### Common Issues

**Permission Errors**
```bash
# Ensure you have write permissions to the target folder
chmod -R 755 /path/to/project
```

**Large File Processing**
- The tool processes files sequentially for safety
- Very large files may take time to process
- Monitor disk space for backup creation

**Character Encoding**
- The tool assumes UTF-8 encoding
- Files with special encoding may need conversion

## 🛠️ Development

### Architecture Benefits

The refactored clean architecture provides several advantages:

- **Testability**: Each component can be unit tested in isolation using mocks
- **Maintainability**: Clear separation of concerns makes code easier to understand and modify
- **Extensibility**: Adding new file processors or services is straightforward
- **Single Responsibility**: Each class has one clear purpose, reducing complexity
- **Dependency Injection**: All dependencies are injected, making the code more flexible

### Building and Running

```bash
# Build the project
cd src/Anonimization
dotnet build

# Run in development
dotnet run -- [arguments]

# Run tests (when available)
dotnet test
```

### Adding New File Processors

The modular design makes it easy to add support for new file types:

1. Create a new processor implementing `IFileProcessor`:
```csharp
public class PythonFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".py" };
    
    protected override string RemoveComments(string content)
    {
        // Remove Python comments (# ...)
        return Regex.Replace(content, @"#.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
    }
}
```

2. Register it in `FileAnonymizationService.CreateFileProcessors()`:
```csharp
new PythonFileProcessor(),
```

### Testing Strategy

The clean architecture enables comprehensive testing:

- **Unit Tests**: Test individual services in isolation using mocks
- **Integration Tests**: Test service interactions and file operations
- **End-to-End Tests**: Test complete workflows from CLI to file output

Example unit test structure:
```csharp
public class FileAnonymizationServiceTests
{
    private readonly Mock<ICompanyNameReplacer> _mockReplacer;
    private readonly Mock<IBackupService> _mockBackupService;
    private readonly FileAnonymizationService _service;

    // Test methods...
}
```

### Contributing Guidelines

1. **Follow SOLID Principles**: Ensure new code adheres to clean architecture patterns
2. **Add Comprehensive Tests**: Include unit tests for new functionality
3. **Maintain Interface Contracts**: Don't break existing interfaces
4. **Document Changes**: Update README and XML documentation
5. **Handle Errors Gracefully**: Provide meaningful error messages

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Support

- 📚 Check the [documentation](README.md)
- 🐛 Report bugs via [GitHub Issues](../../issues)
- 💡 Request features via [GitHub Issues](../../issues)
- 📧 Contact the maintainers

## 🔗 Related Tools

- [git-secrets](https://github.com/awslabs/git-secrets) - Prevents secrets in git repos
- [BFG Repo-Cleaner](https://rtyley.github.io/bfg-repo-cleaner/) - Removes large files from git history
- [gitignore.io](https://gitignore.io/) - Generate .gitignore files

---

**Made with ❤️ for developers who need to share code safely**
