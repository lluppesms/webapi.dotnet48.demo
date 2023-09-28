# .NET Framework 4.8 WebAPI Example

This repo contains an example of deploying a .NET Framework 4.8 MVC/WebAPI app to Azure using GitHub Actions.

---

## Feature Demo: SQL Database Access via Managed Identity

The web application shows an example of using Entity Framework to access the database with the Azure Web Application's Managed Identity so there is no secret information in the connection strings. See the constructor in the [DatabaseEntities.cs](/web/Contoso.WebApi/Models/DatabaseEntities.cs) for an example of what needs to be done in order to enable this, which adds these lines to the constructor:

``` csharp
var conn = (System.Data.SqlClient.SqlConnection)Database.Connection;
var credential = new Azure.Identity.DefaultAzureCredential();
var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
conn.AccessToken = token.Token;
```

---

## Feature Demo: In-Memory Unit Tests

The Contoso.WebApi.Tests project shows an example of how the SQL Server can be mocked with a simple in-memory database, allowing you to run unit tests in your pipeline without having to have a SQL Server available.

---

## Deploying the App

See the [.github/workflows/readme.md](.github/workflows/readme.md) file for secrets and variables that need to be created before running the GitHub Actions to deploy this application.

Once the secrets have been created, run the [deploy-dnf-infra-bicep-website](.github/workflows/deploy-dnf-infra-bicep-website.yml) to deploy the application to Azure.

Alternatively, the jobs to deploy the Azure and to build/deploy the web application can be run separately.

- [deploy-dnf-infra-bicep.yml](.github/workflows/deploy-dnf-infra-bicep.yml)

- [deploy-dnf-website.yml](.github/workflows/deploy-dnf-website.yml)

These GitHub actions are currently all set to manually deploy. Once you are ready, change the triggers to automatically run the deploys when you check in.

---

## Additional Step: SQL Server Setup

    Note: I haven't got all of the SQL database setup automated yet, but it's just a one-time step once you have a SQL server deployed. At some point, I'll automate this, just ran out of time this week...!

### To Grant Access to the Web App's Managed Identity

Run the [/database/UpdateSecurity.sql](/database/UpdateSecurity.sql) script to grant the Web App's Managed Identity to the database.

### Create the demo tables and data

Run the [/database/CreateDatabaseAndData.sql](/database/CreateDatabaseAndData.sql) script to create the tables and data. If you run the script multiple times, it will detect that the tables and/or data exists and will skip over those steps.
