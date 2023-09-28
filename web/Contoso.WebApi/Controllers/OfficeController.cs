//-----------------------------------------------------------------------
// <copyright file="OfficeController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Office View Controller
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System.Web.Mvc;

namespace Contoso.WebApi.Controllers
{
	[Authorize]
	//[AllowAnonymous]

	/// <summary>
	/// Office View Controller
	/// </summary>
	public class OfficeController : _BaseController
    {
        #region Initialization
        //// Dependency Injection for unit testing
        //// Unit tests can create their own context and pass it in so you can test without real database
        //// If a Repository class is not passed in, then create one against the database

        /// <summary>
        /// Office Repository
        /// </summary>
        public ITbl_DimOfficeRepository Tbl_DimOfficeDb { get; private set; }

        /////// <summary>
        /////// Office Service
        /////// </summary>
        ////public ITbl_DimOfficeService Tbl_DimOfficeDb { get; private set; }

        /// <summary>
        /// Office Controller
        /// </summary>
        public OfficeController() : this(new Tbl_DimOfficeRepository())
        {
        }

        /// <summary>
        /// Office View Controller
        /// </summary>
        /// <param name="repositoryTbl_DimOffice">Tbl_DimOffice Repository</param>
        public OfficeController(ITbl_DimOfficeRepository repositoryTbl_DimOffice)
        {
            Tbl_DimOfficeDb = repositoryTbl_DimOffice;
        }
        #endregion

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Office List";
            return View(Tbl_DimOfficeDb.FindAll(GetUserName()));
        }

        /// <summary>
        /// Detail View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Office Details";
            var tbl_DimOffice = Tbl_DimOfficeDb.FindOne(GetUserName(), id);
            if (tbl_DimOffice == null)
            {
                 return HttpNotFound("No Office found for id " + id.ToString());
            }
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            ViewBag.Title = "Create Office";
            var tbl_DimOffice = new Tbl_DimOffice();
            FetchLookupTables(tbl_DimOffice);
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OfficeID,OfficeName,OfficeAddress,OfficeCity,OfficeState,OfficeZip,OfficeCountry")] Tbl_DimOffice tbl_DimOffice)
        {
            var errorMessage = string.Empty;

            if (ModelState.IsValid)
            {
                var dupField = string.Empty;
                var dupMessage = string.Empty;
                if (Tbl_DimOfficeDb.DupCheck(tbl_DimOffice.OfficeID, tbl_DimOffice.OfficeName, ref dupField, ref dupMessage))
                {
                    ModelState.AddModelError(dupField, dupMessage);
                    errorMessage = dupField + ": " + dupMessage;
                }
                else
                {
                    if (Tbl_DimOfficeDb.Add(GetUserName(), tbl_DimOffice))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Create Office";
            ViewBag.Message = errorMessage;
            FetchLookupTables(tbl_DimOffice);
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Office";
            var tbl_DimOffice = Tbl_DimOfficeDb.FindOne(GetUserName(), id);
            if (tbl_DimOffice == null)
            {
                 return HttpNotFound("No Office found for id " + id.ToString());
            }
            FetchLookupTables(tbl_DimOffice);
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_DimOffice">Tbl_DimOffice Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OfficeID,OfficeName,OfficeAddress,OfficeCity,OfficeState,OfficeZip,OfficeCountry")] int id, Tbl_DimOffice tbl_DimOffice)
        {
            var errorMessage = string.Empty;

            if (ModelState.IsValid && Tbl_DimOfficeDb.Save(GetUserName(), id, tbl_DimOffice))
            {
                // ReSharper disable once RedundantAnonymousTypePropertyName
                return RedirectToAction("Details", new { id });
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Edit Office";
            ViewBag.Message = (errorMessage != string.Empty) ? errorMessage : "Edit Failed!";
            FetchLookupTables(tbl_DimOffice);
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Delete(int id)
        {
            ViewBag.Title = "Delete Office";
            var tbl_DimOffice = Tbl_DimOfficeDb.FindOne(GetUserName(), id);
            if (tbl_DimOffice == null)
            {
                 return HttpNotFound("No Office found for id " + id.ToString());
            }

            var deleteMessage = string.Empty;
            if (Tbl_DimOfficeDb.DeleteCheck(GetUserName(), id, ref deleteMessage))
            {
                ViewBag.Message = deleteMessage;
            }

            FetchLookupTables(tbl_DimOffice);
            return View(tbl_DimOffice);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_DimOffice">Tbl_DimOffice Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tbl_DimOffice tbl_DimOffice)
        {
            Tbl_DimOfficeDb.Delete(GetUserName(), id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fetch Lookup Tables
        /// </summary>
        /// <param name="tbl_DimOffice">Record</param>
        private void FetchLookupTables(Tbl_DimOffice tbl_DimOffice)
        {
            if (tbl_DimOffice == null)
            {
                ////ViewBag.SomeField = string.Empty;

            }
            else
            {
                ////ViewBag.SomeField = tbl_DimOffice.SomeField;

            }

        }

        /// <summary>
        /// Dispose of objects
        /// </summary>
        /// <param name="disposing">Disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Tbl_DimOfficeDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}