using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimchaFund.Data
{
    public class Contributors
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool AlwaysInclude { get; set; }
        public decimal Balance { get; set; }
    }
    public class Simcha
    {
        public int SimchaId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int ContributorCount { get; set; }
        public decimal Total { get; set; }
    }
   public class Deposit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class Contribution
    {
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
    public class SimchaDatabase 
    {
        private string _connectionString;
        public SimchaDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSimcha(Simcha simcha)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO SIMCHAS (Name, Date) VALUES (@Name, @Date)";
            connection.Open();
            
            cmd.Parameters.AddWithValue("@Name", simcha.Name);
            cmd.Parameters.AddWithValue("@Date", simcha.Date);
            cmd.ExecuteNonQuery();
        }

        public void AddContributor (Contributors contributor)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO CONTRIBUTORS (FirstName, LastName, PhoneNumber, AlwaysInclude, DateCreated) 
                                VALUES (@FirstName, @LastName, @PhoneNumber, @AlwaysInclude, @DateCreated)
                                SELECT SCOPE_IDENTITY()";
            connection.Open();
            cmd.Parameters.AddWithValue("@FirstName", contributor.FirstName);
            cmd.Parameters.AddWithValue("@LastName", contributor.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", contributor.PhoneNumber);
            cmd.Parameters.AddWithValue("@AlwaysInclude", contributor.AlwaysInclude);
            cmd.Parameters.AddWithValue("@DateCreated", contributor.DateCreated);
        }

        public void AddDeposit (Deposit deposit)
        {
            var connection = new SqlConnection();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits (Amount, Date, ContributorId) VALUES (@amount, @date, @contributorId)";
            connection.Open();
            cmd.Parameters.AddWithValue("@Amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@Date", deposit.Date);
            cmd.Parameters.AddWithValue("@ContributorId", deposit.Id);
        }

        public List<Simcha> GetAllSimchos()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM SIMCHAS";
            List<Simcha> simchos = new ();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                simchos.Add(new Simcha
                {
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"]
                });
            }

            return simchos;
        }

        public List<Contributors> GetAllContributors()
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributors";
            List<Contributors> contributors = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributors.Add(new Contributors
                {
                    FirstName=(string)reader["FirstName"],
                    LastName=(string)reader["LastName"],
                    PhoneNumber=(string)reader["PhoneNumber"],
                    AlwaysInclude=(bool)reader["AlwaysInclude"],
                    DateCreated=(DateTime)reader["DateCreated"]
                });
            }

            return contributors;
        }

        public List<Contribution> GetAllContributionsforPerson(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributions WHERE ContributorId = @id";
            List<Contribution> contributions = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributions.Add(new Contribution
                {
                    Amount = (decimal)reader["Amount"],
                });
            }

        }

        public List<Contributors>GetContributorsForSimcha (int simchaId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Contributors c JOIN Contributions ci " +
                                "ON c.id = ci.ContributorId " +
                                "WHERE SimchaId = @SimchaId";
            var reader = cmd.ExecuteReader();
            List<Contributors> contributors = new ();
            while (reader.Read())
            {
                contributors.Add(new Contributors
                {
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"]
                });

            }

            return contributors;
        }

        public List <Deposit>GetAllDeposits (int contributorId)
        {
            var connection = new SqlConnection(_connectionString);
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM DEPOSITS WHERE ContributorId = @contributorId";
            List<Deposit> deposits = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                deposits.Add(new Deposit
                {
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"]
                });
            }

            return deposits;
        }
    }
}
