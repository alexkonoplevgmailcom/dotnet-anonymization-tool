namespace MyCompany.LibraryManagement.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string MembershipType { get; set; } = "MyCompany Standard";
        public bool IsActive { get; set; } = true;
        public DateTime? DateOfBirth { get; set; }
        public int MaxBooksAllowed { get; set; } = 5;
        public int CurrentBooksCount { get; set; } = 0;
        public MemberPreferences Preferences { get; set; } = new MemberPreferences();
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
        public string GetDisplayName()
        {
            return $"{GetFullName()} (MyCompany {MembershipType})";
        }
        public bool CanBorrowBooks()
        {
            return IsActive && CurrentBooksCount < MaxBooksAllowed;
        }
        public void BorrowBook()
        {
            if (CanBorrowBooks())
            {
                CurrentBooksCount++; 
                Console.WriteLine($"MyCompany member {GetFullName()} borrowed a book. Current count: {CurrentBooksCount}");
            }
            else
            {
                throw new InvalidOperationException($"MyCompany member {GetFullName()} cannot borrow more books");
            }
        }
        public void ReturnBook()
        {
            if (CurrentBooksCount > 0)
            {
                CurrentBooksCount--; 
                Console.WriteLine($"MyCompany member {GetFullName()} returned a book. Current count: {CurrentBooksCount}");
            }
        }
        public int? GetAge()
        {
            if (DateOfBirth.HasValue)
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > today.AddYears(-age))
                    age--;
                return age;
            }
            return null; 
        }
        public TimeSpan GetMembershipDuration()
        {
            return DateTime.Now - JoinDate;
        }
        public override string ToString()
        {
            return $"MyCompany Library Member: {GetFullName()} - {MembershipType} [{Email}]";
        }
    }
    public class MemberPreferences
    {
        public List<string> PreferredCategories { get; set; } = new List<string>();
        public string PreferredLanguage { get; set; } = "Hebrew";
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;
        public void AddPreferredCategory(string category)
        {
            if (!string.IsNullOrEmpty(category) && !PreferredCategories.Contains(category))
            {
                PreferredCategories.Add(category);
                Console.WriteLine($"Added preferred category '{category}' for MyCompany member");
            }
        }
    }
}