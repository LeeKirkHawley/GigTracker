﻿use Master

IF  NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'GigTracker')
    BEGIN
        CREATE DATABASE GigTracker
    END;


use GigTracker

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User' and xtype='U')
BEGIN
    CREATE TABLE [User] (
		Id int NOT NULL IDENTITY PRIMARY KEY,
        UserName varchar(128) not null,
		Password varchar(64) null,
		FirstName varchar(64) null,
		LastName varchar(64) null,
		Email varchar(256) not null
    )
END;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Gig' and xtype='U')
BEGIN
    CREATE TABLE Gig (
		Id int NOT NULL IDENTITY PRIMARY KEY,
        VenueName varchar(128) not null,
		VenueAddress varchar(64) null,
		VenuePhone varchar(64) null,
		Date datetime null,
		ArtistName varchar(256) not null
    )
END;


GO