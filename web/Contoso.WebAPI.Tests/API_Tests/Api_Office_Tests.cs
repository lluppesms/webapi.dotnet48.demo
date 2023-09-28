//-----------------------------------------------------------------------
// <copyright file="Api_Tbl_DimOffice_Tests.cs" company="Contoso, Inc.">
//   Copyright © 2017 Contoso, Inc.
// </copyright>
// <summary>
//   Tbl_DimOffice API Test Cases
// </summary>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rhino.Mocks;
using Contoso.WebApi.Tests;
using Contoso.WebApi.API;
using Contoso.WebApi.Data;
using Contoso.WebApi.SampleData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web.Http.Hosting;
using System.Data.Entity;

namespace Contoso.WebApi.UnitTests.API
{
	/// <summary>
	/// Tbl_DimOffice API Test Class
	/// </summary>
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class Api_Office_Tests : BaseEffortTestController
	{
		#region Variables
		/// <summary>
		/// The repository
		/// </summary>
		private Tbl_DimOfficeRepository repository;

		/// <summary>
		/// The controller
		/// </summary>
		private OfficeController controller;
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
		/// Tbl_DimOffice Get All Records works
		/// </summary>
		[TestMethod]
		public void API_Office_Get_All_Works()
		{
			// Arrange
			// Act
			var result = controller.Get();

			// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.OK, "API did not return the expected HTTP Status!");

			var content = result.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<List<Tbl_DimOffice>>(content);
			Assert.IsTrue(data.Count >= SampleDataManager.Test_Tbl_DimOffice.Count, "API did not return the expected records!");
			Assert.IsNotNull(data.First().OfficeName, "Data should not be null!");
		}

		///// <summary>
		///// Tbl_DimOffice Get All Grid Records works
		///// </summary>
		//[TestMethod]
		//public void API_Office_Get_All_Grid_Works()
		//{
		//	// Arrange
		//	// Act
		//	var data = controller.GetGrid();

		//	// Assert
		//	Assert.IsNotNull(data);

		//	Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_DimOffice.Count, "API did not return the expected records!");
		//	Assert.IsNotNull(data.Data.First().OfficeName, "Data should not be null!");
		//}

		/// <summary>
		/// Tbl_DimOffice Repository Grid Paging works
		/// </summary>
		[TestMethod]
		public void Repository_Office_Grid_Paging_Works()
		{
			// Arrange
			// Act
			var data = repository.FindGridRecords(UserNameAdmin, string.Empty, 1, 1);

			// Assert
			Assert.IsNotNull(data);
			Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_DimOffice.Count, "API did not return the expected records!");
		}

		/// <summary>
		/// Tbl_DimOffice Repository Grid Search works
		/// </summary>
		[TestMethod]
		public void Repository_Office_Grid_Search_Works()
		{
			// Arrange
			// Act
			var data = repository.FindGridRecords(UserNameAdmin, "e", 1, 1);

			// Assert
			Assert.IsNotNull(data);
			Assert.IsTrue(data.Total >= 0, "API did not return the expected records!");
		}

