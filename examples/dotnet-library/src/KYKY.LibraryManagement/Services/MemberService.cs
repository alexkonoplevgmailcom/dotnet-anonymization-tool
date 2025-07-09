using MyCompany.LibraryManagement.Models;
namespace MyCompany.LibraryManagement.Services
{
    public interface IMemberService
    {
        Task<Member> AddMemberAsync(Member member);
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<int> GetTotalMembersCountAsync();
        Task<Member?> GetMemberByIdAsync(int id);
    }
    public interface ILoanService
    {
        Task<bool> CreateLoanAsync(int memberId, int bookId);
    }
    public class MemberService : IMemberService
    {
        private readonly List<Member> _kykyMembers;
        public MemberService()
        {
            _kykyMembers = new List<Member>(); 
            Console.WriteLine("MyCompany Member Service initialized");
            Console.WriteLine("שירות חברי MyCompany אותחל");
        }
        public async Task<Member> AddMemberAsync(Member member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member), "Member cannot be null for MyCompany library");
            if (string.IsNullOrEmpty(member.FirstName) || string.IsNullOrEmpty(member.LastName))
                throw new ArgumentException("Member name is required for MyCompany library");
            if (!string.IsNullOrEmpty(member.Email) && 
                _kykyMembers.Any(m => m.Email == member.Email))
            {
                throw new InvalidOperationException($"Member with email {member.Email} already exists in MyCompany library");
            }
            _kykyMembers.Add(member);
            Console.WriteLine($"Member '{member.GetFullName()}' added to MyCompany library");
            Console.WriteLine($"החבר '{member.GetFullName()}' נוסף לספרייה MyCompany");
            return await Task.FromResult(member);
        }
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            Console.WriteLine($"Retrieved {_kykyMembers.Count} members from MyCompany library");
            Console.WriteLine($"נטענו {_kykyMembers.Count} חברים מספרייה MyCompany");
            return await Task.FromResult(_kykyMembers.ToList());
        }
        public async Task<int> GetTotalMembersCountAsync()
        {
            var count = _kykyMembers.Count;
            Console.WriteLine($"Total members in MyCompany library: {count}");
            return await Task.FromResult(count);
        }
        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            var member = _kykyMembers.FirstOrDefault(m => m.Id == id);
            if (member != null)
            {
                Console.WriteLine($"Member found in MyCompany library: {member.GetFullName()}");
            }
            else
            {
                Console.WriteLine($"Member with ID {id} not found in MyCompany library");
            }
            return await Task.FromResult(member);
        }
        public async Task<IEnumerable<Member>> SearchMembersByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await Task.FromResult(Enumerable.Empty<Member>());
            var matchingMembers = _kykyMembers
                .Where(member => member.GetFullName().Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            Console.WriteLine($"Found {matchingMembers.Count} members matching '{name}' in MyCompany library");
            return await Task.FromResult(matchingMembers);
        }
        public async Task<bool> UpdateMemberStatusAsync(int memberId, bool isActive)
        {
            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
                return false;
            member.IsActive = isActive;
            Console.WriteLine($"Member '{member.GetFullName()}' status updated to {(isActive ? "Active" : "Inactive")} in MyCompany system");
            return true;
        }
    }
    public class LoanService : ILoanService
    {
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        public LoanService(IBookService bookService, IMemberService memberService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            Console.WriteLine("MyCompany Loan Service initialized");
            Console.WriteLine("שירות השאלות MyCompany אותחל");
        }
        public async Task<bool> CreateLoanAsync(int memberId, int bookId)
        {
            try
            {
                var member = await _memberService.GetMemberByIdAsync(memberId);
                var book = await _bookService.GetBookByIdAsync(bookId);
                if (member == null)
                {
                    Console.WriteLine($"Member with ID {memberId} not found in MyCompany system");
                    return false;
                }
                if (book == null)
                {
                    Console.WriteLine($"Book with ID {bookId} not found in MyCompany catalog");
                    return false;
                }
                if (!member.CanBorrowBooks())
                {
                    Console.WriteLine($"MyCompany member {member.GetFullName()} cannot borrow more books");
                    return false;
                }
                if (!book.CanBeBorrowed())
                {
                    Console.WriteLine($"Book '{book.Title}' is not available for borrowing in MyCompany library");
                    return false;
                }
                member.BorrowBook();
                book.MarkAsBorrowed();
                Console.WriteLine($"MyCompany Loan created: '{book.Title}' borrowed by {member.GetFullName()}");
                Console.WriteLine($"השאלה MyCompany נוצרה: '{book.Title}' הושאל על ידי {member.GetFullName()}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating MyCompany loan: {ex.Message}");
                return false;
            }
        }
    }
}