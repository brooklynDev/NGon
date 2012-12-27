using System.Web.Mvc;

namespace NGon
{
    public class DynamicNGonJavascriptController : Controller
    {
        public ActionResult NGon(string data)
        {
            return JavaScript(data);
        }
    }
}