using System.Text.RegularExpressions;
using Anonimization.Core.Interfaces;

namespace Anonimization.Core.FileProcessors;

/// <summary>
/// Base class for file processors with common functionality
/// </summary>
public abstract class BaseFileProcessor : IFileProcessor
{
    public abstract IEnumerable<string> SupportedExtensions { get; }

    public virtual string ProcessContent(string content)
    {
        content = RemoveComments(content);
        return CleanupWhitespace(content);
    }

    protected abstract string RemoveComments(string content);

    protected virtual string CleanupWhitespace(string content)
    {
        // Remove empty lines that resulted from comment removal
        content = Regex.Replace(content, @"^\s*\r?\n", "", RegexOptions.Multiline);

        // Remove multiple consecutive empty lines, keeping only one
        content = Regex.Replace(content, @"(\r?\n){3,}", "\n\n");

        return content.Trim();
    }
}

/// <summary>
/// Processor for C# files
/// </summary>
public class CSharpFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".cs" };

    protected override string RemoveComments(string content)
    {
        // Remove single-line comments (// ...)
        content = Regex.Replace(content, @"//.*?(?=\r?\n|$)", "", RegexOptions.Multiline);

        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

        // Remove XML documentation comments (/// ...)
        content = Regex.Replace(content, @"///.*?(?=\r?\n|$)", "", RegexOptions.Multiline);

        return content;
    }
}

/// <summary>
/// Processor for Java files
/// </summary>
public class JavaFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".java" };

    protected override string RemoveComments(string content)
    {
        // Remove single-line comments (// ...)
        content = Regex.Replace(content, @"//.*?(?=\r?\n|$)", "", RegexOptions.Multiline);

        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

        // Remove JavaDoc comments (/** ... */)
        content = Regex.Replace(content, @"/\*\*.*?\*/", "", RegexOptions.Singleline);

        return content;
    }
}

/// <summary>
/// Processor for XML and XSD files
/// </summary>
public class XmlFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".xml", ".xsd" };

    protected override string RemoveComments(string content)
    {
        // Remove XML comments (<!-- ... -->)
        return Regex.Replace(content, @"<!--.*?-->", "", RegexOptions.Singleline);
    }
}

/// <summary>
/// Processor for SQL files
/// </summary>
public class SqlFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".sql" };

    protected override string RemoveComments(string content)
    {
        // Remove single-line comments (-- ...)
        content = Regex.Replace(content, @"--.*?(?=\r?\n|$)", "", RegexOptions.Multiline);

        // Remove multi-line comments (/* ... */)
        content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

        return content;
    }
}

/// <summary>
/// Processor for HTML files
/// </summary>
public class HtmlFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".htm", ".html" };

    protected override string RemoveComments(string content)
    {
        // Remove HTML comments (<!-- ... -->)
        return Regex.Replace(content, @"<!--.*?-->", "", RegexOptions.Singleline);
    }
}

/// <summary>
/// Processor for JSON files (no comments to remove, just formatting)
/// </summary>
public class JsonFileProcessor : BaseFileProcessor
{
    public override IEnumerable<string> SupportedExtensions => new[] { ".json" };

    protected override string RemoveComments(string content)
    {
        // JSON files don't typically have comments to remove in standard JSON
        return content;
    }

    protected override string CleanupWhitespace(string content)
    {
        // For JSON, we don't want to aggressively clean whitespace
        return content;
    }
}
