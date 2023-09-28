//-----------------------------------------------------------------------
// <copyright file="SampleData_Tbl_DimOffice.cs" company="Contoso, Inc.">
//   Copyright © 2017 Contoso, Inc.
// </copyright>
// <summary>
// Sample Data for Tbl_DimOffice
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contoso.WebApi.SampleData
{
	/// <summary>
	/// Sample Data
	/// </summary>
	public static partial class SampleDataManager
    {
        /// <summary>
        /// Creates Sample Data for Tbl_DimOffice
        /// </summary>
        /// <param name="DatabaseEntities">The database context.</param>
        private static void Create_Tbl_DimOffice_Data(DatabaseEntities DatabaseEntities = null)
        {
            if (Test_Tbl_DimOffice.Count <= 0)
            {
				// (OfficeName, OfficeAddress, OfficeCity, OfficeState, OfficeZip, OfficeCountry)
				Test_Tbl_DimOffice.AddRange(new List<Tbl_DimOffice>
                {
                    new Tbl_DimOffice() { OfficeID = 1, OfficeName = "Office 1", OfficeAddress = "100 Main", OfficeCity = "MyTown", OfficeState = "MN", OfficeCountry = "USA", OfficeZip = "55124" },
                    new Tbl_DimOffice() { OfficeID = 2, OfficeName = "Office 2", OfficeAddress = "100 Main", OfficeCity = "MyTown", OfficeState = "MN", OfficeCountry = "USA", OfficeZip = "55124" }
                });
            }

            if (DatabaseEntities != null && !DatabaseEntities.Tbl_DimOffice.Any())
            {
                DatabaseEntities.Tbl_DimOffice.AddRange(Test_Tbl_DimOffice);
            }
        }
    }
}
