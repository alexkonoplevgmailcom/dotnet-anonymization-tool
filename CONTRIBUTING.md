# Contributing to Anonymization Tool

Thank you for your interest in contributing to the Anonymization Tool! This document provides guidelines for contributing to the project.

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Git
- Your favorite code editor (VS Code recommended)

### Setting Up Development Environment

1. Fork the repository on GitHub
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR_USERNAME/anonymization-tool.git
   cd anonymization-tool
   ```
3. Build the project:
   ```bash
   cd src/Anonimization
   dotnet build
   ```
4. Run tests (when available):
   ```bash
   dotnet test
   ```

## 📝 How to Contribute

### Reporting Bugs

When reporting bugs, please include:

- **Description**: Clear description of the issue
- **Steps to Reproduce**: Detailed steps to reproduce the bug
- **Expected Behavior**: What you expected to happen
- **Actual Behavior**: What actually happened
- **Environment**: OS, .NET version, file types involved
- **Sample Files**: If possible, provide sample files that trigger the issue

### Suggesting Features

For feature requests, please:

- Check if the feature already exists or is planned
- Provide a clear use case for the feature
- Describe the expected behavior
- Consider backwards compatibility

### Code Contributions

1. **Create a Branch**: Create a feature branch for your changes
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Changes**: Implement your changes following the coding standards

3. **Test Your Changes**: Ensure your changes don't break existing functionality

4. **Commit**: Use descriptive commit messages
   ```bash
   git commit -m "Add support for TypeScript files (.ts extension)"
   ```

5. **Push**: Push to your fork
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Pull Request**: Create a pull request with:
   - Clear title and description
   - Reference to any related issues
   - Screenshots/examples if applicable

## 🎯 Development Guidelines

### Code Style

- Use C# naming conventions (PascalCase for public members, camelCase for private)
- Add XML documentation comments for public methods
- Keep methods focused and small
- Use meaningful variable and method names
- Follow existing code patterns in the project

### Adding Support for New File Types

To add support for a new file type:

1. Add the extension to the `_fileProcessors` dictionary in `FileAnonymizer`
2. Implement a corresponding `Process{Language}File` method
3. Update the supported file types table in README.md
4. Add test cases for the new file type

Example:
```csharp
private string ProcessTypeScriptFile(string content)
{
    // Remove single-line comments (// ...)
    content = Regex.Replace(content, @"//.*?(?=\r?\n|$)", "", RegexOptions.Multiline);
    
    // Remove multi-line comments (/* ... */)
    content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);
    
    return CleanupWhitespace(content);
}
```

### Testing

- Test with various file types and sizes
- Test edge cases (empty files, files with only comments, etc.)
- Test company name replacement with different patterns
- Verify backup and restore functionality

### Performance Considerations

- Be mindful of memory usage with large files
- Consider file processing time for large codebases
- Optimize regex patterns for performance
- Test with directories containing many files

## 🔍 Code Review Process

All contributions go through code review:

1. **Automated Checks**: CI/CD pipeline runs automated checks
2. **Manual Review**: Maintainers review code for:
   - Correctness and functionality
   - Code quality and style
   - Performance implications
   - Security considerations
   - Documentation completeness

3. **Feedback**: Address any feedback from reviewers
4. **Approval**: Once approved, changes will be merged

## 📚 Documentation

When contributing:

- Update README.md if adding features or changing usage
- Add XML documentation comments for new public methods
- Update inline comments for complex logic
- Consider adding examples for new features

## 🐛 Debugging

### Common Development Issues

**Build Errors**
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

**Runtime Issues**
- Use the VS Code debugger with provided launch configurations
- Add breakpoints in VS Code
- Use `Console.WriteLine` for quick debugging

**File Processing Issues**
- Test with small sample files first
- Check file encoding (tool assumes UTF-8)
- Verify file permissions

## 📋 Pull Request Checklist

Before submitting a pull request:

- [ ] Code builds without warnings
- [ ] All existing functionality still works
- [ ] New features are tested
- [ ] Documentation is updated
- [ ] Commit messages are descriptive
- [ ] Code follows project style guidelines
- [ ] No sensitive information (passwords, keys) in code

## 🆘 Getting Help

If you need help:

- Check existing [GitHub Issues](../../issues)
- Create a new issue with the "question" label
- Review the main [README.md](README.md) documentation

## 📄 License

By contributing to this project, you agree that your contributions will be licensed under the MIT License.

## 🙏 Recognition

Contributors will be recognized in:

- GitHub contributors list
- Release notes for significant contributions
- Special mentions for major features

Thank you for helping make the Anonymization Tool better! 🎉
