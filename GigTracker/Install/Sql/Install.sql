use Master

IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'GigTracker')
    BEGIN
		RAISERROR('Creating database GigTracker', 0, 1) WITH NOWAIT;
        CREATE DATABASE GigTracker
    END;


use GigTracker

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User' and xtype='U')
BEGIN
    CREATE TABLE [User] (
		Id int NOT NULL IDENTITY PRIMARY KEY,
        UserName varchar(128) not null,
		Password varchar(256) null,
		FirstName varchar(64) null,
		LastName varchar(64) null,
		Email varchar(256) not null,
		Role varchar(64) null,
		Token varchar(200) null
    )
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Gig' and xtype='U')
BEGIN
    CREATE TABLE Gig (
		Id int NOT NULL IDENTITY PRIMARY KEY,
		UserId int NOT NULL,
        VenueName varchar(128) not null,
		VenueAddress varchar(64) null,
		VenuePhone varchar(64) null,
		Date datetime2 null,
		ArtistName varchar(256) not null,
		constraint fk_gig_user foreign key (UserId) references [User](Id)
    )
END;
GO