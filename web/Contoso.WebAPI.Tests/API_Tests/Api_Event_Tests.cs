//-----------------------------------------------------------------------
// <copyright file="Api_Tbl_FactEvent_Tests.cs" company="Luppes Consulting, Inc.">
//   Copyright © 2023  Luppes Consulting, Inc.
// </copyright>
// <summary>
//   Tbl_FactEvent API Test Cases
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.API;
using Contoso.WebApi.Data;
using Contoso.WebApi.SampleData;
using Contoso.WebApi.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Http.Hosting;

namespace Contoso.WebApi.UnitTests.API
{
	/// <summary>
	/// Tbl_FactEvent API Test Class
	/// </summary>
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class Api_Event_Tests : BaseEffortTestController
	{
		#region Variables
		/// <summary>
		/// The repository
		/// </summary>
		private Tbl_FactEventRepository repository;

		/// <summary>
		/// The controller
		/// </summary>
		private EventController controller;
		#endregion

		#region Initialization
		/// <summary>
		/// Initializes the test.
		/// </summary>
		[TestInitialize]
		public void InitializeTest()
		{
			BuildControllerWithMockRequestObject();
		}

		/// <summary>
		/// Creates the test data.
		/// </summary>
		public void CreateTestData()
		{
			CreateDbContextIfNeeded();
			SampleDataManager.CreateSampleData();
			dbContext.Tbl_DimRoom.AddRange(SampleDataManager.Test_Tbl_DimRoom);
			dbContext.Tbl_DimOffice.AddRange(SampleDataManager.Test_Tbl_DimOffice);
			dbContext.Tbl_FactEvent.AddRange(SampleDataManager.Test_Tbl_FactEvent);
			dbContext.SaveChanges();
			//SampleDataManager.CreateSampleData(dbContext);
		}
		#endregion

		#region Get Data Tests
		/// <summary>
		/// Tbl_FactEvent Get All Records works
		/// </summary>
		[TestMethod]
		public void API_Event_Get_All_Works()
		{
			// Arrange
			// Act
			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.OK, "API did not return the expected HTTP Status!");

			var content = result.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<List<Tbl_FactEvent>>(content);
			Assert.IsTrue(data.Count >= SampleDataManager.Test_Tbl_FactEvent.Count, "API did not return the expected records!");
			Assert.IsNotNull(data.First().EventName, "Data should not be null!");
		}

		///// <summary>
		///// Tbl_FactEvent Get All Grid Records works
		///// </summary>
		//[TestMethod]
		//public void API_Event_Get_All_Grid_Works()
		//{
		//	// Arrange
		//	// Act
		//	var data = controller.GetGrid();

		//	// Assert
		//	Assert.IsNotNull(data);

		//	Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_FactEvent.Count, "API did not return the expected records!");
		//	Assert.IsNotNull(data.Data.First().EventName, "Data should not be null!");
		//}

