DECLARE @DropTables VARCHAR(1) = 'N'
DECLARE @DeleteData VARCHAR(1) = 'N'
DECLARE @ReseedTables VARCHAR(1) = 'Y'
DECLARE @DisplayData VARCHAR(1) = 'Y'

-- ------------------------------------------------------------------------------------------
IF @DropTables = 'Y'
BEGIN
    -- SELECT * FROM sys.tables 
	PRINT 'Dropping all tables...!'
    DROP TABLE tbl_FactEvent;
    DROP TABLE tbl_DimRoom;
    DROP TABLE tbl_DimOffice;
END

IF (EXISTS(SELECT * FROM sys.tables WHERE NAME = 'tbl_DimOffice'))
BEGIN
  PRINT 'Tables already exist - skipping creation!'
END
ELSE
BEGIN
	PRINT 'Creating all tables...!'
    CREATE TABLE tbl_DimOffice
    (
        OfficeID INT IDENTITY(1,1) PRIMARY KEY,
        OfficeName VARCHAR(255) NOT NULL,
        OfficeAddress VARCHAR(255) NOT NULL,
        OfficeCity VARCHAR(100) NOT NULL,
        OfficeState VARCHAR(50) NOT NULL,
        OfficeZip VARCHAR(10) NOT NULL,
        OfficeCountry VARCHAR(100) NOT NULL
    );

    CREATE TABLE tbl_DimRoom
    (
        RoomID INT IDENTITY(1,1) PRIMARY KEY,
        OfficeID INT,
        RoomName VARCHAR(255) NOT NULL,
        FOREIGN KEY (OfficeID) REFERENCES tbl_DimOffice(OfficeID)
    );

    CREATE TABLE tbl_FactEvent
    (
        EventID INT IDENTITY(1,1) PRIMARY KEY,
        RoomID INT,
        EventName VARCHAR(255) NOT NULL,
        EventOwner VARCHAR(255) NOT NULL,
        EventStartDateTime DATETIME NOT NULL,
        EventEndDateTime DATETIME NOT NULL,
        FOREIGN KEY (RoomID) REFERENCES tbl_DimRoom(RoomID)
    );
END

-- ------------------------------------------------------------------------------------------
IF @DeleteData = 'Y'
BEGIN
	PRINT 'Deleting all data from tables...!'
	DELETE FROM tbl_FactEvent
	DELETE FROM tbl_DimRoom
	DELETE FROM tbl_DimOffice
	IF @ReseedTables = 'Y'
	BEGIN
		PRINT 'Resetting identities to 1!'
		DBCC CHECKIDENT (tbl_FactEvent, RESEED, 0);
		DBCC CHECKIDENT (tbl_DimRoom, RESEED, 0);
		DBCC CHECKIDENT (tbl_DimOffice, RESEED, 0);
	END
END

-- ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT *
FROM tbl_DimOffice)
BEGIN
	PRINT 'Inserting office data...!'
	INSERT INTO tbl_DimOffice
		(OfficeName, OfficeAddress, OfficeCity, OfficeState, OfficeZip, OfficeCountry)
	VALUES
		('Home Office',    '123 Main St', 'Irving',  'TX', '75039', 'USA'),
		('Redmond Office', '456 Main St', 'Redmond', 'WA', '98052', 'USA'),
		('Chicago Office', '789 Main St', 'Chicago', 'IL', '60601', 'USA');
END

-- ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT *
FROM tbl_DimRoom)
BEGIN
	PRINT 'Inserting room data...!'
	-- Assuming each office has 5 rooms
	INSERT INTO tbl_DimRoom
		(OfficeID, RoomName)
	SELECT OfficeID, 'Conference Room'
	FROM tbl_DimOffice
	INSERT INTO tbl_DimRoom
		(OfficeID, RoomName)
	SELECT OfficeID, 'Meeting Room 101'
	FROM tbl_DimOffice
	INSERT INTO tbl_DimRoom
		(OfficeID, RoomName)
	SELECT OfficeID, 'Auditorium'
	FROM tbl_DimOffice
	INSERT INTO tbl_DimRoom
		(OfficeID, RoomName)
	SELECT OfficeID, 'Break Room'
	FROM tbl_DimOffice
	INSERT INTO tbl_DimRoom
		(OfficeID, RoomName)
	SELECT OfficeID, 'Executive Lounge'
	FROM tbl_DimOffice
END

-- ------------------------------------------------------------------------------------------
IF NOT EXISTS(SELECT *
FROM tbl_FactEvent)
BEGIN
	PRINT 'Inserting event data...!'
	INSERT INTO tbl_FactEvent
		(RoomID, EventName, EventOwner, EventStartDateTime, EventEndDateTime)
	VALUES
		(1, 'Product Launch Meeting', 'Jennifer Smith', '2023-10-01 09:00:00', '2023-10-01 11:00:00'),
		(2, 'Team Offsite Discussion', 'Brian Johnson', '2023-10-02 10:00:00', '2023-10-02 15:00:00'),
		(3, 'Financial Review', 'Charles Brown', '2023-10-03 13:00:00', '2023-10-03 15:00:00'),
		(4, 'HR Policy Refresh', 'Natalie Davis', '2023-10-04 15:00:00', '2023-10-04 17:00:00'),
		(5, 'Quarterly Sales Review', 'Erica Martinez', '2023-10-05 16:00:00', '2023-10-05 18:00:00'),
		(6, 'Engineering Sprint Retrospective', 'Derek Clark', '2023-10-06 09:00:00', '2023-10-06 10:30:00'),
		(7, 'Design Thinking Workshop', 'Angela Robinson', '2023-10-07 11:00:00', '2023-10-07 15:00:00'),
		(8, 'Customer Feedback Session', 'Oliver Wilson', '2023-10-08 14:00:00', '2023-10-08 16:30:00'),
		(9, 'Marketing Strategy Planning', 'Isabella Taylor', '2023-10-09 09:00:00', '2023-10-09 12:30:00'),
		(10, 'Leadership Training', 'Sophia Lee', '2023-10-10 12:00:00', '2023-10-10 15:00:00');
END

-- ------------------------------------------------------------------------------------------
IF @DisplayData = 'Y'
BEGIN
	PRINT 'Displaying data...!'
	SELECT o.*, r.*, e.*
	FROM tbl_FactEvent e
		INNER JOIN tbl_DimRoom r ON e.RoomID = r.RoomID
		INNER JOIN tbl_DimOffice o ON r.OfficeID = o.OfficeID
END

