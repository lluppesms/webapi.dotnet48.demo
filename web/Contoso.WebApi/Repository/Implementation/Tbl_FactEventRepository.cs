//-----------------------------------------------------------------------
// <copyright file="Tbl_FactEventRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_FactEvent Repository
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_FactEvent Repository
    /// </summary>
    public class Tbl_FactEventRepository : _BaseRepository, ITbl_FactEventRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        public Tbl_FactEventRepository()
        {
            db = new DatabaseEntities();
            //SetupRetryPolicy();
        }

        /// <summary>
        /// Initialization of this repository class with injected database context.
        /// </summary>
        /// <param name="context">The database context</param>
        public Tbl_FactEventRepository(DatabaseEntities context)
        {
            db = context;
            //SetupRetryPolicy();
        }
        #endregion

        /// <summary>
        /// Find Paged Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="searchTxt">Search Text</param>
        /// <param name="skipNbr">Skip</param>
        /// <param name="takeNbr">Take</param>
        /// <returns>Records</returns>
        public Tbl_FactEventGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<Tbl_FactEvent> tbl_FactEvents;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        tbl_FactEvents = db.Tbl_FactEvent
                          .OrderBy(c => c.EventName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_FactEvent.Count();
                    }
                    else
                    {
                        tbl_FactEvents = db.Tbl_FactEvent
                          .Where(c => c.EventName.Contains(searchTxt))
                          .OrderBy(c => c.EventName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_FactEvent.Count(c => c.EventName.Contains(searchTxt));
                    }
                }
                else
                {
                    tbl_FactEvents = db.Tbl_FactEvent.ToList();
                    count = db.Tbl_FactEvent.Count();
                }
                var results = new Tbl_FactEventGrid(tbl_FactEvents, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new Tbl_FactEventGrid
                {
                    Data = new List<Tbl_FactEvent>(),
                    Total = 0,
                    ReturnMessageTxt = returnMessageTxt
                };
                return results;
            }
        }

        /// <summary>
        /// Find All Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <returns>Records</returns>
        public IQueryable<Tbl_FactEvent> FindAll(string requestingUserName)
        {
            return from p in db.Tbl_FactEvent select p;
        }

		/// <summary>
		/// Find All Events for One Room
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <returns>Records</returns>
		public IQueryable<Tbl_FactEvent> FindAllForOneRoom(string requestingUserName, int id)
		{
			return from p in db.Tbl_FactEvent where p.RoomID == id select p;
		}

        /// <summary>
		/// Find One Records
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <param name="id">id</param>
		/// <returns>Records</returns>
		public Tbl_FactEvent FindOne(string requestingUserName, int id)
        {
            return db.Tbl_FactEvent.FirstOrDefault(u => u.EventID == id);
        }

        /// <summary>
        /// Duplicate Record Check
        /// </summary>
        /// <param name="keyValue">Key Value</param>
        /// <param name="dscr">Description</param>
        /// <param name="fieldName">Field Name</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        public bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage)
        {
            if (db.Tbl_FactEvent.Any(a => a.EventID == keyValue))
            {
                fieldName = "EventID";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.Tbl_FactEvent.Any(a => a.EventName == dscr))
                {
                    return false;
                }
                fieldName = "EventName";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_FactEvent">Object</param>
        /// <returns>Success</returns>
        public bool Add(string requestingUserName, Tbl_FactEvent tbl_FactEvent)
        {






            db.Tbl_FactEvent.Add(tbl_FactEvent);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete Check
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        public bool DeleteCheck(string requestingUserName, int id, ref string errorMessage)
        {
            //// bool DeleteCheck = false;
            //// if (db.Tbl_FactEvent_Related_Table.Any(a => a.EventID == id))
            //// {
            ////     DeleteCheck = true;
            ////     errorMessage = "A related record with this key value exists! You cannot delete it!";
            //// }
            //// return DeleteCheck;
            return true;
        }

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <returns>Success</returns>
        public bool Delete(string requestingUserName, int id)
        {
            var tbl_FactEvent = FindOne(requestingUserName, id);
            db.Tbl_FactEvent.Remove(tbl_FactEvent);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_FactEvent">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, Tbl_FactEvent tbl_FactEvent)
        {



            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="tbl_FactEvent">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, Tbl_FactEvent tbl_FactEvent)
        {
            var originalTbl_FactEvent = FindOne(requestingUserName, id);
            if (originalTbl_FactEvent == null)
            {
                return false;
            }

            originalTbl_FactEvent.RoomID = tbl_FactEvent.RoomID;
            originalTbl_FactEvent.EventName = tbl_FactEvent.EventName;
            originalTbl_FactEvent.EventOwner = tbl_FactEvent.EventOwner;
            originalTbl_FactEvent.EventStartDateTime = tbl_FactEvent.EventStartDateTime;
            originalTbl_FactEvent.EventEndDateTime = tbl_FactEvent.EventEndDateTime;




            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Disposal
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
