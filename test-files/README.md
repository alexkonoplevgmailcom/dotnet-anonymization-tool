# Example Test Files for Anonymization Tool

This folder contains sample files for testing the anonymization tool's functionality.

## 📁 Test Files Structure

```
test-files/
├── csharp/
│   ├── Program.cs
│   └── Models/
│       └── User.cs
├── java/
│   ├── Main.java
│   └── models/
│       └── Customer.java
├── xml/
│   ├── config.xml
│   └── schema.xsd
├── sql/
│   └── database.sql
└── html/
    └── index.html
```

## 🧪 Usage

1. Run the anonymization tool on this folder:
   ```bash
   dotnet run test-files
   ```

2. With company name replacement:
   ```bash
   dotnet run test-files "ACME"
   ```

3. The tool will:
   - Create a backup folder
   - Remove all comments from the files
   - Replace "ACME" with "MyCompany" (if specified)
   - Generate a validation report

## 📝 What Gets Tested

- **C# Files**: Single-line (`//`), multi-line (`/* */`), and XML doc (`///`) comments
- **Java Files**: Single-line (`//`), multi-line (`/* */`), and JavaDoc (`/** */`) comments  
- **XML/XSD Files**: XML comments (`<!-- -->`)
- **SQL Files**: Single-line (`--`) and multi-line (`/* */`) comments
- **HTML Files**: HTML comments (`<!-- -->`)
- **Company Name Replacement**: Various patterns like namespaces, file paths, variables

## 🔍 Expected Results

After anonymization, you should see:
- All comment blocks removed
- Company names replaced with "MyCompany"
- Original formatting preserved
- Backup folder created with timestamp
- Validation report generated (if requested)
