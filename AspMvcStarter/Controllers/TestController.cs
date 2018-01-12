using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspMvcStarter.Controllers
{
    public class TestController : Controller
    {
        FashionContext database = new FashionContext();
        
        public ActionResult Index()
        {

            var featuredpostlist = database.FeaturedPosts.ToList();
            return View(featuredpostlist);
        }
    }
}