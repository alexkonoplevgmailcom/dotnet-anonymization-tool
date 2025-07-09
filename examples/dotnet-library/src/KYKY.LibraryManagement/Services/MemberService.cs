using KYKY.LibraryManagement.Models;

namespace KYKY.LibraryManagement.Services
{
    /// <summary>
    /// Member Service interface for KYKY Library Management System
    /// ממשק שירות חברים למערכת ניהול הספרייה של KYKY
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// Add new member to KYKY library
        /// הוספת חבר חדש לספרייה KYKY
        /// </summary>
        Task<Member> AddMemberAsync(Member member);
        
        /// <summary>
        /// Get all members from KYKY library
        /// קבלת כל החברים מספרייה KYKY
        /// </summary>
        Task<IEnumerable<Member>> GetAllMembersAsync();
        
        /// <summary>
        /// Get total count of members in KYKY library
        /// קבלת ספירה כוללת של חברים בספרייה KYKY
        /// </summary>
        Task<int> GetTotalMembersCountAsync();
        
        /// <summary>
        /// Find member by ID in KYKY system
        /// חיפוש חבר לפי מזהה במערכת KYKY
        /// </summary>
        Task<Member?> GetMemberByIdAsync(int id);
    }
    
    /// <summary>
    /// Member Service interface for KYKY Library Management System
    /// ממשק שירות הזמנות למערכת ניהול הספרייה של KYKY
    /// </summary>
    public interface ILoanService
    {
        /// <summary>
        /// Create new loan in KYKY library system
        /// יצירת השאלה חדשה במערכת ספרייה KYKY
        /// </summary>
        Task<bool> CreateLoanAsync(int memberId, int bookId);
    }
    
    /// <summary>
    /// Member Service implementation for KYKY Library Management System
    /// מימוש שירות חברים למערכת ניהול הספרייה של KYKY
    /// 
    /// This service handles all member-related operations for KYKY library
    /// שירות זה מטפל בכל הפעולות הקשורות לחברים של ספרייה KYKY
    /// </summary>
    public class MemberService : IMemberService
    {
        // אוסף חברי KYKY - KYKY members collection
        private readonly List<Member> _kykyMembers;
        
        /// <summary>
        /// Constructor for KYKY Member Service
        /// בנאי לשירות חברי KYKY
        /// </summary>
        public MemberService()
        {
            _kykyMembers = new List<Member>(); // אתחול רשימת חברי KYKY - Initialize KYKY members list
            
            /*
             * הודעת אתחול שירות
             * Service initialization message
             */
            Console.WriteLine("KYKY Member Service initialized");
            Console.WriteLine("שירות חברי KYKY אותחל");
        }
        
        /// <summary>
        /// Add new member to KYKY library
        /// הוספת חבר חדש לספרייה KYKY
        /// </summary>
        /// <param name="member">החבר להוספה - Member to add</param>
        /// <returns>החבר שנוסף - Added member</returns>
        public async Task<Member> AddMemberAsync(Member member)
        {
            // בדיקת תקינות נתוני החבר - Validate member data
            if (member == null)
                throw new ArgumentNullException(nameof(member), "Member cannot be null for KYKY library");
            
            if (string.IsNullOrEmpty(member.FirstName) || string.IsNullOrEmpty(member.LastName))
                throw new ArgumentException("Member name is required for KYKY library");
            
            /*
             * בדיקת קיום חבר עם אותו דוא"ל במערכת KYKY
             * Check if member with same email exists in KYKY system
             */
            if (!string.IsNullOrEmpty(member.Email) && 
                _kykyMembers.Any(m => m.Email == member.Email))
            {
                throw new InvalidOperationException($"Member with email {member.Email} already exists in KYKY library");
            }
            
            // הוספת החבר לספרייה KYKY - Add member to KYKY library
            _kykyMembers.Add(member);
            
            // רישום הוספת חבר - Log member addition
            Console.WriteLine($"Member '{member.GetFullName()}' added to KYKY library");
            Console.WriteLine($"החבר '{member.GetFullName()}' נוסף לספרייה KYKY");
            
            /*
             * החזרת החבר שנוסף
             * Return added member
             */
            return await Task.FromResult(member);
        }
        
        /// <summary>
        /// Get all members from KYKY library
        /// קבלת כל החברים מספרייה KYKY
        /// </summary>
        /// <returns>אוסף חברי KYKY - Collection of KYKY members</returns>
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            /*
             * רישום קבלת רשימת חברים
             * Log members list retrieval
             */
            Console.WriteLine($"Retrieved {_kykyMembers.Count} members from KYKY library");
            Console.WriteLine($"נטענו {_kykyMembers.Count} חברים מספרייה KYKY");
            
            // החזרת עותק של רשימת חברי KYKY - Return copy of KYKY members list
            return await Task.FromResult(_kykyMembers.ToList());
        }
        
        /// <summary>
        /// Get total count of members in KYKY library
        /// קבלת ספירה כוללת של חברים בספרייה KYKY
        /// </summary>
        /// <returns>מספר החברים - Number of members</returns>
        public async Task<int> GetTotalMembersCountAsync()
        {
            var count = _kykyMembers.Count;
            
            // רישום ספירת חברים - Log member count
            Console.WriteLine($"Total members in KYKY library: {count}");
            
            /*
             * החזרת מספר החברים
             * Return member count
             */
            return await Task.FromResult(count);
        }
        
        /// <summary>
        /// Find member by ID in KYKY system
        /// חיפוש חבר לפי מזהה במערכת KYKY
        /// </summary>
        /// <param name="id">מזהה החבר - Member ID</param>
        /// <returns>החבר או null - Member or null</returns>
        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            /*
             * חיפוש חבר במערכת KYKY
             * Search for member in KYKY system
             */
            var member = _kykyMembers.FirstOrDefault(m => m.Id == id);
            
            if (member != null)
            {
                // רישום מציאת חבר - Log member found
                Console.WriteLine($"Member found in KYKY library: {member.GetFullName()}");
            }
            else
            {
                /*
                 * רישום חבר לא נמצא
                 * Log member not found
                 */
                Console.WriteLine($"Member with ID {id} not found in KYKY library");
            }
            
            return await Task.FromResult(member);
        }
        
        /// <summary>
        /// Search members by name in KYKY library
        /// חיפוש חברים לפי שם בספרייה KYKY
        /// </summary>
        public async Task<IEnumerable<Member>> SearchMembersByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await Task.FromResult(Enumerable.Empty<Member>());
            
            /*
             * חיפוש חברים לפי שם במערכת KYKY
             * Search members by name in KYKY system
             */
            var matchingMembers = _kykyMembers
                .Where(member => member.GetFullName().Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            Console.WriteLine($"Found {matchingMembers.Count} members matching '{name}' in KYKY library");
            
            return await Task.FromResult(matchingMembers);
        }
        
        /// <summary>
        /// Update member status in KYKY system
        /// עדכון סטטוס חבר במערכת KYKY
        /// </summary>
        /// <param name="memberId">Member ID to update</param>
        /// <param name="isActive">New active status</param>
        /// <returns>True if updated successfully</returns>
        public async Task<bool> UpdateMemberStatusAsync(int memberId, bool isActive)
        {
            var member = await GetMemberByIdAsync(memberId);
            if (member == null)
                return false;
            
            member.IsActive = isActive;
            
            /*
             * רישום עדכון סטטוס
             * Log status update
             */
            Console.WriteLine($"Member '{member.GetFullName()}' status updated to {(isActive ? "Active" : "Inactive")} in KYKY system");
            
            return true;
        }
    }
    
    /// <summary>
    /// Loan Service implementation for KYKY Library Management System
    /// מימוש שירות השאלות למערכת ניהול הספרייה של KYKY
    /// </summary>
    public class LoanService : ILoanService
    {
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        
        /// <summary>
        /// Constructor for KYKY Loan Service
        /// בנאי לשירות השאלות KYKY
        /// </summary>
        public LoanService(IBookService bookService, IMemberService memberService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            
            /*
             * הודעת אתחול שירות השאלות
             * Loan service initialization message
             */
            Console.WriteLine("KYKY Loan Service initialized");
            Console.WriteLine("שירות השאלות KYKY אותחל");
        }
        
        /// <summary>
        /// Create new loan in KYKY library system
        /// יצירת השאלה חדשה במערכת ספרייה KYKY
        /// </summary>
        public async Task<bool> CreateLoanAsync(int memberId, int bookId)
        {
            try
            {
                // קבלת פרטי חבר וספר - Get member and book details
                var member = await _memberService.GetMemberByIdAsync(memberId);
                var book = await _bookService.GetBookByIdAsync(bookId);
                
                if (member == null)
                {
                    Console.WriteLine($"Member with ID {memberId} not found in KYKY system");
                    return false;
                }
                
                if (book == null)
                {
                    Console.WriteLine($"Book with ID {bookId} not found in KYKY catalog");
                    return false;
                }
                
                /*
                 * בדיקת יכולת השאלה
                 * Check borrowing capability
                 */
                if (!member.CanBorrowBooks())
                {
                    Console.WriteLine($"KYKY member {member.GetFullName()} cannot borrow more books");
                    return false;
                }
                
                if (!book.CanBeBorrowed())
                {
                    Console.WriteLine($"Book '{book.Title}' is not available for borrowing in KYKY library");
                    return false;
                }
                
                // ביצוע השאלה - Perform loan
                member.BorrowBook();
                book.MarkAsBorrowed();
                
                /*
                 * רישום השאלה מוצלחת
                 * Log successful loan
                 */
                Console.WriteLine($"KYKY Loan created: '{book.Title}' borrowed by {member.GetFullName()}");
                Console.WriteLine($"השאלה KYKY נוצרה: '{book.Title}' הושאל על ידי {member.GetFullName()}");
                
                return true;
            }
            catch (Exception ex)
            {
                /*
                 * שגיאה ביצירת השאלה
                 * Error creating loan
                 */
                Console.WriteLine($"Error creating KYKY loan: {ex.Message}");
                return false;
            }
        }
    }
}
