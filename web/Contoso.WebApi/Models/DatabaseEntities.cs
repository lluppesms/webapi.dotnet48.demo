//-----------------------------------------------------------------------
// <copyright file="DatabaseEntities.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc.. All rights reserved.
// </copyright>
// <summary>
// Database Entities
// </summary>
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// Using Managed Identity rights to connect to SQL Server
//-----------------------------------------------------------------------
// if this is a local database, skip the constructor logic,
// otherwise use the App Service Managed Identity to get a token for login
//-----------------------------------------------------------------------
// Run these scripts in the database to grant access to the App Service MI:
//    CREATE USER [<appServiceIdentityName>] FROM EXTERNAL PROVIDER
//    ALTER ROLE db_owner ADD MEMBER  [<appServiceIdentityName>]
//-----------------------------------------------------------------------
// The Connection string should be set to the following:
//    data source=tcp:<databaseServerName>.database.windows.net,1433;initial catalog=<databaseName>;
//-----------------------------------------------------------------------
// For more info, see:
// https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cef%2Cdotnet
//-----------------------------------------------------------------------

using System.Data.Common;
using System.Data.Entity;

namespace Contoso.WebApi.Data
{
	/// <summary>
	/// Database Entities
	/// </summary>
	public class DatabaseEntities : DbContext
	{
		public DatabaseEntities()
		{
			var conn = (System.Data.SqlClient.SqlConnection)Database.Connection;
			if (!conn.ConnectionString.Contains("data source=."))
			{
				var credential = new Azure.Identity.DefaultAzureCredential();
				var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
				conn.AccessToken = token.Token;
			}
		}

		public DatabaseEntities(DbConnection connection)
		: base(connection, true)
		{
		}

		/// <summary>
		/// Tbl_DimOffice Table
		/// </summary>
		public DbSet<Tbl_DimOffice> Tbl_DimOffice { get; set; }

		/// <summary>
		/// Tbl_DimRoom Table
		/// </summary>
		public DbSet<Tbl_DimRoom> Tbl_DimRoom { get; set; }

		/// <summary>
		/// Tbl_FactEvent Table
		/// </summary>
		public DbSet<Tbl_FactEvent> Tbl_FactEvent { get; set; }
	}
}
