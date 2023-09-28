//-----------------------------------------------------------------------
// <copyright file="SampleData_Tbl_DimRoom.cs" company="Contoso, Inc.">
//   Copyright © 2017 Contoso, Inc.
// </copyright>
// <summary>
// Sample Data for Tbl_DimRoom
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
        /// Creates Sample Data for Tbl_DimRoom
        /// </summary>
        /// <param name="DatabaseEntities">The database context.</param>
        private static void Create_Tbl_DimRoom_Data(DatabaseEntities DatabaseEntities = null)
        {
            if (Test_Tbl_DimRoom.Count <= 0)
            {
				Test_Tbl_DimRoom.AddRange(new List<Tbl_DimRoom>
                {
                    new Tbl_DimRoom() { RoomID = 1, OfficeID = 1, RoomName = "Room 1" },
                    new Tbl_DimRoom() { RoomID = 2, OfficeID = 1, RoomName = "Room 2" },
					new Tbl_DimRoom() { RoomID = 3, OfficeID = 2, RoomName = "Room 3" },
					new Tbl_DimRoom() { RoomID = 4, OfficeID = 2, RoomName = "Room 4" }
				});
            }

            if (DatabaseEntities != null && !DatabaseEntities.Tbl_DimRoom.Any())
            {
                DatabaseEntities.Tbl_DimRoom.AddRange(Test_Tbl_DimRoom);
            }
        }
    }
}
