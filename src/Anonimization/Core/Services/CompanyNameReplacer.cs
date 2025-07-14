using System.Text.RegularExpressions;
using Anonimization.Core.Interfaces;

namespace Anonimization.Core.Services;

/// <summary>
/// Handles company name replacement with comprehensive pattern matching
/// </summary>
public class CompanyNameReplacer : ICompanyNameReplacer
{
    public string ReplaceCompanyName(string content, string companyName)
    {
        if (string.IsNullOrEmpty(companyName))
            return content;

        const string replacement = "MyCompany";
        var escapedCompanyName = Regex.Escape(companyName);

        // Apply various replacement patterns
        content = ApplyWordBoundaryReplacement(content, escapedCompanyName, replacement);
        content = ApplyCompoundIdentifierReplacements(content, escapedCompanyName, replacement);
        content = ApplyPackageNameReplacements(content, escapedCompanyName, replacement);
        content = ApplyPathReplacements(content, escapedCompanyName, replacement);
        content = ApplyUnderscoreReplacements(content, escapedCompanyName, replacement);
        content = ApplyHyphenReplacements(content, escapedCompanyName, replacement);
        content = ApplyEnvironmentVariableReplacements(content, escapedCompanyName, replacement);
        content = ApplyColonReplacements(content, escapedCompanyName, replacement);

        return content;
    }

    private static string ApplyWordBoundaryReplacement(string content, string escapedCompanyName, string replacement)
        => Regex.Replace(content, $@"\b{escapedCompanyName}\b", replacement, RegexOptions.IgnoreCase);

    private static string ApplyCompoundIdentifierReplacements(string content, string escapedCompanyName, string replacement)
    {
        // Replace company name at the beginning of PascalCase identifiers (e.g., AcmeService -> MyCompanyService)
        content = Regex.Replace(content, $@"\b{escapedCompanyName}([A-Z][a-zA-Z0-9]*)", $"{replacement}$1", RegexOptions.IgnoreCase);

        // Replace company name at the end of PascalCase identifiers (e.g., ServiceAcme -> ServiceMyCompany)
        content = Regex.Replace(content, $@"([A-Z][a-zA-Z0-9]*){escapedCompanyName}\b", $"$1{replacement}", RegexOptions.IgnoreCase);

        // Replace company name in the middle of compound identifiers (e.g., DataAcmeProcessor -> DataMyCompanyProcessor)
        content = Regex.Replace(content, $@"([A-Z][a-zA-Z0-9]*){escapedCompanyName}([A-Z][a-zA-Z0-9]*)", $"$1{replacement}$2", RegexOptions.IgnoreCase);

        // Replace in camelCase identifiers (e.g., acmeService -> myCompanyService)
        content = Regex.Replace(content, $@"\b{escapedCompanyName}([A-Z][a-zA-Z0-9]*)", m =>
        {
            var suffix = m.Groups[1].Value;
            return $"{replacement.Substring(0, 1).ToLower()}{replacement.Substring(1)}{suffix}";
        }, RegexOptions.IgnoreCase);

        return content;
    }

    private static string ApplyPackageNameReplacements(string content, string escapedCompanyName, string replacement)
    {
        content = Regex.Replace(content, $@"\.{escapedCompanyName}\.", $".{replacement}.", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"^{escapedCompanyName}\.", $"{replacement}.", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        content = Regex.Replace(content, $@"\.{escapedCompanyName}$", $".{replacement}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        return content;
    }

    private static string ApplyPathReplacements(string content, string escapedCompanyName, string replacement)
    {
        content = Regex.Replace(content, $@"/{escapedCompanyName}/", $"/{replacement}/", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"^{escapedCompanyName}/", $"{replacement}/", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        content = Regex.Replace(content, $@"/{escapedCompanyName}$", $"/{replacement}", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        return content;
    }

    private static string ApplyUnderscoreReplacements(string content, string escapedCompanyName, string replacement)
    {
        content = Regex.Replace(content, $@"\b{escapedCompanyName}_", $"{replacement}_", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"_{escapedCompanyName}_", $"_{replacement}_", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"_{escapedCompanyName}\b", $"_{replacement}", RegexOptions.IgnoreCase);
        return content;
    }

    private static string ApplyHyphenReplacements(string content, string escapedCompanyName, string replacement)
    {
        content = Regex.Replace(content, $@"\b{escapedCompanyName}-", $"{replacement}-", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"-{escapedCompanyName}-", $"-{replacement}-", RegexOptions.IgnoreCase);
        content = Regex.Replace(content, $@"-{escapedCompanyName}\b", $"-{replacement}", RegexOptions.IgnoreCase);
        return content;
    }

    private static string ApplyEnvironmentVariableReplacements(string content, string escapedCompanyName, string replacement)
        => Regex.Replace(content, $@"\$\{{{escapedCompanyName}_", $"${{{replacement}_", RegexOptions.IgnoreCase);

    private static string ApplyColonReplacements(string content, string escapedCompanyName, string replacement)
        => Regex.Replace(content, $@"\b{escapedCompanyName}:", $"{replacement}:", RegexOptions.IgnoreCase);
}
