using Contoso.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Contoso.WebApi.UnitTests.Views
{
	[TestClass]
	public class View_Home_Tests
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			//Assert.AreEqual("Home Page", result.ViewBag.Title);
		}
	}
}
