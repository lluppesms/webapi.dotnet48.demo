//-----------------------------------------------------------------------
// <copyright file="BaseEffortTestController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Base Test Class for Effort Tests
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using Contoso.WebApi.SampleData;
using Effort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Contoso.WebApi.Tests
{
    /// <summary>
    /// Base Test Class
    /// </summary>
    [TestClass]
    public class BaseEffortTestController
    {
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName = "UNITTEST";

		/// <summary>
		/// The user name
		/// </summary>
		public string UserNameAdmin = "UNITTEST";

		/// <summary>
		/// The database context
		/// </summary>
		public DatabaseEntities dbContext;

        /// <summary>
        /// Runs when assembly is started to initialize Effort Data Provider
        /// </summary>
        /// <param name="context">The context.</param>
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            //// This code makes sure that Effort is initialized BEFORE the application starts.
            //// If you don't have this, then your tests will work locally, but will fail on
            //// builds or when Analyzing Code Coverage, like this:
            ////    Message: Initialization method SetupTest threw exception.
            //// System.TypeInitializationException: System.TypeInitializationException:
            //// The type initializer for 'Effort.DbConnectionFactory' threw an exception. --->
            //// Effort.Exceptions.EffortException: The Effort library failed to register its
            //// provider automatically, so manual registration is required.
            ////    The Entity Framework was already using a DbConfiguration instance before an
            //// attempt was made to add an 'Loaded' event handler. 'Loaded' event handlers can
            //// only be added as part of application start up before the Entity Framework is
            //// used.See http://go.microsoft.com/fwlink/?LinkId=260883 for more information..
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        /// <summary>
        /// Runs when test is initialized to set up context and sample data
        /// </summary>
        [TestInitialize]
        public void SetupTest()
        {
            CreateDbContextIfNeeded();
        }

        /// <summary>
        /// Creates the database context if needed.
        /// </summary>
        public void CreateDbContextIfNeeded()
        {
            ////DbConnection effortConnection =
            ////  Effort.DbConnectionFactory.CreatePersistent("MyInstanceName");
            ////MyContext context = new MyContext(effortConnection);
            ////  There are two options here, CreatePersistent which takes an instance name
            //// (and keeps the database for the duration of the whole test suite) and
            //// CreateTransient which doesn’t need a name and lasts only as long as the variable
            ////    you store it in. You can choose the appropriate one for your needs, as it is a
            ////    trade - off between the time it takes
            ////    to set up your database and test data – and the risk of two tests interacting
            ////    coincidentally because of shared data.
            if (dbContext == null)
            {
                var connection = DbConnectionFactory.CreateTransient();
                dbContext = new DatabaseEntities(connection);
				CreateUserIdentityData();
            }
        }

        /// <summary>
        /// Creates the user data.
        /// </summary>
        public void CreateUserIdentityData()
        {
            if (dbContext != null)
            {
                SampleDataManager.CreateSampleData();
                //dbContext.AspNetUser.AddRange(SampleDataManager.Test_AspNetUser);
                //dbContext.AspNetRole.AddRange(SampleDataManager.Test_AspNetRole);
                //dbContext.AspNetUserRole.AddRange(SampleDataManager.Test_AspNetUserRole);
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Builds the mock API controller context.
        /// </summary>
        /// <returns>Context</returns>
        public HttpControllerContext BuildMockAPIControllerContext()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary());
            return new HttpControllerContext(config, routeData, request);
        }

        /// <summary>
        /// Builds the mock HTTP context.
        /// </summary>
        /// <returns>Context</returns>
        public HttpContextBase BuildMockHttpContext()
        {
            var mockHttpContext = MockRepository.GenerateMock<HttpContextBase>();
            var mockRequest = MockRepository.GenerateMock<HttpRequestBase>();
            mockHttpContext.Stub(x => x.Request).Return(mockRequest);
            mockRequest.Stub(x => x.HttpMethod).Return("GET");
            return mockHttpContext;
        }

        /// <summary>
        /// Creates security principal and adds it to the current context
        /// </summary>
        public GenericPrincipal AddUserIdentityClaims()
        {
            var identity = new GenericIdentity(UserName);
            identity.AddClaim(new System.Security.Claims.Claim("WindowsAccountName", UserName));
            var principal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = principal;
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""), new HttpResponse(new System.IO.StringWriter()));
            HttpContext.Current.User = principal;
            return principal;
        }
    }
}