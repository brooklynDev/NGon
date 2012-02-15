using System;
using System.Dynamic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace NGon.Tests
{
    [TestFixture]
    public class NGonTests
    {
        private ControllerBase _controller;
        private HtmlHelper _helper;
        private const string StartScriptTagFormat = @"<script type=""text/javascript"">window.{0}={{}};";
        private const string EndScriptTag = @"</script>";

        [SetUp]
        public void Setup()
        {
            var controllerContext = new ControllerContext(new Mock<HttpContextBase>().Object, new RouteData(), new Mock<ControllerBase>().Object);
            controllerContext.Controller.ViewData = new ViewDataDictionary { { "NGon", new ExpandoObject() } };
            var viewContext = new ViewContext(controllerContext, new Mock<IView>().Object, controllerContext.Controller.ViewData, new TempDataDictionary(), new Mock<TextWriter>().Object);
            var mockViewDataContainer = new Mock<IViewDataContainer>();
            mockViewDataContainer.Setup(v => v.ViewData).Returns(controllerContext.Controller.ViewData);
            _controller = controllerContext.Controller;
            _helper = new HtmlHelper(viewContext, mockViewDataContainer.Object);
        }

        [Test]
        public void SimpleStringPropertyTest()
        {
            //arrange
            _controller.ViewBag.NGon.Foo = "100";

            //act
            var result = _helper.IncludeNGon();

            //assert
            var expected = String.Concat(String.Format(StartScriptTagFormat, "ngon"), @"ngon.Foo=""100"";", EndScriptTag);
            var actual = result.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SimpleIntPropertyTest()
        {
            //arrange
            _controller.ViewBag.NGon.Foo = 100;

            //act
            var result = _helper.IncludeNGon();

            //assert
            var expected = String.Concat(String.Format(StartScriptTagFormat, "ngon"), @"ngon.Foo=100;", EndScriptTag);
            var actual = result.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PocoObjectJsonTest()
        {
            var person = new { FirstName = "John", LastName = "Doe", Age = 45 };

            //arrange
            _controller.ViewBag.NGon.Person = person;

            //act
            var result = _helper.IncludeNGon();

            //assert
            var expected = String.Concat(String.Format(StartScriptTagFormat, "ngon"), @"ngon.Person={""FirstName"":""John"",""LastName"":""Doe"",""Age"":45};", EndScriptTag);
            var actual = result.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NamespaceTest()
        {
            const string @namespace = "test";
            //arrange
            _controller.ViewBag.NGon.Foo = 100;

            //act
            var result = _helper.IncludeNGon(@namespace);

            //assert
            var expected = String.Concat(String.Format(StartScriptTagFormat, @namespace), @namespace, @".Foo=100;", EndScriptTag);
            var actual = result.ToString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MissingNGonInViewBagThrowsException()
        {
            //arrange
            _controller.ViewData.Clear();

            //act
            //assert
            Assert.Throws<InvalidOperationException>(() => _helper.IncludeNGon());
        }
    }
}
