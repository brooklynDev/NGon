NGon - Easily send your variables to JavaScript
===============


----------


This project is a port of the Ruby Gon gem found here: [https://github.com/gazay/gon][1]

Usage:
------

The first thing you need to do when using NGon, is to add the NGonActionFilterAttribute to the global action filters:

**Global.asax.cs**

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalFilters.Filters.Add(new NGonActionFilterAttribute());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

Then, in your controller, you can add any value to the dynamic NGon property of the ViewBag:

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.NGon.SomeValue = 100;
            return View();
        }
    }

In your javascript, you then add this (you'll likely want to put this in your layout/master page file):

    @Html.IncludeNGon()

Finally, anywhere in your javascript, you now have access to that value:

    <script type="text/javascript">
        $(function () {
            $("#button").click(function () {
                alert(ngon.SomeValue);
            });
        }); </script>

Additional:
------

This doesn't only work on simple types, it also works on any POCO object that can be serialized using the default `JavascriptSerializer`:

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var person = new Person { FirstName = "John", LastName = "Doe", Age = 30 };
            ViewBag.NGon.Person = person;
            return View();
        }
    }

and in your javascript:

    <script type="text/javascript">
        $(function () {
            $("#button").click(function () {
                var person = ngon.Person;
                var div = $("#output");
                div.html('');
                div.append("FirstName: " + person.FirstName);
                div.append(", LastName: " + person.LastName);
                div.append(", Age: " + person.Age);
            });
        });
    </script>

Options:
------

When calling the `Html.IncludeNGon` method, there's an optional parameter for `@namespace`. What this does is that instead of referring to the variable in your javascript as `ngon.SomeValue', you can refer to it using whatever namespace prefix you like:

    @Html.IncludeNGon("test")

and then in your javascript:

    alert(test.SomeValue);

  [1]: https://github.com/gazay/gon