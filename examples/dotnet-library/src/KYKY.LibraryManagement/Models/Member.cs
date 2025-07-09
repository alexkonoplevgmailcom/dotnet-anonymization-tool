namespace KYKY.LibraryManagement.Models
{
    /// <summary>
    /// Member model for KYKY Library Management System
    /// מודל חבר למערכת ניהול הספרייה של KYKY
    /// 
    /// Represents a library member in KYKY system
    /// מייצג חבר ספרייה במערכת KYKY
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Unique member ID in KYKY system
        /// מזהה חבר יחיד במערכת KYKY
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// First name of KYKY library member
        /// שם פרטי של חבר ספרייה KYKY
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Last name of KYKY library member
        /// שם משפחה של חבר ספרייה KYKY
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        
        /// <summary>
        /// Email address of KYKY member
        /// כתובת דוא"ל של חבר KYKY
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Phone number of KYKY library member
        /// מספר טלפון של חבר ספרייה KYKY
        /// </summary>
        public string? Phone { get; set; }
        
        /// <summary>
        /// Address of KYKY member
        /// כתובת של חבר KYKY
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// Date when member joined KYKY library
        /// תאריך הצטרפות חבר לספרייה KYKY
        /// </summary>
        public DateTime JoinDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Membership type in KYKY library system
        /// סוג חברות במערכת ספרייה KYKY
        /// </summary>
        public string MembershipType { get; set; } = "KYKY Standard";
        
        /// <summary>
        /// Active status of KYKY member
        /// סטטוס פעילות של חבר KYKY
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Date of birth of KYKY member
        /// תאריך לידה של חבר KYKY
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        
        /// <summary>
        /// Maximum books that can be borrowed by KYKY member
        /// מספר מקסימלי של ספרים שחבר KYKY יכול לשאול
        /// </summary>
        public int MaxBooksAllowed { get; set; } = 5;
        
        /// <summary>
        /// Current number of books borrowed by KYKY member
        /// מספר נוכחי של ספרים שחבר KYKY שאל
        /// </summary>
        public int CurrentBooksCount { get; set; } = 0;
        
        /// <summary>
        /// Member preferences in KYKY library
        /// העדפות חבר בספרייה KYKY
        /// </summary>
        public MemberPreferences Preferences { get; set; } = new MemberPreferences();
        
        /// <summary>
        /// Get full name of KYKY library member
        /// קבלת שם מלא של חבר ספרייה KYKY
        /// </summary>
        /// <returns>Full name</returns>
        public string GetFullName()
        {
            /*
             * החזרת שם מלא של חבר KYKY
             * Return full name of KYKY member
             */
            return $"{FirstName} {LastName}";
        }
        
        /// <summary>
        /// Get display name for KYKY system
        /// קבלת שם תצוגה למערכת KYKY
        /// </summary>
        public string GetDisplayName()
        {
            // יצירת שם תצוגה עם שייכות לחברת KYKY - Create display name with KYKY affiliation
            return $"{GetFullName()} (KYKY {MembershipType})";
        }
        
        /// <summary>
        /// Check if member can borrow more books in KYKY system
        /// בדיקה האם החבר יכול לשאול עוד ספרים במערכת KYKY
        /// </summary>
        /// <returns>True if can borrow, false otherwise</returns>
        public bool CanBorrowBooks()
        {
            /*
             * בדיקת יכולת השאלה בספרייה KYKY
             * Check borrowing capability in KYKY library
             */
            return IsActive && CurrentBooksCount < MaxBooksAllowed;
        }
        
        /// <summary>
        /// Increment borrowed books count for KYKY member
        /// הגדלת מונה ספרים מושאלים לחבר KYKY
        /// </summary>
        public void BorrowBook()
        {
            if (CanBorrowBooks())
            {
                CurrentBooksCount++; // הגדלת מספר הספרים - Increment book count
                
                // רישום השאלה - Log borrowing
                Console.WriteLine($"KYKY member {GetFullName()} borrowed a book. Current count: {CurrentBooksCount}");
            }
            else
            {
                /*
                 * הודעה על אי יכולת השאלה
                 * Message about inability to borrow
                 */
                throw new InvalidOperationException($"KYKY member {GetFullName()} cannot borrow more books");
            }
        }
        
        /// <summary>
        /// Decrement borrowed books count for KYKY member
        /// הקטנת מונה ספרים מושאלים לחבר KYKY
        /// </summary>
        public void ReturnBook()
        {
            if (CurrentBooksCount > 0)
            {
                CurrentBooksCount--; // הקטנת מספר הספרים - Decrement book count
                
                /*
                 * רישום החזרה
                 * Log return
                 */
                Console.WriteLine($"KYKY member {GetFullName()} returned a book. Current count: {CurrentBooksCount}");
            }
        }
        
        /// <summary>
        /// Get member age if date of birth is available
        /// קבלת גיל החבר אם תאריך הלידה זמין
        /// </summary>
        public int? GetAge()
        {
            if (DateOfBirth.HasValue)
            {
                // חישוב גיל - Calculate age
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                
                if (DateOfBirth.Value.Date > today.AddYears(-age))
                    age--;
                
                return age;
            }
            
            return null; // גיל לא זמין - Age not available
        }
        
        /// <summary>
        /// Get membership duration in KYKY library
        /// קבלת משך החברות בספרייה KYKY
        /// </summary>
        /// <returns>Time span since joining</returns>
        public TimeSpan GetMembershipDuration()
        {
            return DateTime.Now - JoinDate;
        }
        
        /// <summary>
        /// Convert member to string representation for KYKY reports
        /// המרת חבר לייצוג מחרוזת לדוחות KYKY
        /// </summary>
        public override string ToString()
        {
            return $"KYKY Library Member: {GetFullName()} - {MembershipType} [{Email}]";
        }
    }
    
    /// <summary>
    /// Member preferences in KYKY library system
    /// העדפות חבר במערכת ספרייה KYKY
    /// </summary>
    public class MemberPreferences
    {
        /// <summary>
        /// Preferred book categories in KYKY library
        /// קטגוריות ספרים מועדפות בספרייה KYKY
        /// </summary>
        public List<string> PreferredCategories { get; set; } = new List<string>();
        
        /// <summary>
        /// Preferred language for books in KYKY system
        /// שפה מועדפת לספרים במערכת KYKY
        /// </summary>
        public string PreferredLanguage { get; set; } = "Hebrew";
        
        /// <summary>
        /// Email notifications enabled for KYKY member
        /// התראות דוא"ל מופעלות לחבר KYKY
        /// </summary>
        public bool EmailNotifications { get; set; } = true;
        
        /// <summary>
        /// SMS notifications enabled for KYKY member
        /// התראות SMS מופעלות לחבר KYKY
        /// </summary>
        public bool SmsNotifications { get; set; } = false;
        
        /// <summary>
        /// Add preferred category for KYKY member
        /// הוספת קטגוריה מועדפת לחבר KYKY
        /// </summary>
        /// <param name="category">Category to add</param>
        public void AddPreferredCategory(string category)
        {
            if (!string.IsNullOrEmpty(category) && !PreferredCategories.Contains(category))
            {
                PreferredCategories.Add(category);
                Console.WriteLine($"Added preferred category '{category}' for KYKY member");
            }
        }
    }
}
