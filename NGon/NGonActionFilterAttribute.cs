using System.Dynamic;
using System.Web.Mvc;

namespace NGon
{
    public class NGonActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.NGon = new ExpandoObject();
            base.OnActionExecuting(filterContext);
        }
    }
}
