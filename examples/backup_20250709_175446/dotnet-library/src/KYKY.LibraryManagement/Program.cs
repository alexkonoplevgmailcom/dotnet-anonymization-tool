using KYKY.LibraryManagement.Services;
using KYKY.LibraryManagement.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KYKY.LibraryManagement
{
    /// <summary>
    /// KYKY Library Management System
    /// מערכת ניהול ספרייה של KYKY
    /// 
    /// Main application class for KYKY's library management platform
    /// מחלקה ראשית לפלטפורמת ניהול הספרייה של KYKY
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for KYKY Library Management System
        /// נקודת כניסה ראשית למערכת ניהול הספרייה של KYKY
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static async Task Main(string[] args)
        {
            // הדפסת הודעת פתיחה - Print welcome message
            Console.WriteLine("Welcome to KYKY Library Management System!");
            Console.WriteLine("ברוכים הבאים למערכת ניהול הספרייה של KYKY!");
            
            /*
             * הגדרת שירותי KYKY
             * Configure KYKY services
             */
            var host = CreateHostBuilder(args).Build();
            
            // קבלת שירותי KYKY מה-DI container - Get KYKY services from DI container
            var bookService = host.Services.GetRequiredService<IBookService>();
            var memberService = host.Services.GetRequiredService<IMemberService>();
            var loanService = host.Services.GetRequiredService<ILoanService>();
            
            try
            {
                // אתחול מערכת KYKY - Initialize KYKY system
                await InitializeKYKYSystem(bookService, memberService, loanService);
                
                /*
                 * הפעלת מערכת KYKY
                 * Start KYKY system
                 */
                await RunKYKYLibrarySystem(bookService, memberService, loanService);
            }
            catch (Exception ex)
            {
                // שגיאה במערכת KYKY - Error in KYKY system
                Console.WriteLine($"Error in KYKY Library System: {ex.Message}");
                Console.WriteLine($"שגיאה במערכת ספרייה KYKY: {ex.Message}");
            }
            
            Console.WriteLine("KYKY Library Management System shutting down...");
            Console.WriteLine("מערכת ניהול הספרייה של KYKY נסגרת...");
        }
        
        /// <summary>
        /// Create host builder for KYKY application
        /// יצירת בונה host ליישום KYKY
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // רישום שירותי KYKY - Register KYKY services
                    services.AddScoped<IBookService, BookService>();
                    services.AddScoped<IMemberService, MemberService>();
                    services.AddScoped<ILoanService, LoanService>();
                    
                    /*
                     * הוספת שירותי תשתית של KYKY
                     * Add KYKY infrastructure services
                     */
                    services.AddLogging();
                    services.AddMemoryCache();
                });
        
        /// <summary>
        /// Initialize KYKY library system with sample data
        /// אתחול מערכת ספרייה KYKY עם נתוני דוגמה
        /// </summary>
        private static async Task InitializeKYKYSystem(
            IBookService bookService, 
            IMemberService memberService, 
            ILoanService loanService)
        {
            Console.WriteLine("Initializing KYKY Library System...");
            Console.WriteLine("מאתחל מערכת ספרייה KYKY...");
            
            // הוספת ספרים לקטלוג KYKY - Add books to KYKY catalog
            await bookService.AddBookAsync(new Book
            {
                Id = 1,
                Title = "KYKY Programming Guide",
                Author = "KYKY Development Team",
                ISBN = "978-0-123456-78-9",
                PublishedYear = 2024,
                Publisher = "KYKY Publications"
            });
            
            await bookService.AddBookAsync(new Book
            {
                Id = 2,
                Title = "Advanced KYKY Architecture",
                Author = "KYKY Architects",
                ISBN = "978-0-987654-32-1",
                PublishedYear = 2024,
                Publisher = "KYKY Technical Press"
            });
            
            /*
             * הוספת חברי ספרייה KYKY - Add KYKY library members
             */
            await memberService.AddMemberAsync(new Member
            {
                Id = 1,
                FirstName = "David",
                LastName = "Cohen",
                Email = "david.cohen@kyky.com",
                MembershipType = "KYKY Employee"
            });
            
            await memberService.AddMemberAsync(new Member
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Levy", 
                Email = "sarah.levy@kyky.com",
                MembershipType = "KYKY Premium"
            });
            
            Console.WriteLine("KYKY Library System initialized successfully!");
            Console.WriteLine("מערכת ספרייה KYKY אותחלה בהצלחה!");
        }
        
        /*
         * הפעלת מערכת ספרייה KYKY
         * Run KYKY library system
         */
        private static async Task RunKYKYLibrarySystem(
            IBookService bookService,
            IMemberService memberService, 
            ILoanService loanService)
        {
            // הצגת סטטיסטיקות מערכת KYKY - Display KYKY system statistics
            var totalBooks = await bookService.GetTotalBooksCountAsync();
            var totalMembers = await memberService.GetTotalMembersCountAsync();
            
            Console.WriteLine($"\n=== KYKY Library Statistics ===");
            Console.WriteLine($"=== סטטיסטיקות ספרייה KYKY ===");
            Console.WriteLine($"Total Books in KYKY catalog: {totalBooks}");
            Console.WriteLine($"סך ספרים בקטלוג KYKY: {totalBooks}");
            Console.WriteLine($"Total KYKY Members: {totalMembers}");
            Console.WriteLine($"סך חברי KYKY: {totalMembers}");
            
            /*
             * סימולציה של פעילות ספרייה KYKY
             * Simulate KYKY library activity
             */
            await SimulateKYKYLibraryActivity(bookService, memberService, loanService);
        }
        
        /// <summary>
        /// Simulate library activity in KYKY system
        /// סימולציה של פעילות ספרייה במערכת KYKY
        /// </summary>
        private static async Task SimulateKYKYLibraryActivity(
            IBookService bookService,
            IMemberService memberService,
            ILoanService loanService)
        {
            Console.WriteLine("\n=== Simulating KYKY Library Activity ===");
            Console.WriteLine("=== סימולציה של פעילות ספרייה KYKY ===");
            
            // קבלת ספר ראשון וחבר ראשון - Get first book and first member
            var books = await bookService.GetAllBooksAsync();
            var members = await memberService.GetAllMembersAsync();
            
            if (books.Any() && members.Any())
            {
                var firstBook = books.First();
                var firstMember = members.First();
                
                /*
                 * יצירת השאלה במערכת KYKY
                 * Create loan in KYKY system
                 */
                await loanService.CreateLoanAsync(firstMember.Id, firstBook.Id);
                
                Console.WriteLine($"Book '{firstBook.Title}' loaned to {firstMember.FirstName} {firstMember.LastName} in KYKY system");
                Console.WriteLine($"הספר '{firstBook.Title}' הושאל ל{firstMember.FirstName} {firstMember.LastName} במערכת KYKY");
            }
        }
    }
}