		/// <summary>
		/// Tbl_DimOffice Get One Record Invalid Id fails
		/// </summary>
		[TestMethod]
		public void API_Office_Get_One_Missing_Fails()
		{
			// Arrange
			// Act
			var result = controller.GetOffice(-1);

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "Invalid Status Code returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Get One Record works
		/// </summary>
		[TestMethod]
		public void API_Office_Get_One_Works()
		{
			// Arrange
			// Act
			var result = controller.GetOffice(SampleDataManager.Test_Tbl_DimOffice[0].OfficeID);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<Tbl_DimOffice>(content);
			Assert.IsNotNull(data.OfficeName, "Data should not be null!");
		}
		#endregion

		#region Create (POST) Data Tests
		/// <summary>
		/// Tbl_DimOffice Create (POST) fails with bad data
		/// </summary>
		[TestMethod]
		public void API_Office_Post_Fails_BadData()
		{
			// Arrange
			var data = new Tbl_DimOffice();

			// Act
			var result = controller.PostOffice(data);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode, "Invalid Status Code returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Create (POST) fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Office_Post_Fails_EmptyData()
		{
			// Arrange
			var expectedMessage = "0";

			// Act
			var result = controller.PostOffice(null);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");

			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Create (POST) fails with duplicate data
		/// </summary>
		[TestMethod]
		public void API_Office_Post_Fails_Duplicate()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_DimOffice() { OfficeID = 1, OfficeName = "Office 1", OfficeAddress = "100 Main", OfficeCity = "MyTown", OfficeState = "MN", OfficeCountry = "USA", OfficeZip = "55124" };
			var expectedMessage = "This value already exists!";

			// Act
			var result = controller.PostOffice(data);

			// Assert
			Assert.IsNotNull(result, "API did not return anything!");
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Create (POST) Works with good data
		/// </summary>
		[TestMethod]
		public void API_Office_Post_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_DimOffice() { OfficeID = 99, OfficeName = "Office 99 POST", OfficeAddress = "100 Main", OfficeCity = "MyTown", OfficeState = "MN", OfficeCountry = "USA", OfficeZip = "55124" };

			// Act
			var result = controller.PostOffice(data);
			var resultDelete = controller.DeleteOffice(data.OfficeID);

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
		/// Tbl_DimOffice Put fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Office_Put_Fails_EmptyData()
		{
			// Arrange
			var expectedMessage = "Update Tbl_DimOffice Failed!";

			// Act
			var result = controller.PutOffice(null);

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Update (PUT) fails with bad data
		/// </summary>
		[TestMethod]
		public void API_Office_Put_Fails_BadData()
		{
			// Arrange
			var data = new Tbl_DimOffice();
			var expectedMessage = "Update Tbl_DimOffice Failed!";

			// Act
			var result = controller.PutOffice(data);

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
		}

		/// <summary>
		/// Tbl_DimOffice Update (PUT) Works with good data
		/// </summary>
		[TestMethod]
		public void API_Office_Put_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_DimOffice
			{
				OfficeID = SampleDataManager.Test_Tbl_DimOffice[0].OfficeID,
				OfficeName = SampleDataManager.Test_Tbl_DimOffice[0].OfficeName,
				OfficeAddress = SampleDataManager.Test_Tbl_DimOffice[0].OfficeAddress,
				OfficeCity = SampleDataManager.Test_Tbl_DimOffice[0].OfficeCity,
				OfficeState = SampleDataManager.Test_Tbl_DimOffice[0].OfficeState,
				OfficeCountry = SampleDataManager.Test_Tbl_DimOffice[0].OfficeCountry,
				OfficeZip = SampleDataManager.Test_Tbl_DimOffice[0].OfficeZip
			};
			// Act
			data.OfficeName = "Unit Test 2";
			var result = controller.PutOffice(data);

			// Assert
			var content = result.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned! " + content);
		}

		/// <summary>
		/// Tbl_DimOffice Repository Save works
		/// </summary>
		[TestMethod]
		public void Repository_Office_Save_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = SampleDataManager.Test_Tbl_DimOffice[0];

			// Act
			var result = repository.Save(UserNameAdmin, data);

			// Assert
			Assert.AreEqual(true, result, "Invalid Status Code returned! ");
		}
		#endregion

		#region Delete Data Tests
		/// <summary>
		/// Tbl_DimOffice Delete fails with empty data
		/// </summary>
		[TestMethod]
		public void API_Office_Delete_Fails_MissingData()
		{
			// Arrange
			// Act
			var result = controller.DeleteOffice(-1);

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Tbl_DimOffice Delete Works with good data
		/// </summary>
		[TestMethod]
		public void API_Office_Delete_Works()
		{
			// Arrange
			////TO DO Validate in field values in new object
			var data = new Tbl_DimOffice() { OfficeID = 97, OfficeName = "Office 97 POST", OfficeAddress = "100 Main", OfficeCity = "MyTown", OfficeState = "MN", OfficeCountry = "USA", OfficeZip = "55124" };

			// Act
			var resultAdd = controller.PostOffice(data);
			var contentAdd = resultAdd.Content.ReadAsStringAsync().Result;
			Assert.AreEqual(HttpStatusCode.Created, resultAdd.StatusCode, "Add returned invalid Status Code! " + contentAdd);
			var result = controller.DeleteOffice(data.OfficeID);

			// Assert
			Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Tbl_DimOffice Delete Check Works
		/// </summary>
		[TestMethod]
		public void API_Office_DeleteCheck_Works()
		{
			// Arrange
			var msg = string.Empty;

			// Act
			var result = repository.DeleteCheck(UserNameAdmin, SampleDataManager.Test_Tbl_DimOffice[0].OfficeID, ref msg);

			// Assert
			Assert.AreEqual(true, result, "API did not return the expected HTTP Status!");
		}
		#endregion

		#region Exception Tests
		/// <summary>
		/// Grid Search throws exception
		/// </summary>
		[TestMethod]
		public void Repository_Office_Grid_Throws_Exception()
		{
			//// Arrange
			var mockRepo = new Tbl_DimOfficeRepository(null);

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
		public void API_Office_Put_Throws_Exception()
		{
			//// Arrange
			var data = new Tbl_DimOffice { OfficeID = SampleDataManager.Test_Tbl_DimOffice[0].OfficeID, OfficeName = SampleDataManager.Test_Tbl_DimOffice[0].OfficeName };

			var mockRepo = MockRepository.GenerateStub<ITbl_DimOfficeRepository>();
			mockRepo.Stub(x => x.Save(UserNameAdmin, data.OfficeID, data)).Throw(new Exception("Yikes!"));

			var controller = BuildMockingTbl_DimOfficeController(mockRepo);

			//// Act
			var result = controller.PutOffice(data);

			//// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
		}

		/// <summary>
		/// Delete throws exception
		/// </summary>
		[TestMethod]
		public void API_Office_Delete_Throws_Exception()
		{
			//// Arrange
			var data = new Tbl_DimOffice { OfficeID = SampleDataManager.Test_Tbl_DimOffice[0].OfficeID, OfficeName = SampleDataManager.Test_Tbl_DimOffice[0].OfficeName };

			var mockRepo = MockRepository.GenerateStub<ITbl_DimOfficeRepository>();
			mockRepo.Stub(x => x.FindOne(UserNameAdmin, data.OfficeID)).Return(data);
			mockRepo.Stub(x => x.Delete(UserNameAdmin, data.OfficeID)).Throw(new Exception("Yikes!"));

			var controller = BuildMockingTbl_DimOfficeController(mockRepo);

			//// Act
			var result = controller.DeleteOffice(data.OfficeID);

			//// Assert
			Assert.IsNotNull(result);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
		}
		#endregion

		#region Coverage Tests
		/// <summary>
		/// Tbl_DimOffice Create/Dispose Test
		/// </summary>
		[TestMethod]
		public void API_Office_Create_Dispose_Works()
		{
			// Arrange
			// Act
			controller.Dispose();

			// Assert
			Assert.IsNotNull(controller, "Controller dispose failed!");
		}

		/// <summary>
		/// Create/Dispose Tbl_DimOffice Test
		/// </summary>
		[TestMethod]
		public void API_Office_Create_NoParm_Works()
		{
			//// Arrange
			var controller = new OfficeController();

			//// Act
			controller.Dispose();

			//// Assert
			Assert.IsNotNull(controller, "Controller dispose failed!");
		}

		/// <summary>
		/// Create/Dispose Tbl_DimOffice Test
		/// </summary>
		[TestMethod]
		public void API_Office_Create_Repository_Works()
		{
			//// Arrange
			BuildTbl_DimOfficeRepository();
			var controller = new OfficeController(repository);

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
			BuildTbl_DimOfficeRepository();
			BuildTbl_DimOfficeController();
		}

		/// <summary>
		/// Build Repository
		/// </summary>
		public void BuildTbl_DimOfficeRepository()
		{
			if (repository == null)
			{
				CreateTestData();
				repository = new Tbl_DimOfficeRepository(dbContext);
			}
		}

		/// <summary>
		/// Build Controller
		/// </summary>
		public void BuildTbl_DimOfficeController()
		{
			if (controller == null)
			{
				if (repository == null)
				{
					BuildTbl_DimOfficeRepository();
				}
				//controller = new OfficeController(dbContext, repository)
				//controller = new OfficeController(repository)
				controller = new OfficeController(dbContext)
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
		/// Builds the Tbl_DimOffice Controller with a mock repository
		/// </summary>
		/// <param name="mockRepo">The mock repository</param>
		/// <returns>Mocked Controller</returns>
		public OfficeController BuildMockingTbl_DimOfficeController(ITbl_DimOfficeRepository mockRepo)
		{
			var controller = new OfficeController(mockRepo)
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
