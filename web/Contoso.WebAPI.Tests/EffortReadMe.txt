I've been on a quest to find a better way to do unit testing.  I write a lot of code that uses the data repository pattern to access data in a database, and mocking calls to databases just so you can say that you've tested your functions doesn't really get me excited about unit testing.  I recently ran into a project called Effort.EF6 and was very intrigued as it looks like a much cleaner and effective way of testing data methods.  I found several examples online, but none that seemed to be complete working examples.  With a little bit of Effort, I managed to make this project work, and wanted to share that with others.

This document will guide you through updating a unit test project that uses RhinoMocks into a set of Unit Tests that uses Effort and an in-memory database to actually execute the data methods against a real database and fully exercise your code.

The only drawback I've seen so far is that this is designed for LINQ to SQL Entity Framework calls, but it doesn't do anything for Stored Procedures, so you will still have to mock those.

This document will explain what was changed step by step, so that you can update your own project with these changes.

========================================================================================================================
1.	Update DbContext to accept a DbConnection
========================================================================================================================
The data context where your tables are defined can stay almost the same, but you have to be able to set the type of database connection that it uses.  Change the DbContext.cs file (shown here are "ProjectEntities") to accept a connection as parameter:

Old DbContext:
public class ProjectEntities : DbContext
{
    . . . {tables} . . . 
}
New DbContext:
using System.Data.Common;
    public class ProjectEntities : DbContext
    {
        public ProjectEntities() { }
        public ProjectEntities(DbConnection connection) : base(connection, true) { }
    . . . {tables} . . . 
    }

========================================================================================================================
2.	Update Repositories to Accept new DbContext
========================================================================================================================
Update your Repositories to accept DbContext as a parameter, rather than just creating the context yourself.

Old Repository:
    public class TableNameRepository : _BaseRepository
    {
        protected ProjectEntities db = new ProjectEntities();
    ...
}
New Repository
    public class TableNameRepository : _BaseRepository
    {
        protected ProjectEntities db;
        public TableNameRepository()
        {
            db = new ProjectEntities ();
        }

        public TableNameRepository(ProjectEntities context)
        {
            db = context;
        }
    ...
}

========================================================================================================================
3.	Add Effort.EF6
========================================================================================================================
Go in to your Unit Test project and add the Effort.Ef6 Nuget package.  
Make sure your unit test project includes a reference to System.Web and System.Web.Mvc.

========================================================================================================================
4.	Create Effort DbContext
========================================================================================================================
In your Unit Test project, create a new DbContext Factory for Effort to use.
See EffortProviderFactory.cs

========================================================================================================================
5.	Create Base Test Class
========================================================================================================================
Create a Base Test Class that all your other tests inherit from, so you don't have to repeat this stuff for every test.
See BaseEffortTestController.cs

========================================================================================================================
6.	Create Sample Data
========================================================================================================================
Edit SampleDataManager.cs to create the data you want to use for your tests.

========================================================================================================================
7.	Create an API Test Object
========================================================================================================================
Update the the API Test Classes for your Table API.  The CreateTestData function will be whatever data you want to initialize your in-memory database with.  The last three functions (BuildControllerWithMockRequestObject(), BuildTableNameRepository(), BuildTableNameController()) are key to making this work, as well as the CreateTestData() function.  In the BuildControllerWithMockRequestObject, your controller should create the controller context using the BuildMockAPIControllerContext method.

========================================================================================================================
8.	Create View Tests
========================================================================================================================
Create a test class to test each of the views for each table.  The last three functions (BuildControllerWithMockRequestObject(), BuildTableNameRepository(), BuildTableNameController()) are key to making this work, as well as the CreateTestData() function.  In the BuildControllerWithMockRequestObject, your controller should create the controller context using the BuildMockHttpContext method.

========================================================================================================================
9.	Start Testing!
========================================================================================================================
Your tests should be ready to go!  I was able to get 100% code coverage on the repository and models with this method, and a very high percent coverage on the API and View Controllers with this code.

I hope this was helpful for you – let me know what you think!

Lyle
