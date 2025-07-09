using KYKY.LibraryManagement.Models;

namespace KYKY.LibraryManagement.Services
{
    /// <summary>
    /// Book Service interface for KYKY Library Management System
    /// ממשק שירות ספרים למערכת ניהול הספרייה של KYKY
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Add new book to KYKY library catalog
        /// הוספת ספר חדש לקטלוג ספרייה KYKY
        /// </summary>
        Task<Book> AddBookAsync(Book book);
        
        /// <summary>
        /// Get all books from KYKY catalog
        /// קבלת כל הספרים מקטלוג KYKY
        /// </summary>
        Task<IEnumerable<Book>> GetAllBooksAsync();
        
        /// <summary>
        /// Get total count of books in KYKY library
        /// קבלת ספירה כוללת של ספרים בספרייה KYKY
        /// </summary>
        Task<int> GetTotalBooksCountAsync();
        
        /// <summary>
        /// Find book by ID in KYKY system
        /// חיפוש ספר לפי מזהה במערכת KYKY
        /// </summary>
        Task<Book?> GetBookByIdAsync(int id);
    }
    
    /// <summary>
    /// Book Service implementation for KYKY Library Management System
    /// מימוש שירות ספרים למערכת ניהול הספרייה של KYKY
    /// 
    /// This service handles all book-related operations for KYKY library
    /// שירות זה מטפל בכל הפעולות הקשורות לספרים של ספרייה KYKY
    /// </summary>
    public class BookService : IBookService
    {
        // אוסף ספרי KYKY - KYKY books collection
        private readonly List<Book> _kykyBooks;
        
        /// <summary>
        /// Constructor for KYKY Book Service
        /// בנאי לשירות ספרי KYKY
        /// </summary>
        public BookService()
        {
            _kykyBooks = new List<Book>(); // אתחול רשימת ספרי KYKY - Initialize KYKY books list
            
            /*
             * הודעת אתחול שירות
             * Service initialization message
             */
            Console.WriteLine("KYKY Book Service initialized");
            Console.WriteLine("שירות ספרי KYKY אותחל");
        }
        
        /// <summary>
        /// Add new book to KYKY library catalog
        /// הוספת ספר חדש לקטלוג ספרייה KYKY
        /// </summary>
        /// <param name="book">הספר להוספה - Book to add</param>
        /// <returns>הספר שנוסף - Added book</returns>
        public async Task<Book> AddBookAsync(Book book)
        {
            // בדיקת תקינות נתוני הספר - Validate book data
            if (book == null)
                throw new ArgumentNullException(nameof(book), "Book cannot be null for KYKY library");
            
            if (string.IsNullOrEmpty(book.Title))
                throw new ArgumentException("Book title is required for KYKY catalog");
            
            /*
             * בדיקת קיום ספר עם אותו ISBN במערכת KYKY
             * Check if book with same ISBN exists in KYKY system
             */
            if (!string.IsNullOrEmpty(book.ISBN) && 
                _kykyBooks.Any(b => b.ISBN == book.ISBN))
            {
                throw new InvalidOperationException($"Book with ISBN {book.ISBN} already exists in KYKY catalog");
            }
            
            // הוספת הספר לקטלוג KYKY - Add book to KYKY catalog
            _kykyBooks.Add(book);
            
            // רישום הוספת ספר - Log book addition
            Console.WriteLine($"Book '{book.Title}' added to KYKY library catalog");
            Console.WriteLine($"הספר '{book.Title}' נוסף לקטלוג ספרייה KYKY");
            
            /*
             * החזרת הספר שנוסף
             * Return added book
             */
            return await Task.FromResult(book);
        }
        
        /// <summary>
        /// Get all books from KYKY catalog
        /// קבלת כל הספרים מקטלוג KYKY
        /// </summary>
        /// <returns>אוסף ספרי KYKY - Collection of KYKY books</returns>
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            /*
             * רישום קבלת רשימת ספרים
             * Log books list retrieval
             */
            Console.WriteLine($"Retrieved {_kykyBooks.Count} books from KYKY catalog");
            Console.WriteLine($"נטענו {_kykyBooks.Count} ספרים מקטלוג KYKY");
            
            // החזרת עותק של רשימת ספרי KYKY - Return copy of KYKY books list
            return await Task.FromResult(_kykyBooks.ToList());
        }
        
        /// <summary>
        /// Get total count of books in KYKY library
        /// קבלת ספירה כוללת של ספרים בספרייה KYKY
        /// </summary>
        /// <returns>מספר הספרים - Number of books</returns>
        public async Task<int> GetTotalBooksCountAsync()
        {
            var count = _kykyBooks.Count;
            
            // רישום ספירת ספרים - Log book count
            Console.WriteLine($"Total books in KYKY library: {count}");
            
            /*
             * החזרת מספר הספרים
             * Return book count
             */
            return await Task.FromResult(count);
        }
        
        /// <summary>
        /// Find book by ID in KYKY system
        /// חיפוש ספר לפי מזהה במערכת KYKY
        /// </summary>
        /// <param name="id">מזהה הספר - Book ID</param>
        /// <returns>הספר או null - Book or null</returns>
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            /*
             * חיפוש ספר במערכת KYKY
             * Search for book in KYKY system
             */
            var book = _kykyBooks.FirstOrDefault(b => b.Id == id);
            
            if (book != null)
            {
                // רישום מציאת ספר - Log book found
                Console.WriteLine($"Book found in KYKY catalog: {book.Title}");
            }
            else
            {
                /*
                 * רישום ספר לא נמצא
                 * Log book not found
                 */
                Console.WriteLine($"Book with ID {id} not found in KYKY catalog");
            }
            
            return await Task.FromResult(book);
        }
        
        /// <summary>
        /// Search books by title in KYKY catalog
        /// חיפוש ספרים לפי כותרת בקטלוג KYKY
        /// </summary>
        public async Task<IEnumerable<Book>> SearchBooksByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                return await Task.FromResult(Enumerable.Empty<Book>());
            
            /*
             * חיפוש ספרים לפי כותרת במערכת KYKY
             * Search books by title in KYKY system
             */
            var matchingBooks = _kykyBooks
                .Where(book => book.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            Console.WriteLine($"Found {matchingBooks.Count} books matching '{title}' in KYKY catalog");
            
            return await Task.FromResult(matchingBooks);
        }
        
        /// <summary>
        /// Update book availability in KYKY system
        /// עדכון זמינות ספר במערכת KYKY
        /// </summary>
        /// <param name="bookId">Book ID to update</param>
        /// <param name="isAvailable">New availability status</param>
        /// <returns>True if updated successfully</returns>
        public async Task<bool> UpdateBookAvailabilityAsync(int bookId, bool isAvailable)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book == null)
                return false;
            
            book.IsAvailable = isAvailable;
            
            /*
             * רישום עדכון זמינות
             * Log availability update
             */
            Console.WriteLine($"Book '{book.Title}' availability updated to {isAvailable} in KYKY system");
            
            return true;
        }
    }
}
