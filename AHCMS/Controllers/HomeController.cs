using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AHCMS.Core.Repository;
using AHCMS.Models;

namespace AHCMS.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (repository.Query<ContentType>().Where(x => x.Name == "test").Count() <= 0)
            {
                ContentType t = new ContentType();
                t.Name = "test";
                t.Description = "测试用的";
                repository.Save(t);
            }
            else
            {
                ViewBag.description = repository.Query<ContentType>().First().Description;
            }
            return View();
        }

    }
}
