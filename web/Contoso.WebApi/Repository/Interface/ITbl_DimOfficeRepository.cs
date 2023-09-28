//-----------------------------------------------------------------------
// <copyright file="ITbl_DimOfficeRepository.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimOffice Interface
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimOffice Interface
    /// </summary>
    public interface ITbl_DimOfficeRepository
    {
        /// <summary>
        /// Find All Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <returns>Records</returns>
        IQueryable<Tbl_DimOffice> FindAll(string requestingUserName);

        /// <summary>
        /// Find Paged Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="searchTxt">Search Text</param>
        /// <param name="skipNbr">Skip</param>
        /// <param name="takeNbr">Take</param>
        /// <returns>Records</returns>
        Tbl_DimOfficeGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr);

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        Tbl_DimOffice FindOne(string requestingUserName, int id);

		/// <summary>
		/// Find One Details
		/// </summary>
		/// <param name="requestingUserName">Requesting UserName</param>
		/// <param name="id">id</param>
		/// <returns>Records</returns>
		OfficeDetailsVM FindOneDetails(string requestingUserName, int id);

        /// <summary>
        /// Find One Office Rooms
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        List<Tbl_DimRoom> FindOneRooms(string requestingUserName, int id);

		/// <summary>
		/// Duplicate Record Check
		/// </summary>
		/// <param name="keyValue">Key Value</param>
		/// <param name="dscr">Description</param>
		/// <param name="fieldName">Field Name</param>
		/// <param name="errorMessage">Message</param>
		/// <returns>Success</returns>
		bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage);

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        bool Add(string requestingUserName, Tbl_DimOffice tbl_DimOffice);

        /// <summary>
        /// Delete Check
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        bool DeleteCheck(string requestingUserName, int id, ref string errorMessage);

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <returns>Success</returns>
        bool Delete(string requestingUserName, int id);

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        bool Save(string requestingUserName, Tbl_DimOffice tbl_DimOffice);

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Id</param>
        /// <param name="tbl_DimOffice">Object</param>
        /// <returns>Success</returns>
        bool Save(string requestingUserName, int id, Tbl_DimOffice tbl_DimOffice);

#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        /// <summary>
        /// Disposal
        /// </summary>
        void Dispose();
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
    }
}
