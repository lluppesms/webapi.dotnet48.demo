//-----------------------------------------------------------------------
// <copyright file="Tbl_DimOfficeRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimOffice Repository
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimOffice Repository
    /// </summary>
    public class Tbl_DimOfficeRepository : _BaseRepository, ITbl_DimOfficeRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        public Tbl_DimOfficeRepository()
        {
            db = new DatabaseEntities();
            //SetupRetryPolicy();
        }

        /// <summary>
        /// Initialization of this repository class with injected database context.
        /// </summary>
        /// <param name="context">The database context</param>
        public Tbl_DimOfficeRepository(DatabaseEntities context)
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
        public Tbl_DimOfficeGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<Tbl_DimOffice> tbl_DimOffices;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        tbl_DimOffices = db.Tbl_DimOffice
                          .OrderBy(c => c.OfficeName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_DimOffice.Count();
                    }
                    else
                    {
                        tbl_DimOffices = db.Tbl_DimOffice
                          .Where(c => c.OfficeName.Contains(searchTxt))
                          .OrderBy(c => c.OfficeName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_DimOffice.Count(c => c.OfficeName.Contains(searchTxt));
                    }
                }
                else
                {
                    tbl_DimOffices = db.Tbl_DimOffice.ToList();
                    count = db.Tbl_DimOffice.Count();
                }
                var results = new Tbl_DimOfficeGrid(tbl_DimOffices, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new Tbl_DimOfficeGrid
                {
                    Data = new List<Tbl_DimOffice>(),
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
        public IQueryable<Tbl_DimOffice> FindAll(string requestingUserName)
        {
            return from p in db.Tbl_DimOffice select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public Tbl_DimOffice FindOne(string requestingUserName, int id)
        {
            return db.Tbl_DimOffice.FirstOrDefault(u => u.OfficeID == id);
        }

		/// <summary>
		/// Find One Records
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <param name="id">id</param>
		/// <returns>Records</returns>
		public OfficeDetailsVM FindOneDetails(string requestingUserName, int id)
		{
			var office = db.Tbl_DimOffice.FirstOrDefault(u => u.OfficeID == id);
            var rooms = db.Tbl_DimRoom.Where(r => r.OfficeID == id).Select(r => r.RoomID).ToList();
            return new OfficeDetailsVM(office, rooms);
		}

		/// <summary>
		/// Find One Office Rooms
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <param name="id">id</param>
		/// <returns>Records</returns>
		public List<Tbl_DimRoom> FindOneRooms(string requestingUserName, int id)
		{
			return db.Tbl_DimRoom.Where(r => r.OfficeID == id).ToList();
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
            if (db.Tbl_DimOffice.Any(a => a.OfficeID == keyValue))
            {
                fieldName = "OfficeID";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.Tbl_DimOffice.Any(a => a.OfficeName == dscr))
                {
                    return false;
                }
                fieldName = "OfficeName";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        public bool Add(string requestingUserName, Tbl_DimOffice tbl_DimOffice)
        {






            db.Tbl_DimOffice.Add(tbl_DimOffice);
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
            //// if (db.Tbl_DimOffice_Related_Table.Any(a => a.OfficeID == id))
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
            var tbl_DimOffice = FindOne(requestingUserName, id);
            db.Tbl_DimOffice.Remove(tbl_DimOffice);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, Tbl_DimOffice tbl_DimOffice)
        {



            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, Tbl_DimOffice tbl_DimOffice)
        {
            var originalTbl_DimOffice = FindOne(requestingUserName, id);
            if (originalTbl_DimOffice == null)
            {
                return false;
            }

            originalTbl_DimOffice.OfficeName = tbl_DimOffice.OfficeName;
            originalTbl_DimOffice.OfficeAddress = tbl_DimOffice.OfficeAddress;
            originalTbl_DimOffice.OfficeCity = tbl_DimOffice.OfficeCity;
            originalTbl_DimOffice.OfficeState = tbl_DimOffice.OfficeState;
            originalTbl_DimOffice.OfficeZip = tbl_DimOffice.OfficeZip;
            originalTbl_DimOffice.OfficeCountry = tbl_DimOffice.OfficeCountry;




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
