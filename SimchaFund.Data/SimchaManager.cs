using System;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class Contributors
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class Simcha
    {
        public int SimchaId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int ContributorCount { get; set; }
        public decimal Total { get; set; }
    }
   
    public class SimchaDatabase 
    {
        private string _connectionString;
        public SimchaDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
