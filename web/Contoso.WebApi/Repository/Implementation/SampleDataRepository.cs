//-----------------------------------------------------------------------
// <copyright file="SampleDataRepository.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Sample Data Repository
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.WebApi.Repository.Implementation
{
    /// <summary>
    /// Repository for Sample Data
    /// </summary>
    public class SampleDataRepository : _BaseRepository, ISampleDataRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        public SampleDataRepository()
        {
            db = new DatabaseEntities();
            //SetupRetryPolicy();
        }

        /// <summary>
        /// Initialization of this repository class with injected database context.
        /// </summary>
        /// <param name="context">The database context</param>
        public SampleDataRepository(DatabaseEntities context)
        {
            db = context;
            //SetupRetryPolicy();
        }
        #endregion

        /// <summary>
        /// Populate Sample Data
        /// </summary>
        public async Task<(bool result, string message)> PopulateSampleData(string requestingUserName, bool forceReset = false)
        {
            var result = await Task.FromResult(true);
            var message = string.Empty;

            try
            {
                var dataExists = db.Tbl_DimOffice.Any();
                if (dataExists && !forceReset)
                {
                    message = "Data already exists and force reset was not specified!";
                    WriteToLog(message);
                    return (true, message);
                }
                if (dataExists && forceReset)
                {
                    WriteToLog("Data exists - removing it!!!");
                    db.Tbl_FactEvent.RemoveRange(db.Tbl_FactEvent);
                    db.SaveChanges();
                    db.Tbl_DimRoom.RemoveRange(db.Tbl_DimRoom);
                    db.SaveChanges();
                    db.Tbl_DimOffice.RemoveRange(db.Tbl_DimOffice);
                    db.SaveChanges();
                }

                //  INSERT INTO tbl_DimOffice (OfficeName, OfficeAddress, OfficeCity, OfficeState, OfficeZip, OfficeCountry)
                //  VALUES
                //      ('Microsoft - Las Colinas', '7100 State Hwy 161', 'Irving', 'TX', '75039', 'USA'),
                //      ('Microsoft - Redmond', 'Microsoft Building 92, NE 36th St', 'Redmond', 'WA', '98052', 'USA'),
                //      ('Microsoft - Chicago', '200 E Randolph St #200', 'Chicago', 'IL', '60601', 'USA');
                var officeRepo = new Tbl_DimOfficeRepository(db);
                var roomRepo = new Tbl_DimRoomRepository(db);
                var eventRepo = new Tbl_FactEventRepository(db);

                var officesAdded = 0;
                var roomsAdded = 0;
                var eventsAdded = 0;
                officeRepo.Add(requestingUserName, new Tbl_DimOffice
                {
                    OfficeName = "Microsoft - Las Colinas",
                    OfficeAddress = "7100 State Hwy 161",
                    OfficeCity = "Irving",
                    OfficeState = "TX",
                    OfficeZip = "75039",
                    OfficeCountry = "USA"
                });
                officeRepo.Add(requestingUserName, new Tbl_DimOffice
                {
                    OfficeName = "Microsoft - Redmond",
                    OfficeAddress = "Microsoft Building 92, NE 36th St",
                    OfficeCity = "Redmond",
                    OfficeState = "WA",
                    OfficeZip = "98052",
                    OfficeCountry = "USA"
                });
                officeRepo.Add(requestingUserName, new Tbl_DimOffice
                {
                    OfficeName = "Microsoft - Chicago",
                    OfficeAddress = "200 E Randolph St #200",
                    OfficeCity = "Chicago",
                    OfficeState = "IL",
                    OfficeZip = "60601",
                    OfficeCountry = "USA"
                });
                var offices = officeRepo.FindAll(requestingUserName).ToList();
                officesAdded = offices.Count;

                foreach (var office in offices)
                {
                    //INSERT INTO tbl_DimRoom(OfficeID, RoomName)
                    //  SELECT OfficeID, 'Conference Room' FROM tbl_DimOffice
                    //  SELECT OfficeID, 'Meeting Room 101'
                    //  SELECT OfficeID, 'Auditorium'
                    //  SELECT OfficeID, 'Break Room'
                    //  SELECT OfficeID, 'Executive Lounge'
                    roomRepo.Add(requestingUserName, new Tbl_DimRoom
                    {
                        OfficeID = office.OfficeID,
                        RoomName = "Conference Room"
                    });
                    roomRepo.Add(requestingUserName, new Tbl_DimRoom
                    {
                        OfficeID = office.OfficeID,
                        RoomName = "Meeting Room 101"
                    });
                    roomRepo.Add(requestingUserName, new Tbl_DimRoom
                    {
                        OfficeID = office.OfficeID,
                        RoomName = "Auditorium"
                    });
                    roomRepo.Add(requestingUserName, new Tbl_DimRoom
                    {
                        OfficeID = office.OfficeID,
                        RoomName = "Break Room"
                    });
                    roomRepo.Add(requestingUserName, new Tbl_DimRoom
                    {
                        OfficeID = office.OfficeID,
                        RoomName = "Executive Lounge"
                    });
                }
                var rooms = roomRepo.FindAll(requestingUserName).ToList();
                roomsAdded = rooms.Count;

                // INSERT INTO tbl_FactEvent(RoomID, EventName, EventOwner, EventStartDateTime, EventEndDateTime)
                //  VALUES
                //    (1, 'Product Launch Meeting', 'Jennifer Smith', '2023-10-01 09:00:00', '2023-10-01 11:00:00'),
                //    (2, 'Team Offsite Discussion', 'Brian Johnson', '2023-10-02 10:00:00', '2023-10-02 15:00:00'),
                //    (3, 'Financial Review', 'Charles Brown', '2023-10-03 13:00:00', '2023-10-03 15:00:00'),
                //    (4, 'HR Policy Refresh', 'Natalie Davis', '2023-10-04 15:00:00', '2023-10-04 17:00:00'),
                //    (5, 'Quarterly Sales Review', 'Erica Martinez', '2023-10-05 16:00:00', '2023-10-05 18:00:00'),
                //    (6, 'Engineering Sprint Retrospective', 'Derek Clark', '2023-10-06 09:00:00', '2023-10-06 10:30:00'),
                //    (7, 'Design Thinking Workshop', 'Angela Robinson', '2023-10-07 11:00:00', '2023-10-07 15:00:00'),
                //    (8, 'Customer Feedback Session', 'Oliver Wilson', '2023-10-08 14:00:00', '2023-10-08 16:30:00'),
                //    (9, 'Marketing Strategy Planning', 'Isabella Taylor', '2023-10-09 09:00:00', '2023-10-09 12:30:00'),
                //    (10, 'Leadership Training', 'Sophia Lee', '2023-10-10 12:00:00', '2023-10-10 15:00:00');

                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[0].RoomID,
                    EventName = "Product Launch Meeting",
                    EventOwner = "Jennifer Smith",
                    EventStartDateTime = Convert.ToDateTime("2023-10-01 09:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-01 11:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[1].RoomID,
                    EventName = "Team Offsite Discussion",
                    EventOwner = "Brian Johnson",
                    EventStartDateTime = Convert.ToDateTime("2023-10-02 10:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-02 15:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[2].RoomID,
                    EventName = "Financial Review",
                    EventOwner = "Charles Brown",
                    EventStartDateTime = Convert.ToDateTime("2023-10-03 13:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-03 15:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[3].RoomID,
                    EventName = "HR Policy Refresh",
                    EventOwner = "Natalie Davis",
                    EventStartDateTime = Convert.ToDateTime("2023-10-04 15:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-04 17:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[4].RoomID,
                    EventName = "Quarterly Sales Review",
                    EventOwner = "Erica Martinez",
                    EventStartDateTime = Convert.ToDateTime("2023-10-05 16:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-05 18:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[5].RoomID,
                    EventName = "Engineering Sprint Retrospective",
                    EventOwner = "Derek Clark",
                    EventStartDateTime = Convert.ToDateTime("2023-10-06 09:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-06 10:30:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[6].RoomID,
                    EventName = "Design Thinking Workshop",
                    EventOwner = "Angela Robinson",
                    EventStartDateTime = Convert.ToDateTime("2023-10-07 11:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-07 15:00:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[7].RoomID,
                    EventName = "Customer Feedback Session",
                    EventOwner = "Oliver Wilson",
                    EventStartDateTime = Convert.ToDateTime("2023-10-08 14:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-08 16:30:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[8].RoomID,
                    EventName = "Marketing Strategy Planning",
                    EventOwner = "Isabella Taylor",
                    EventStartDateTime = Convert.ToDateTime("2023-10-09 09:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-09 12:30:00")
                });
                eventRepo.Add(requestingUserName, new Tbl_FactEvent
                {
                    RoomID = rooms[9].RoomID,
                    EventName = "Leadership Training",
                    EventOwner = "Sophia Lee",
                    EventStartDateTime = Convert.ToDateTime("2023-10-10 12:00:00"),
                    EventEndDateTime = Convert.ToDateTime("2023-10-10 15:00:00")
                });
                eventsAdded = eventRepo.FindAll(requestingUserName).ToList().Count;

                message = dataExists ? $"Data existed and was removed! " : string.Empty;
                message += $"Added {officesAdded} offices, {roomsAdded} rooms, and {eventsAdded} events!";
                result = true;
                WriteToLog(message);
                return (result, message);
            }
            catch (Exception ex)
            {
                message = $"Error creating sample data! {GetExceptionMessage(ex)}";
                WriteToLog(message);
                return (false, message);
            }
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