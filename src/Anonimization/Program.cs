using Anonimization.Configuration;

/// <summary>
/// Entry point for the Anonymization Tool
/// </summary>
class Program
{
    /// <summary>
    /// Main entry point of the application
    /// </summary>
    /// <param name="args">Command line arguments</param>
    static void Main(string[] args)
    {
        try
        {
            var application = ServiceContainer.CreateApplication();
            application.Run(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
