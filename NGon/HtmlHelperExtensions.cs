using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using System.Linq;

namespace NGon
{
    public static class HtmlHelperExtensions
    {
        private const string ExternalJSRoute = "ngon.js";
        private static void AddExternalJsFileRoute(HtmlHelper helper, string javascript)
        {
            var dynamicNGonRoute = new Route(ExternalJSRoute, new RouteValueDictionary(new { controller = "DynamicNGonJavascript", action = "NGon" }), new MvcRouteHandler());
            if (!helper.RouteCollection.Cast<Route>().Any(r => r.Url == ExternalJSRoute))
            {
                helper.RouteCollection.Insert(0, dynamicNGonRoute);
            }
            else
            {
                dynamicNGonRoute = helper.RouteCollection.Cast<Route>().First(r => r.Url == ExternalJSRoute);
                dynamicNGonRoute.Defaults.Remove("data");
            }

            dynamicNGonRoute.Defaults.Add("data", javascript);
        }

        private static string GetOutputJavascript(HtmlHelper helper, dynamic ngon, string @namespace)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("window.{0}={{}};", @namespace);

            foreach (var prop in ngon)
            {
                builder.AppendFormat("{0}.{1}={2};", @namespace, prop.Key, helper.Raw(JsonConvert.SerializeObject(prop.Value)));
            }

            return builder.ToString();
        }

        public static IHtmlString IncludeNGon(this HtmlHelper helper, string @namespace = "ngon", bool useExternalJSFile = false, bool outputScriptTag = true)
        {
            var viewData = helper.ViewContext.ViewData;
            if (viewData == null)
            {
                return MvcHtmlString.Empty;
            }

            var ngon = viewData["NGon"] as ExpandoObject;
            if (ngon == null)
            {
                throw new InvalidOperationException("Cannot find NGon in ViewBag. Did you remember to add the global NGonActionFilterAttribute?");
            }

            var outputJavascript = GetOutputJavascript(helper, ngon, @namespace);
            if (!outputScriptTag)
            {
                return new HtmlString(outputJavascript);
            }

            var tag = new TagBuilder("script");
            tag.Attributes.Add(new KeyValuePair<string, string>("type", "text/javascript"));

            if (useExternalJSFile)
            {
                AddExternalJsFileRoute(helper, outputJavascript);
                tag.Attributes.Add("src", ExternalJSRoute + "?r=" + DateTime.Now.Ticks);
            }
            else
            {
                tag.InnerHtml = outputJavascript;
            }

            return new HtmlString(tag.ToString());
        }
    }
}
