//-----------------------------------------------------------------------
// <copyright file="EventController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Event View Controller
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Contoso.WebApi.Controllers
{
	[Authorize]
	//[AllowAnonymous]

	/// <summary>
	/// Event View Controller
	/// </summary>
	public class EventController : _BaseController
    {
        #region Initialization
        //// Dependency Injection for unit testing
        //// Unit tests can create their own context and pass it in so you can test without real database
        //// If a Repository class is not passed in, then create one against the database

        /// <summary>
        /// Event Repository
        /// </summary>
        public ITbl_FactEventRepository Tbl_FactEventDb { get; private set; }

        /////// <summary>
        /////// Event Service
        /////// </summary>
        ////public ITbl_FactEventService Tbl_FactEventDb { get; private set; }

        /// <summary>
        /// Event Controller
        /// </summary>
        public EventController() : this(new Tbl_FactEventRepository())
        {
        }

        /// <summary>
        /// Event View Controller
        /// </summary>
        /// <param name="repositoryTbl_FactEvent">Tbl_FactEvent Repository</param>
        public EventController(ITbl_FactEventRepository repositoryTbl_FactEvent)
        {
            Tbl_FactEventDb = repositoryTbl_FactEvent;
        }
        #endregion

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Event List";
            return View(Tbl_FactEventDb.FindAll(GetUserName()));
        }

        /// <summary>
        /// Detail View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Event Details";
            var tbl_FactEvent = Tbl_FactEventDb.FindOne(GetUserName(), id);
            if (tbl_FactEvent == null)
            {
                 return HttpNotFound("No Event found for id " + id.ToString());
            }
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            ViewBag.Title = "Create Event";
            var tbl_FactEvent = new Tbl_FactEvent();
            FetchLookupTables(tbl_FactEvent);
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <param name="tbl_FactEvent">Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,RoomID,EventName,EventOwner,EventStartDateTime,EventEndDateTime")] Tbl_FactEvent tbl_FactEvent)
        {
            var errorMessage = string.Empty;

            if (ModelState.IsValid)
            {
                var dupField = string.Empty;
                var dupMessage = string.Empty;
                if (Tbl_FactEventDb.DupCheck(tbl_FactEvent.EventID, tbl_FactEvent.EventName, ref dupField, ref dupMessage))
                {
                    ModelState.AddModelError(dupField, dupMessage);
                    errorMessage = dupField + ": " + dupMessage;
                }
                else
                {
                    if (Tbl_FactEventDb.Add(GetUserName(), tbl_FactEvent))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Create Event";
            ViewBag.Message = errorMessage;
            FetchLookupTables(tbl_FactEvent);
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Event";
            var tbl_FactEvent = Tbl_FactEventDb.FindOne(GetUserName(), id);
            if (tbl_FactEvent == null)
            {
                 return HttpNotFound("No Event found for id " + id.ToString());
            }
            FetchLookupTables(tbl_FactEvent);
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_FactEvent">Tbl_FactEvent Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,RoomID,EventName,EventOwner,EventStartDateTime,EventEndDateTime")] int id, Tbl_FactEvent tbl_FactEvent)
        {
            var errorMessage = string.Empty;

            if (ModelState.IsValid && Tbl_FactEventDb.Save(GetUserName(), id, tbl_FactEvent))
            {
                // ReSharper disable once RedundantAnonymousTypePropertyName
                return RedirectToAction("Details", new { id });
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Edit Event";
            ViewBag.Message = (errorMessage != string.Empty) ? errorMessage : "Edit Failed!";
            FetchLookupTables(tbl_FactEvent);
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Delete(int id)
        {
            ViewBag.Title = "Delete Event";
            var tbl_FactEvent = Tbl_FactEventDb.FindOne(GetUserName(), id);
            if (tbl_FactEvent == null)
            {
                 return HttpNotFound("No Event found for id " + id.ToString());
            }

            var deleteMessage = string.Empty;
            if (Tbl_FactEventDb.DeleteCheck(GetUserName(), id, ref deleteMessage))
            {
                ViewBag.Message = deleteMessage;
            }

            FetchLookupTables(tbl_FactEvent);
            return View(tbl_FactEvent);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_FactEvent">Tbl_FactEvent Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tbl_FactEvent tbl_FactEvent)
        {
            Tbl_FactEventDb.Delete(GetUserName(), id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fetch Lookup Tables
        /// </summary>
        /// <param name="tbl_FactEvent">Record</param>
        private void FetchLookupTables(Tbl_FactEvent tbl_FactEvent)
        {
            if (tbl_FactEvent == null)
            {
                ////ViewBag.SomeField = string.Empty;

            }
            else
            {
                ////ViewBag.SomeField = tbl_FactEvent.SomeField;

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
                Tbl_FactEventDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}