namespace MyCompany.LibraryManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public int? Pages { get; set; }
        public string Category { get; set; } = "General";
        public string Language { get; set; } = "Hebrew"; 
        public bool IsAvailable { get; set; } = true;
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public BookLocation Location { get; set; } = new BookLocation();
        public string? Summary { get; set; }
        public string GetBookInfo()
        {
            return $"MyCompany Book: {Title} by {Author} ({PublishedYear}) - ISBN: {ISBN}";
        }
        public bool CanBeBorrowed()
        {
            return IsAvailable && !string.IsNullOrEmpty(Title);
        }
        public void MarkAsBorrowed()
        {
            IsAvailable = false; 
            Console.WriteLine($"Book '{Title}' marked as borrowed in MyCompany library");
        }
        public void MarkAsReturned()
        {
            IsAvailable = true; 
            Console.WriteLine($"Book '{Title}' returned to MyCompany library");
        }
        public int GetBookAge()
        {
            return DateTime.Now.Year - PublishedYear;
        }
        public override string ToString()
        {
            return $"MyCompany Library Book: {Title} - {Author} [{Category}]";
        }
    }
    public class BookLocation
    {
        public int Floor { get; set; } = 1;
        public string Section { get; set; } = "General";
        public string Shelf { get; set; } = "A1";
        public override string ToString()
        {
            return $"MyCompany Library - Floor {Floor}, Section {Section}, Shelf {Shelf}";
        }
    }
}