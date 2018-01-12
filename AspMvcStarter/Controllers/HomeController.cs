using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspMvcStarter.Models;

namespace AspMvcStarter.Controllers
{
    public class HomeController : Controller
    {

        FashionContext database = new FashionContext();
        // GET: Home
        public ActionResult Index()
        {
            if (Request.Cookies["username"] != null)
            {
                ViewBag.Username = Request.Cookies["username"].Value;
            }
            var featuredpostlist = database.Photos.ToList().OrderBy(a => Guid.NewGuid()).Take(10);

            return View(featuredpostlist);
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string username , string location)
        {
            List<User> result;
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(location))
            {
                ViewBag.Username = username;
                ViewBag.Location = location;
                result = database.Users.Where(a => (a.Username.Contains(username.Trim()) || a.Name.Contains(username.Trim())) && a.Location.Contains(location.Trim()) && a.Tailor == 1).ToList();
            }else if ( !string.IsNullOrWhiteSpace(location) )
            {
                ViewBag.Location = location;
                result = database.Users.Where(a => a.Location.Contains(location.Trim()) && a.Tailor == 1).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(username) )
            {
                ViewBag.Username = username;
                result = database.Users.Where(a => a.Username.Contains(username.Trim()) || a.Name.Contains(username.Trim()) && a.Tailor == 1).ToList();
            }
            else
            {
                result = database.Users.Where(a=>a.Tailor==1).ToList();
            }

            return View(result);
        }
    }
}