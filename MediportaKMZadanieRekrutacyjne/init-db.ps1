# Skrypt PowerShell
$server = "localhost"
$database = "SODB"
$user = "sa"
$password = "admin1234"
$connectionString = "Server=$server;Database=$database;User Id=$user;Password=$password;"

$query = @"
CREATE DATABASE $database
GO
USE $database
GO
CREATE TABLE [dbo].[Tags] (
    [TagID]                INT            IDENTITY (1, 1) NOT NULL,
    [HasSynonyms]          BIT            NOT NULL,
    [IsModeratorOnly]      BIT            NOT NULL,
    [IsRequired]           BIT            NOT NULL,
    [Count]                INT            NOT NULL,
    [Name]                 NVARCHAR (MAX) NULL,
    [PopulationPercentage] DECIMAL (7, 5) NULL
)
GO
"@

Invoke-Sqlcmd -ConnectionString $connectionString -Query $query
