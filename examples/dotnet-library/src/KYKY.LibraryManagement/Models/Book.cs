namespace KYKY.LibraryManagement.Models
{
    /// <summary>
    /// Book model for KYKY Library Management System
    /// מודל ספר למערכת ניהול הספרייה של KYKY
    /// 
    /// Represents a book in the KYKY library catalog
    /// מייצג ספר בקטלוג הספרייה של KYKY
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Unique identifier for book in KYKY system
        /// מזהה יחיד לספר במערכת KYKY
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Title of the book in KYKY catalog
        /// כותרת הספר בקטלוג KYKY
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Author of the book
        /// מחבר הספר
        /// </summary>
        public string Author { get; set; } = string.Empty;
        
        /// <summary>
        /// ISBN number for KYKY book tracking
        /// מספר ISBN למעקב ספרי KYKY
        /// </summary>
        public string ISBN { get; set; } = string.Empty;
        
        /// <summary>
        /// Year the book was published
        /// שנת הוצאת הספר
        /// </summary>
        public int PublishedYear { get; set; }
        
        /// <summary>
        /// Publisher of the book
        /// מוציא לאור של הספר
        /// </summary>
        public string Publisher { get; set; } = string.Empty;
        
        /// <summary>
        /// Number of pages in the book
        /// מספר דפים בספר
        /// </summary>
        public int? Pages { get; set; }
        
        /// <summary>
        /// Category of the book in KYKY classification
        /// קטגוריית הספר בסיווג KYKY
        /// </summary>
        public string Category { get; set; } = "General";
        
        /// <summary>
        /// Language of the book
        /// שפת הספר
        /// </summary>
        public string Language { get; set; } = "Hebrew"; // ברירת מחדל עברית - Default Hebrew
        
        /// <summary>
        /// Availability status in KYKY library
        /// סטטוס זמינות בספרייה KYKY
        /// </summary>
        public bool IsAvailable { get; set; } = true;
        
        /// <summary>
        /// Date when book was added to KYKY catalog
        /// תאריך הוספת הספר לקטלוג KYKY
        /// </summary>
        public DateTime DateAdded { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Location of book in KYKY library
        /// מיקום הספר בספרייה KYKY
        /// </summary>
        public BookLocation Location { get; set; } = new BookLocation();
        
        /// <summary>
        /// Summary or description of the book
        /// תקציר או תיאור הספר
        /// </summary>
        public string? Summary { get; set; }
        
        /// <summary>
        /// Get full book information for KYKY display
        /// קבלת מידע מלא על הספר לתצוגה KYKY
        /// </summary>
        /// <returns>Formatted book information</returns>
        public string GetBookInfo()
        {
            /*
             * יצירת מידע מפורט על הספר
             * Create detailed book information
             */
            return $"KYKY Book: {Title} by {Author} ({PublishedYear}) - ISBN: {ISBN}";
        }
        
        /// <summary>
        /// Check if book is available for loan in KYKY system
        /// בדיקה האם הספר זמין להשאלה במערכת KYKY
        /// </summary>
        /// <returns>True if available, false otherwise</returns>
        public bool CanBeBorrowed()
        {
            // בדיקת זמינות בספרייה KYKY - Check availability in KYKY library
            return IsAvailable && !string.IsNullOrEmpty(Title);
        }
        
        /// <summary>
        /// Mark book as borrowed in KYKY system
        /// סימון הספר כמושאל במערכת KYKY
        /// </summary>
        public void MarkAsBorrowed()
        {
            //one more commentßßß
            IsAvailable = false; // סימון כלא זמין - Mark as unavailable
            
            // רישום השאלה - Log borrowing
            Console.WriteLine($"Book '{Title}' marked as borrowed in KYKY library");
        }
        
        /// <summary>
        /// Mark book as returned to KYKY library
        /// סימון הספר כמוחזר לספרייה KYKY
        /// </summary>
        public void MarkAsReturned()
        {
            IsAvailable = true; // סימון כזמין - Mark as available
            
            // רישום החזרה - Log return
            Console.WriteLine($"Book '{Title}' returned to KYKY library");
        }
        
        /// <summary>
        /// Get book age in years
        /// קבלת גיל הספר בשנים
        /// </summary>
        public int GetBookAge()
        {
            /*
             * חישוב גיל הספר
             * Calculate book age
             */
            return DateTime.Now.Year - PublishedYear;
        }
        
        /// <summary>
        /// Convert book to display format for KYKY reports
        /// המרת הספר לפורמט תצוגה לדוחות KYKY
        /// </summary>
        public override string ToString()
        {
            return $"KYKY Library Book: {Title} - {Author} [{Category}]";
        }
    }
    
    /// <summary>
    /// Book location in KYKY library
    /// מיקום ספר בספרייה KYKY
    /// </summary>
    public class BookLocation
    {
        /// <summary>
        /// Floor number in KYKY library building
        /// מספר קומה בבניין ספרייה KYKY
        /// </summary>
        public int Floor { get; set; } = 1;
        
        /// <summary>
        /// Section in KYKY library
        /// מדור בספרייה KYKY
        /// </summary>
        public string Section { get; set; } = "General";
        
        /// <summary>
        /// Shelf number in KYKY library
        /// מספר מדף בספרייה KYKY
        /// </summary>
        public string Shelf { get; set; } = "A1";
        
        /// <summary>
        /// Get complete location string for KYKY library
        /// קבלת מחרוזת מיקום מלאה לספרייה KYKY
        /// </summary>
        /// <returns>Formatted location string</returns>
        public override string ToString()
        {
            return $"KYKY Library - Floor {Floor}, Section {Section}, Shelf {Shelf}";
        }
    }
}
