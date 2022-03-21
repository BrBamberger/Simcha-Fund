using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SimchaFund.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimchaFund.Data;



namespace SimchaFund.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=true;";

        public IActionResult Index()
        {
            SimchaDatabase manager = new SimchaDatabase(_connectionString);
            SimchaViewModel vm = new SimchaViewModel();
            vm.Simchos = manager.GetAllSimchos();
            
            return View(vm);
        }

      public IActionResult Contributors()
        {
            SimchaDatabase db = new SimchaDatabase(_connectionString);
            ContributorViewModel vm = new();
            vm.Contributors = db.GetAllContributors();
            return View(vm);
        }

        public ShowHistory (int contributorId)
        {
            SimchaDatabase db = new SimchaDatabase(_connectionString);
            
            return View();
        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
