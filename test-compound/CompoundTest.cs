using System;
namespace MyCompanyProject
{
    public class MyCompanyService
    {
        private string MyCompanyApiKey = "key";
        public void ProcessMyCompanyData()
        {
            var serviceMyCompany = new ServiceMyCompany();
            var dataMyCompanyProcessor = new DataMyCompanyProcessor();
            Console.WriteLine("MyCompany Corp processing");
        }
    }
    public class ServiceMyCompany { }
    public class DataMyCompanyProcessor { }
}