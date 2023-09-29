//-----------------------------------------------------------------------
// <copyright file="DiagnosticsController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Diagnostics View Controller
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Contoso.WebApi.Controllers
{
	[Authorize]
	//[AllowAnonymous]
	public class DiagnosticsController : Controller
	{
		public ActionResult Index()
		{
			var isAdmin = IsAdmin();
			ViewBag.IsAdmin = isAdmin;
			ViewBag.UserName = GetUserName();

			var assmbly = Assembly.GetExecutingAssembly();
			var fileInfo = new System.IO.FileInfo(assmbly.Location);
			ViewBag.VersionNumber = assmbly.GetName().Version.ToString();

			if (isAdmin)
			{
				ViewBag.VersionDateUTC = fileInfo.LastWriteTimeUtc.ToString("yyyy-MM-ddTHH:mm:ssZ");

				var connectString = ConfigurationManager.ConnectionStrings["DatabaseEntities"].ConnectionString;
				var userLoc = connectString.IndexOf("User", StringComparison.Ordinal);
				if (userLoc > 0)
				{
					connectString = connectString.Substring(0, userLoc) + "...";
				}
				ViewBag.Connection = connectString;
			}

			var buildInfoFile = Server.MapPath("\\buildinfo.json");
			ViewBag.BuildInfoFile = buildInfoFile;
			if (System.IO.File.Exists(buildInfoFile))
			{
				using (var r = new StreamReader(buildInfoFile))
				{
					var buildInfoData = r.ReadToEnd();
					var buildInfoObject = JsonConvert.DeserializeObject<BuildInfo>(buildInfoData);
					ViewBag.BuildNumber = buildInfoObject.BuildNumber;
					ViewBag.BuildDate = buildInfoObject.BuildDate;
				}
			}

			//ViewBag.AdTenantId = ConfigurationManager.AppSettings["AzureAD__TenantId"];
			//ViewBag.AdClientId = ConfigurationManager.AppSettings["AzureAD__ClientId"];

			var adTenantId = ConfigurationManager.AppSettings["AzureAD__TenantId"];
			ViewBag.AdTenantId = adTenantId != null && adTenantId.Length > 6 ? adTenantId.Substring(0, 5) + "..." : adTenantId;
			var adClientId = ConfigurationManager.AppSettings["AzureAD__ClientId"];
			ViewBag.AdClientId = adClientId != null && adClientId.Length > 6 ? adClientId.Substring(0, 5) + "..." : adClientId;

			ViewBag.AdInstance = ConfigurationManager.AppSettings["AzureAD__AADInstance"];
			ViewBag.AdDomain = ConfigurationManager.AppSettings["AzureAD__Domain"];
			ViewBag.AdLogoutUri= ConfigurationManager.AppSettings["AzureAD__PostLogoutRedirectUri"];

			return View();
		}

		/// <summary>
		/// Returns User Name if logged on, UNKNOWN if not
		/// </summary>
		/// <returns>User Name</returns>
		protected string GetUserName()
		{
			var currentUser = HttpContext.GetOwinContext().Authentication.User;
			return currentUser != null ? currentUser.Identity.Name : "UNKNOWN";
		}

		/// <summary>
		/// Returns true if user is in Admin Role, false if not or not logged in
		/// </summary>
		/// <returns>Is Admin</returns>
		private bool IsAdmin()
		{
			var currentUser = HttpContext.GetOwinContext().Authentication.User;
			return currentUser != null ? currentUser.IsInRole("Admin") :false;
		}
	}
}