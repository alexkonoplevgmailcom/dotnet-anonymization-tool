using MyCompany.LibraryManagement.Models;
namespace MyCompany.LibraryManagement.Services
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(Book book);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<int> GetTotalBooksCountAsync();
        Task<Book?> GetBookByIdAsync(int id);
    }
    public class BookService : IBookService
    {
        private readonly List<Book> _kykyBooks;
        public BookService()
        {
            _kykyBooks = new List<Book>(); 
            Console.WriteLine("MyCompany Book Service initialized");
            Console.WriteLine("שירות ספרי MyCompany אותחל");
        }
        public async Task<Book> AddBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book), "Book cannot be null for MyCompany library");
            if (string.IsNullOrEmpty(book.Title))
                throw new ArgumentException("Book title is required for MyCompany catalog");
            if (!string.IsNullOrEmpty(book.ISBN) && 
                _kykyBooks.Any(b => b.ISBN == book.ISBN))
            {
                throw new InvalidOperationException($"Book with ISBN {book.ISBN} already exists in MyCompany catalog");
            }
            _kykyBooks.Add(book);
            Console.WriteLine($"Book '{book.Title}' added to MyCompany library catalog");
            Console.WriteLine($"הספר '{book.Title}' נוסף לקטלוג ספרייה MyCompany");
            return await Task.FromResult(book);
        }
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            Console.WriteLine($"Retrieved {_kykyBooks.Count} books from MyCompany catalog");
            Console.WriteLine($"נטענו {_kykyBooks.Count} ספרים מקטלוג MyCompany");
            return await Task.FromResult(_kykyBooks.ToList());
        }
        public async Task<int> GetTotalBooksCountAsync()
        {
            var count = _kykyBooks.Count;
            Console.WriteLine($"Total books in MyCompany library: {count}");
            return await Task.FromResult(count);
        }
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var book = _kykyBooks.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                Console.WriteLine($"Book found in MyCompany catalog: {book.Title}");
            }
            else
            {
                Console.WriteLine($"Book with ID {id} not found in MyCompany catalog");
            }
            return await Task.FromResult(book);
        }
        public async Task<IEnumerable<Book>> SearchBooksByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                return await Task.FromResult(Enumerable.Empty<Book>());
            var matchingBooks = _kykyBooks
                .Where(book => book.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
                .ToList();
            Console.WriteLine($"Found {matchingBooks.Count} books matching '{title}' in MyCompany catalog");
            return await Task.FromResult(matchingBooks);
        }
        public async Task<bool> UpdateBookAvailabilityAsync(int bookId, bool isAvailable)
        {
            var book = await GetBookByIdAsync(bookId);
            if (book == null)
                return false;
            book.IsAvailable = isAvailable;
            Console.WriteLine($"Book '{book.Title}' availability updated to {isAvailable} in MyCompany system");
            return true;
        }
    }
}