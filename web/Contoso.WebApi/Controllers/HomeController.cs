//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
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