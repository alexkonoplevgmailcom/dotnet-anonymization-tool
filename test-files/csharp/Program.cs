// This is a single-line comment in C#
using System;
using System.Collections.Generic;
using ACME.Models; // Company namespace reference

namespace ACME.TestApplication
{
    /// <summary>
    /// Main program class for ACME Corporation
    /// This class demonstrates comment removal
    /// </summary>
    public class Program
    {
        /* 
         * Multi-line comment block
         * Contains company information: ACME Corp
         * Should be completely removed
         */
        
        /// <summary>
        /// Main entry point for the ACME application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            // Initialize ACME services
            var service = new ACME.Services.DataService();
            
            Console.WriteLine("Welcome to ACME Corporation!");
            Console.WriteLine($"Company: {GetCompanyName()}"); // ACME reference
            
            /* Process user data
               - Validate ACME credentials
               - Load ACME configuration
               - Start ACME processes */
            ProcessUserData();
        }

        /// <summary>
        /// Returns the company name
        /// </summary>
        /// <returns>Company name string</returns>
        private static string GetCompanyName()
        {
            // TODO: Load from ACME configuration file
            return "ACME Corporation";
        }

        // Process user data for ACME systems
        private static void ProcessUserData()
        {
            /*
             * This method handles:
             * 1. ACME user authentication
             * 2. ACME data validation  
             * 3. ACME business logic
             */
            
            var config = LoadAcmeConfiguration(); // Company-specific config
            // Additional processing...
        }

        /// <summary>
        /// Loads ACME-specific configuration
        /// </summary>
        private static object LoadAcmeConfiguration()
        {
            // Load configuration from ACME_CONFIG environment variable
            var configPath = Environment.GetEnvironmentVariable("ACME_CONFIG_PATH");
            return new { CompanyName = "ACME", Version = "1.0" };
        }
    }
}
