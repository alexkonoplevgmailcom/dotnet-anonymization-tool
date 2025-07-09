// User model for ACME Corporation
using System;

namespace ACME.Models
{
    /// <summary>
    /// Represents a user in the ACME system
    /// Contains personal and company information
    /// </summary>
    public class User
    {
        // Primary key for ACME database
        public int Id { get; set; }
        
        /// <summary>
        /// User's full name in ACME format
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        
        // Email address (must be @acme.com domain)
        public string Email { get; set; } = string.Empty;
        
        /*
         * Department within ACME Corporation
         * Possible values: IT, HR, Finance, Marketing
         * Used for ACME access control
         */
        public string Department { get; set; } = string.Empty;
        
        /// <summary>
        /// Date when user joined ACME
        /// </summary>
        public DateTime JoinDate { get; set; }
        
        // Internal ACME employee ID
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>
        /// Validates if user belongs to ACME domain
        /// </summary>
        /// <returns>True if valid ACME user</returns>
        public bool IsValidAcmeUser()
        {
            // Check if email ends with @acme.com
            return Email.EndsWith("@acme.com", StringComparison.OrdinalIgnoreCase);
        }

        /*
         * Gets the user's full ACME identifier
         * Format: ACME_{Department}_{EmployeeId}
         */
        public string GetAcmeIdentifier()
        {
            return $"ACME_{Department}_{EmployeeId}";
        }
    }
}
