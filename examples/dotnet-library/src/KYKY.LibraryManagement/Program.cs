using MyCompany.LibraryManagement.Services;
using MyCompany.LibraryManagement.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace MyCompany.LibraryManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to MyCompany Library Management System!");
            Console.WriteLine("ברוכים הבאים למערכת ניהול הספרייה של MyCompany!");
            var host = CreateHostBuilder(args).Build();
            var bookService = host.Services.GetRequiredService<IBookService>();
            var memberService = host.Services.GetRequiredService<IMemberService>();
            var loanService = host.Services.GetRequiredService<ILoanService>();
            try
            {
                await InitializeKYKYSystem(bookService, memberService, loanService);
                await RunKYKYLibrarySystem(bookService, memberService, loanService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MyCompany Library System: {ex.Message}");
                Console.WriteLine($"שגיאה במערכת ספרייה MyCompany: {ex.Message}");
            }
            Console.WriteLine("MyCompany Library Management System shutting down...");
            Console.WriteLine("מערכת ניהול הספרייה של MyCompany נסגרת...");
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IBookService, BookService>();
                    services.AddScoped<IMemberService, MemberService>();
                    services.AddScoped<ILoanService, LoanService>();
                    services.AddLogging();
                    services.AddMemoryCache();
                });
        private static async Task InitializeKYKYSystem(
            IBookService bookService, 
            IMemberService memberService, 
            ILoanService loanService)
        {
            Console.WriteLine("Initializing MyCompany Library System...");
            Console.WriteLine("מאתחל מערכת ספרייה MyCompany...");
            await bookService.AddBookAsync(new Book
            {
                Id = 1,
                Title = "MyCompany Programming Guide",
                Author = "MyCompany Development Team",
                ISBN = "978-0-123456-78-9",
                PublishedYear = 2024,
                Publisher = "MyCompany Publications"
            });
            await bookService.AddBookAsync(new Book
            {
                Id = 2,
                Title = "Advanced MyCompany Architecture",
                Author = "MyCompany Architects",
                ISBN = "978-0-987654-32-1",
                PublishedYear = 2024,
                Publisher = "MyCompany Technical Press"
            });
            await memberService.AddMemberAsync(new Member
            {
                Id = 1,
                FirstName = "David",
                LastName = "Cohen",
                Email = "david.cohen@MyCompany.com",
                MembershipType = "MyCompany Employee"
            });
            await memberService.AddMemberAsync(new Member
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Levy", 
                Email = "sarah.levy@MyCompany.com",
                MembershipType = "MyCompany Premium"
            });
            Console.WriteLine("MyCompany Library System initialized successfully!");
            Console.WriteLine("מערכת ספרייה MyCompany אותחלה בהצלחה!");
        }
        private static async Task RunKYKYLibrarySystem(
            IBookService bookService,
            IMemberService memberService, 
            ILoanService loanService)
        {
            var totalBooks = await bookService.GetTotalBooksCountAsync();
            var totalMembers = await memberService.GetTotalMembersCountAsync();
            Console.WriteLine($"\n=== MyCompany Library Statistics ===");
            Console.WriteLine($"=== סטטיסטיקות ספרייה MyCompany ===");
            Console.WriteLine($"Total Books in MyCompany catalog: {totalBooks}");
            Console.WriteLine($"סך ספרים בקטלוג MyCompany: {totalBooks}");
            Console.WriteLine($"Total MyCompany Members: {totalMembers}");
            Console.WriteLine($"סך חברי MyCompany: {totalMembers}");
            await SimulateKYKYLibraryActivity(bookService, memberService, loanService);
        }
        private static async Task SimulateKYKYLibraryActivity(
            IBookService bookService,
            IMemberService memberService,
            ILoanService loanService)
        {
            Console.WriteLine("\n=== Simulating MyCompany Library Activity ===");
            Console.WriteLine("=== סימולציה של פעילות ספרייה MyCompany ===");
            var books = await bookService.GetAllBooksAsync();
            var members = await memberService.GetAllMembersAsync();
            if (books.Any() && members.Any())
            {
                var firstBook = books.First();
                var firstMember = members.First();
                await loanService.CreateLoanAsync(firstMember.Id, firstBook.Id);
                Console.WriteLine($"Book '{firstBook.Title}' loaned to {firstMember.FirstName} {firstMember.LastName} in MyCompany system");
                Console.WriteLine($"הספר '{firstBook.Title}' הושאל ל{firstMember.FirstName} {firstMember.LastName} במערכת MyCompany");
            }
        }
    }
}