using System.Web.Mvc;

namespace NGon.SampleApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var person = new Person { FirstName = "John", LastName = "Doe", Age = 30 };
            ViewBag.NGon.Person = person;
            return View();
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