		/// <summary>
		/// Tbl_FactEvent Repository Grid Paging works
		/// </summary>
		[TestMethod]
		public void Repository_Event_Grid_Paging_Works()
		{
			// Arrange
			// Act
			var data = repository.FindGridRecords(UserNameAdmin, string.Empty, 1, 1);

			// Assert
			Assert.IsNotNull(data);
			Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_FactEvent.Count, "API did not return the expected records!");
		}

		/// <summary>
		/// Tbl_FactEvent Repository Grid Search works
		/// </summary>
		[TestMethod]
		public void Repository_Event_Grid_Search_Works()
		{
			// Arrange
			// Act
			var data = repository.FindGridRecords(UserNameAdmin, "e", 1, 1);

			// Assert
			Assert.IsNotNull(data);
			Assert.IsTrue(data.Total >= 0, "API did not return the expected records!");
		}

		/// <summary>
		/// Tbl_FactEvent Get One Record Invalid Id fails
		/// </summary>
		[TestMethod]
		public void API_Event_Get_One_Missing_Fails()
		{
			// Arrange
			// Act
			var result = controller.GetEvent(-1);

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "Invalid Status Code returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Get One Record works
		/// </summary>
		[TestMethod]
		public void API_Event_Get_One_Works()
		{
			// Arrange
			// Act
			var result = controller.GetEvent(SampleDataManager.Test_Tbl_FactEvent[0].EventID);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<Tbl_FactEvent>(content);
			Assert.IsNotNull(data.EventName, "Data should not be null!");
		}
		#endregion

		#region Create (POST) Data Tests
		/// <summary>
		/// Tbl_FactEvent Create (POST) fails with bad data
		/// </summary>
		[TestMethod]
		public void API_Event_Post_Fails_BadData()
		{
			// Arrange
			var data = new Tbl_FactEvent();

			// Act
			var result = controller.PostEvent(data);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode, "Invalid Status Code returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Create (POST) fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Event_Post_Fails_EmptyData()
		{
			// Arrange
			var expectedMessage = "0";

			// Act
			var result = controller.PostEvent(null);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");

			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Create (POST) fails with duplicate data
		/// </summary>
		[TestMethod]
		public void API_Event_Post_Fails_Duplicate()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_FactEvent { EventID = 1, EventName = "Event 1", RoomID = 1, EventOwner = "User 1", EventStartDateTime = Convert.ToDateTime("09/01/2023 08:00"), EventEndDateTime = Convert.ToDateTime("09/01/2023 08:30") };
			var expectedMessage = "This value already exists!";

			// Act
			var result = controller.PostEvent(data);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Create (POST) Works with good data
		/// </summary>
		[TestMethod]
		public void API_Event_Post_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_FactEvent { RoomID = 1, EventID = 99, EventName = "Unit Test POST", EventOwner = "User 1", EventStartDateTime = Convert.ToDateTime("09/01/2023 08:00"), EventEndDateTime = Convert.ToDateTime("09/01/2023 08:30") };

			// Act
			var result = controller.PostEvent(data);
			var resultDelete = controller.DeleteEvent(data.EventID);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			var contentAdd = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(HttpStatusCode.Created, result.StatusCode, "Invalid Status Code returned! " + contentAdd);
			var contentDelete = result.Content.ReadAsStringAsync().Result;
			Assert.IsFalse(string.IsNullOrEmpty(contentDelete), "Invalid message returned!");
		}
		#endregion

		#region Update (PUT) Data Tests
		/// <summary>
		/// Tbl_FactEvent Put fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Event_Put_Fails_EmptyData()
		{
			// Arrange
			var expectedMessage = "Update Tbl_FactEvent Failed!";

			// Act
			var result = controller.PutEvent(null);

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Update (PUT) fails with bad data
		/// </summary>
		[TestMethod]
		public void API_Event_Put_Fails_BadData()
		{
			// Arrange
			var data = new Tbl_FactEvent();
			var expectedMessage = "Update Tbl_FactEvent Failed!";

			// Act
			var result = controller.PutEvent(data);

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_FactEvent Update (PUT) Works with good data
		/// </summary>
		[TestMethod]
		public void API_Event_Put_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_FactEvent { EventID = SampleDataManager.Test_Tbl_FactEvent[0].EventID, EventName = SampleDataManager.Test_Tbl_FactEvent[0].EventName, EventOwner = SampleDataManager.Test_Tbl_FactEvent[0].EventOwner, EventStartDateTime = SampleDataManager.Test_Tbl_FactEvent[0].EventStartDateTime, EventEndDateTime = SampleDataManager.Test_Tbl_FactEvent[0].EventEndDateTime };

			// Act
			data.EventName = "Unit Test 2";
			var result = controller.PutEvent(data);

			// Assert
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned! " + content);
		}

		/// <summary>
		/// Tbl_FactEvent Repository Save works
		/// </summary>
		[TestMethod]
		public void Repository_Event_Save_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = SampleDataManager.Test_Tbl_FactEvent[0];

			// Act
			var result = repository.Save(UserNameAdmin, data);

			// Assert
			Assert.AreEqual(true, result, "Invalid Status Code returned! ");
		}
		#endregion

		#region Delete Data Tests
		/// <summary>
		/// Tbl_FactEvent Delete fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Event_Delete_Fails_MissingData()
		{
			// Arrange
			// Act
			var result = controller.DeleteEvent(-1);

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Tbl_FactEvent Delete Works with good data
		/// </summary>
		[TestMethod]
		public void API_Event_Delete_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_FactEvent { EventID = 97, EventName = "Unit Test DELETE", RoomID = 1, EventOwner = "User 1", EventStartDateTime = Convert.ToDateTime("09/01/2023 08:00"), EventEndDateTime = Convert.ToDateTime("09/01/2023 08:30") };

			// Act
			var resultAdd = controller.PostEvent(data);
			var contentAdd = resultAdd.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(HttpStatusCode.Created, resultAdd.StatusCode, "Add returned invalid Status Code! " + contentAdd);
			var result = controller.DeleteEvent(data.EventID);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Tbl_FactEvent Delete Check Works
		/// </summary>
		[TestMethod]
		public void API_Event_DeleteCheck_Works()
		{
			// Arrange
			var msg = string.Empty;

			// Act
			var result = repository.DeleteCheck(UserNameAdmin, SampleDataManager.Test_Tbl_FactEvent[0].EventID, ref msg);

			// Assert
			Assert.AreEqual(true, result, "API did not return the expected HTTP Status!");
		}
		#endregion

		#region Exception Tests
		/// <summary>
		/// Grid Search throws exception
		/// </summary>
		[TestMethod]
		public void Repository_Event_Grid_Throws_Exception()
		{
			//// Arrange
			var mockRepo = new Tbl_FactEventRepository(null);

			//// Act
			var data = mockRepo.FindGridRecords(UserNameAdmin, string.Empty, 1, 1);

			//// Assert
			Assert.IsNotNull(data);
			Assert.IsTrue(data.Total == 0, "Method did not return the expected records!");
			Assert.IsNotNull(data.ReturnMessageTxt, "Expected an exception message!");
		}

		/// <summary>
		/// Put throws exception
		/// </summary>
		[TestMethod]
		public void API_Event_Put_Throws_Exception()
		{
			//// Arrange
			var data = new Tbl_FactEvent { EventID = SampleDataManager.Test_Tbl_FactEvent[0].EventID, EventName = SampleDataManager.Test_Tbl_FactEvent[0].EventName };

			var mockRepo = MockRepository.GenerateStub<ITbl_FactEventRepository>();
			mockRepo.Stub(x => x.Save(UserNameAdmin, data.EventID, data)).Throw(new Exception("Yikes!"));

			var controller = BuildMockingTbl_FactEventController(mockRepo);

			//// Act
			var result = controller.PutEvent(data);

			//// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Delete throws exception
		/// </summary>
		[TestMethod]
		public void API_Event_Delete_Throws_Exception()
		{
			//// Arrange
			var data = new Tbl_FactEvent { EventID = SampleDataManager.Test_Tbl_FactEvent[0].EventID, EventName = SampleDataManager.Test_Tbl_FactEvent[0].EventName };

			var mockRepo = MockRepository.GenerateStub<ITbl_FactEventRepository>();
			mockRepo.Stub(x => x.FindOne(UserNameAdmin, data.EventID)).Return(data);
			mockRepo.Stub(x => x.Delete(UserNameAdmin, data.EventID)).Throw(new Exception("Yikes!"));

			var controller = BuildMockingTbl_FactEventController(mockRepo);

			//// Act
			var result = controller.DeleteEvent(data.EventID);

			//// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
		}
		#endregion

		#region Coverage Tests
		/// <summary>
		/// Tbl_FactEvent Create/Dispose Test
		/// </summary>
		[TestMethod]
		public void API_Event_Create_Dispose_Works()
		{
			// Arrange
			// Act
			controller.Dispose();

			// Assert
			Assert.IsNotNull(controller, "Controller dispose failed!");
		}

		/// <summary>
		/// Create/Dispose Tbl_FactEvent Test
		/// </summary>
		[TestMethod]
		public void API_Event_Create_NoParm_Works()
		{
			//// Arrange
			var controller = new EventController();

			//// Act
			controller.Dispose();

			//// Assert
			Assert.IsNotNull(controller, "Controller dispose failed!");
		}

		/// <summary>
		/// Create/Dispose Tbl_FactEvent Test
		/// </summary>
		[TestMethod]
		public void API_Event_Create_Repository_Works()
		{
			//// Arrange
			BuildTbl_FactEventRepository();
			var controller = new EventController(repository);

			//// Act
			controller.Dispose();

			//// Assert
			Assert.IsNotNull(controller, "Controller dispose failed!");
		}
		#endregion

		#region Helper Functions
		/// <summary>
		/// Build Controller And Repository
		/// </summary>
		public void BuildControllerWithMockRequestObject()
		{
			BuildTbl_FactEventRepository();
			BuildTbl_FactEventController();
		}

		/// <summary>
		/// Build Repository
		/// </summary>
		public void BuildTbl_FactEventRepository()
		{
			if (repository == null)
			{
				CreateTestData();
				repository = new Tbl_FactEventRepository(dbContext);
			}
		}

		/// <summary>
		/// Build Controller
		/// </summary>
		public void BuildTbl_FactEventController()
		{
			if (controller == null)
			{
				if (repository == null)
				{
					BuildTbl_FactEventRepository();
				}
				//controller = new EventController(DbContext, repository)
				controller = new EventController(dbContext)
				{
					//// without this the Request object in the controller is null and tests crash
					ControllerContext = BuildMockAPIControllerContext()
				};
				controller.Request = controller.ControllerContext.Request;
				controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = controller.ControllerContext.Configuration;

				//// Adds the security context to the thread, context, and controller
				GenericPrincipal principal = AddUserIdentityClaims();
				controller.RequestContext.Principal = (IPrincipal)principal;
			}
		}

		/// <summary>
		/// Builds the Tbl_FactEvent Controller with a mock repository
		/// </summary>
		/// <param name="mockRepo">The mock repository</param>
		/// <returns>Mocked Controller</returns>
		public EventController BuildMockingTbl_FactEventController(ITbl_FactEventRepository mockRepo)
		{
			var controller = new EventController(mockRepo)
			{
				//// without this the Request object in the controller is null and tests crash
				ControllerContext = BuildMockAPIControllerContext()
			};
			controller.Request = controller.ControllerContext.Request;
			controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = controller.ControllerContext.Configuration;

			//// Adds the security context to the thread, context, and controller
			GenericPrincipal principal = AddUserIdentityClaims();
			controller.RequestContext.Principal = principal;

			return controller;
		}
		#endregion

		#region Cleanup
		/// <summary>
		/// Disposal
		/// </summary>
		public void Dispose()
		{
			if (controller != null)
			{
				controller = null;
			}
			if (repository != null)
			{
				repository = null;
			}
			if (dbContext != null)
			{
				dbContext = null;
			}
		}
		#endregion
	}
}
