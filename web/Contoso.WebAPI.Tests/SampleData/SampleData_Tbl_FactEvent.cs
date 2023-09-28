//-----------------------------------------------------------------------
// <copyright file="SampleData_Tbl_FactEvent.cs" company="Contoso, Inc.">
//   Copyright © 2017 Contoso, Inc.
// </copyright>
// <summary>
// Sample Data for Tbl_FactEvent
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
		/// Creates Sample Data for Tbl_FactEvent
		/// </summary>
		/// <param name="DatabaseEntities">The database context.</param>
		private static void Create_Tbl_FactEvent_Data(DatabaseEntities DatabaseEntities = null)
		{
			if (Test_Tbl_FactEvent.Count <= 0)
			{
				// (RoomID, EventName, EventOwner, EventStartDateTime, EventEndDateTime)
				Test_Tbl_FactEvent.AddRange(new List<Tbl_FactEvent>
				{
					new Tbl_FactEvent() { EventID = 1, RoomID = 1, EventName = "Event 1", EventOwner = "User 1", EventStartDateTime = Convert.ToDateTime("09/01/2023 08:00"), EventEndDateTime= Convert.ToDateTime("09/01/2023 08:30") },
					new Tbl_FactEvent() { EventID = 2, RoomID = 1, EventName = "Event 2", EventOwner = "User 2", EventStartDateTime = Convert.ToDateTime("09/02/2023 09:00"), EventEndDateTime= Convert.ToDateTime("09/02/2023 09:30") },
					new Tbl_FactEvent() { EventID = 3, RoomID = 2, EventName = "Event 3", EventOwner = "User 3", EventStartDateTime = Convert.ToDateTime("09/03/2023 10:00"), EventEndDateTime= Convert.ToDateTime("09/03/2023 10:30") },
					new Tbl_FactEvent() { EventID = 4, RoomID = 2, EventName = "Event 4", EventOwner = "User 4", EventStartDateTime = Convert.ToDateTime("09/04/2023 11:00"), EventEndDateTime= Convert.ToDateTime("09/04/2023 11:30") },
					new Tbl_FactEvent() { EventID = 5, RoomID = 3, EventName = "Event 5", EventOwner = "User 5", EventStartDateTime = Convert.ToDateTime("09/05/2023 12:00"), EventEndDateTime= Convert.ToDateTime("09/05/2023 12:30") },
					new Tbl_FactEvent() { EventID = 6, RoomID = 3, EventName = "Event 6", EventOwner = "User 6", EventStartDateTime = Convert.ToDateTime("09/06/2023 13:00"), EventEndDateTime= Convert.ToDateTime("09/06/2023 13:30") },
					new Tbl_FactEvent() { EventID = 7, RoomID = 4, EventName = "Event 7", EventOwner = "User 7", EventStartDateTime = Convert.ToDateTime("09/07/2023 14:00"), EventEndDateTime= Convert.ToDateTime("09/07/2023 14:30") },
					new Tbl_FactEvent() { EventID = 8, RoomID = 5, EventName = "Event 8", EventOwner = "User 8", EventStartDateTime = Convert.ToDateTime("09/08/2023 15:00"), EventEndDateTime= Convert.ToDateTime("09/08/2023 15:30")  }
				});
			}

			if (DatabaseEntities != null && !DatabaseEntities.Tbl_FactEvent.Any())
			{
				DatabaseEntities.Tbl_FactEvent.AddRange(Test_Tbl_FactEvent);
			}
		}
	}
}
