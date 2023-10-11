//-----------------------------------------------------------------------
// <copyright file="Tbl_DimRoomRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimRoom Repository
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimRoom Repository
    /// </summary>
    public class Tbl_DimRoomRepository : _BaseRepository, ITbl_DimRoomRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        public Tbl_DimRoomRepository()
        {
            db = new DatabaseEntities();
            //SetupRetryPolicy();
        }

        /// <summary>
        /// Initialization of this repository class with injected database context.
        /// </summary>
        /// <param name="context">The database context</param>
        public Tbl_DimRoomRepository(DatabaseEntities context)
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
        public Tbl_DimRoomGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<Tbl_DimRoom> tbl_DimRooms;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        tbl_DimRooms = db.Tbl_DimRoom
                          .OrderBy(c => c.RoomName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_DimRoom.Count();
                    }
                    else
                    {
                        tbl_DimRooms = db.Tbl_DimRoom
                          .Where(c => c.RoomName.Contains(searchTxt))
                          .OrderBy(c => c.RoomName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.Tbl_DimRoom.Count(c => c.RoomName.Contains(searchTxt));
                    }
                }
                else
                {
                    tbl_DimRooms = db.Tbl_DimRoom.ToList();
                    count = db.Tbl_DimRoom.Count();
                }
                var results = new Tbl_DimRoomGrid(tbl_DimRooms, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new Tbl_DimRoomGrid
                {
                    Data = new List<Tbl_DimRoom>(),
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
        public IQueryable<Tbl_DimRoom> FindAll(string requestingUserName)
        {
            return from p in db.Tbl_DimRoom select p;
        }

		///// <summary>
		///// Find All Rooms for one Office
		///// </summary>
		///// <param name="requestingUserName">Requesting UserName</param>
		///// <param name="officeId">Office Id</param>
		///// <returns>Records</returns>
		//public IQueryable<Tbl_DimRoom> FindAllForOneOffice(string requestingUserName, int officeId)
		//{
		//	return from p in db.Tbl_DimRoom where p.OfficeID == officeId select p;
		//}

        /// <summary>
		/// Find One Records
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <param name="id">id</param>
		/// <returns>Records</returns>
		public Tbl_DimRoom FindOne(string requestingUserName, int id)
        {
            return db.Tbl_DimRoom.FirstOrDefault(u => u.RoomID == id);
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
            if (db.Tbl_DimRoom.Any(a => a.RoomID == keyValue))
            {
                fieldName = "RoomID";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.Tbl_DimRoom.Any(a => a.RoomName == dscr))
                {
                    return false;
                }
                fieldName = "RoomName";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimRoom">Object</param>
        /// <returns>Success</returns>
        public bool Add(string requestingUserName, Tbl_DimRoom tbl_DimRoom)
        {






            db.Tbl_DimRoom.Add(tbl_DimRoom);
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
            //// if (db.Tbl_DimRoom_Related_Table.Any(a => a.RoomID == id))
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
            var tbl_DimRoom = FindOne(requestingUserName, id);
            db.Tbl_DimRoom.Remove(tbl_DimRoom);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimRoom">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, Tbl_DimRoom tbl_DimRoom)
        {



            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="tbl_DimRoom">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, Tbl_DimRoom tbl_DimRoom)
        {
            var originalTbl_DimRoom = FindOne(requestingUserName, id);
            if (originalTbl_DimRoom == null)
            {
                return false;
            }

            originalTbl_DimRoom.OfficeID = tbl_DimRoom.OfficeID;
            originalTbl_DimRoom.RoomName = tbl_DimRoom.RoomName;




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
