CREATE DATABASE SODB;

GO

USE SODB;

GO

CREATE TABLE [dbo].[Tags] (
    [TagID]                INT            IDENTITY (1, 1) NOT NULL,
    [HasSynonyms]          BIT            NOT NULL,
    [IsModeratorOnly]      BIT            NOT NULL,
    [IsRequired]           BIT            NOT NULL,
    [Count]                INT            NOT NULL,
    [Name]                 NVARCHAR (MAX) NULL,
    [PopulationPercentage] DECIMAL (7, 5) NULL
);

GO