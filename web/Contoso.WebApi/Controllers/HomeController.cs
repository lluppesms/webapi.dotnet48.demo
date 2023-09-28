//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Home View Controller
// </summary>
//-----------------------------------------------------------------------

using System.Web.Mvc;

namespace Contoso.WebApi.Controllers
{
	//[Authorize]
	[AllowAnonymous]

	/// <summary>
	/// Home Controller
	/// </summary>
	public class HomeController : Controller
    {
		/// <summary>
		/// Index View
		/// </summary>
		public ActionResult Index()
        {
            return View();
        }
    }
}