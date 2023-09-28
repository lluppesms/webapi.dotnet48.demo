//-----------------------------------------------------------------------
// <copyright file="RoomController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Room View Controller
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
	/// Room View Controller
	/// </summary>
	public class RoomController : _BaseController
    {
        #region Initialization
        //// Dependency Injection for unit testing
        //// Unit tests can create their own context and pass it in so you can test without real database
        //// If a Repository class is not passed in, then create one against the database

        /// <summary>
        /// Room Repository
        /// </summary>
        public ITbl_DimRoomRepository Tbl_DimRoomDb { get; private set; }

        /////// <summary>
        /////// Room Service
        /////// </summary>
        ////public ITbl_DimRoomService Tbl_DimRoomDb { get; private set; }

        /// <summary>
        /// Room Controller
        /// </summary>
        public RoomController() : this(new Tbl_DimRoomRepository())
        {
        }

        /// <summary>
        /// Room View Controller
        /// </summary>
        /// <param name="repositoryTbl_DimRoom">Tbl_DimRoom Repository</param>
        public RoomController(ITbl_DimRoomRepository repositoryTbl_DimRoom)
        {
            Tbl_DimRoomDb = repositoryTbl_DimRoom;
        }
        #endregion

        /// <summary>
        /// Index View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Room List";
            return View(Tbl_DimRoomDb.FindAll(GetUserName()));
        }

        /// <summary>
        /// Detail View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Details(int id)
        {
            ViewBag.Title = "Room Details";
            var tbl_DimRoom = Tbl_DimRoomDb.FindOne(GetUserName(), id);
            if (tbl_DimRoom == null)
            {
                 return HttpNotFound("No Room found for id " + id.ToString());
            }
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            ViewBag.Title = "Create Room";
            var tbl_DimRoom = new Tbl_DimRoom();
            FetchLookupTables(tbl_DimRoom);
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Create View
        /// </summary>
        /// <param name="tbl_DimRoom">Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomID,OfficeID,RoomName")] Tbl_DimRoom tbl_DimRoom)
        {
            var errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                var dupField = string.Empty;
                var dupMessage = string.Empty;
                if (Tbl_DimRoomDb.DupCheck(tbl_DimRoom.RoomID, tbl_DimRoom.RoomName, ref dupField, ref dupMessage))
                {
                    ModelState.AddModelError(dupField, dupMessage);
                    errorMessage = dupField + ": " + dupMessage;
                }
                else
                {
                    if (Tbl_DimRoomDb.Add(GetUserName(), tbl_DimRoom))
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Create Room";
            ViewBag.Message = errorMessage;
            FetchLookupTables(tbl_DimRoom);
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Edit Room";
            var tbl_DimRoom = Tbl_DimRoomDb.FindOne(GetUserName(), id);
            if (tbl_DimRoom == null)
            {
                 return HttpNotFound("No Room found for id " + id.ToString());
            }
            FetchLookupTables(tbl_DimRoom);
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Edit View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_DimRoom">Tbl_DimRoom Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomID,OfficeID,RoomName")] int id, Tbl_DimRoom tbl_DimRoom)
        {
            var errorMessage = string.Empty;
            if (ModelState.IsValid && Tbl_DimRoomDb.Save(GetUserName(), id, tbl_DimRoom))
            {
                // ReSharper disable once RedundantAnonymousTypePropertyName
                return RedirectToAction("Details", new { id });
            }

            // Invalid: redisplay with errors
            ViewBag.Title = "Edit Room";
            ViewBag.Message = (errorMessage != string.Empty) ? errorMessage : "Edit Failed!";
            FetchLookupTables(tbl_DimRoom);
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>View</returns>
        public ActionResult Delete(int id)
        {
            ViewBag.Title = "Delete Room";
            var tbl_DimRoom = Tbl_DimRoomDb.FindOne(GetUserName(), id);
            if (tbl_DimRoom == null)
            {
                 return HttpNotFound("No Room found for id " + id.ToString());
            }

            var deleteMessage = string.Empty;
            if (Tbl_DimRoomDb.DeleteCheck(GetUserName(), id, ref deleteMessage))
            {
                ViewBag.Message = deleteMessage;
            }

            FetchLookupTables(tbl_DimRoom);
            return View(tbl_DimRoom);
        }

        /// <summary>
        /// Delete View
        /// </summary>
        /// <param name="id">Key</param>
        /// <param name="tbl_DimRoom">Tbl_DimRoom Object</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tbl_DimRoom tbl_DimRoom)
        {
            Tbl_DimRoomDb.Delete(GetUserName(), id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fetch Lookup Tables
        /// </summary>
        /// <param name="tbl_DimRoom">Record</param>
        private void FetchLookupTables(Tbl_DimRoom tbl_DimRoom)
        {
            if (tbl_DimRoom == null)
            {
                ////ViewBag.SomeField = string.Empty;

            }
            else
            {
                ////ViewBag.SomeField = tbl_DimRoom.SomeField;

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
                Tbl_DimRoomDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}