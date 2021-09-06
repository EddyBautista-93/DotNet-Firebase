using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using net_core_app.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;


namespace net_core_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> About()
        {
            // simulate test user data and login timestamp
            var userId = "12345";
            var currentLoginTime = DateTime.Utc.ToString("MM/dd/yyyy HH:mm:ss");

            // save non identifying data to firebase
            var currentUserLogin = new LoginData() { TimeStampUtc = currentLoginTime};
            var firebaseClient = new firebaseClient("");
            var result = await firebaseClient
                .Child("Users/" + userID + "/Logins")
                .PostAsync(currentUserLogin);

            // grab data from firebase 
            var dbLogins = await firebaseClient
                .Child("Users")
                .Child(userID)
                .Child("Logins")
                .OnceAsync<LoginData>();

            var timestampList = new List<DateTime>();

            //Convert JSON data to original datatype
            foreach (var login in dbLogins)
            {
                timestampList.Add(Convert.ToDateTime(login.Object.TimestampUtc).ToLocalTime());
            }
            //Pass data to the view
            ViewBag.CurrentUser = userId;
            ViewBag.Logins = timestampList.OrderByDescending(x => x);
            return View();

        }
    }
}
