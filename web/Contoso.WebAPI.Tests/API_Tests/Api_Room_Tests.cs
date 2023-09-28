//-----------------------------------------------------------------------
// <copyright file="Api_Tbl_DimRoom_Tests.cs" company="Contoso, Inc.">
//   Copyright © 2017 Contoso, Inc.
// </copyright>
// <summary>
//   Tbl_DimRoom API Test Cases
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
	/// Tbl_DimRoom API Test Class
	/// </summary>
	[ExcludeFromCodeCoverage]
    [TestClass]
    public class Api_Room_Tests : BaseEffortTestController
    {
        #region Variables
        /// <summary>
        /// The repository
        /// </summary>
        private Tbl_DimRoomRepository repository;

        /// <summary>
        /// The controller
        /// </summary>
        private RoomController controller;
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
		/// Tbl_DimRoom Get All Records works
		/// </summary>
		[TestMethod]
        public void API_Room_Get_All_Works()
        {
            // Arrange
            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK, "API did not return the expected HTTP Status!");

            var content = result.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<List<Tbl_DimRoom>>(content);
            Assert.IsTrue(data.Count >= SampleDataManager.Test_Tbl_DimRoom.Count, "API did not return the expected records!");
            Assert.IsNotNull(data.First().RoomName, "Data should not be null!");
        }

        ///// <summary>
        ///// Tbl_DimRoom Get All Grid Records works
        ///// </summary>
        //[TestMethod]
        //public void API_Room_Get_All_Grid_Works()
        //{
        //    // Arrange
        //    // Act
        //    var data = controller.GetGrid();

        //    // Assert
        //    Assert.IsNotNull(data);

        //    Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_DimRoom.Count, "API did not return the expected records!");
        //    Assert.IsNotNull(data.Data.First().RoomName, "Data should not be null!");
        //}

        /// <summary>
        /// Tbl_DimRoom Repository Grid Paging works
        /// </summary>
        [TestMethod]
        public void Repository_Room_Grid_Paging_Works()
        {
            // Arrange
            // Act
            var data = repository.FindGridRecords(UserNameAdmin, string.Empty, 1, 1);

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Total >= SampleDataManager.Test_Tbl_DimRoom.Count, "API did not return the expected records!");
        }

        /// <summary>
        /// Tbl_DimRoom Repository Grid Search works
        /// </summary>
        [TestMethod]
        public void Repository_Room_Grid_Search_Works()
        {
            // Arrange
            // Act
            var data = repository.FindGridRecords(UserNameAdmin, "e", 1, 1);

            // Assert
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Total >= 0, "API did not return the expected records!");
        }

        /// <summary>
        /// Tbl_DimRoom Get One Record Invalid Id fails
        /// </summary>
        [TestMethod]
        public void API_Room_Get_One_Missing_Fails()
        {
            // Arrange
            // Act
            var result = controller.GetRoom(-1);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "Invalid Status Code returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Get One Record works
        /// </summary>
        [TestMethod]
        public void API_Room_Get_One_Works()
        {
            // Arrange
            // Act
            var result = controller.GetRoom(SampleDataManager.Test_Tbl_DimRoom[0].RoomID);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned!");
            var content = result.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<Tbl_DimRoom>(content);
            Assert.IsNotNull(data.RoomName, "Data should not be null!");
        }
        #endregion

        #region Create (POST) Data Tests
        /// <summary>
        /// Tbl_DimRoom Create (POST) fails with bad data
        /// </summary>
        [TestMethod]
        public void API_Room_Post_Fails_BadData()
        {
            // Arrange
            var data = new Tbl_DimRoom();

            // Act
            var result = controller.PostRoom(data);

            // Assert
            Assert.IsNotNull(result, "API did not return anything!");
            Assert.AreEqual(HttpStatusCode.InternalServerError, result.StatusCode, "Invalid Status Code returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Create (POST) fails with empty data
        /// </summary>
        [TestMethod]
        public void API_Room_Post_Fails_EmptyData()
        {
            // Arrange
            var expectedMessage = "0";

            // Act
            var result = controller.PostRoom(null);

            // Assert
            Assert.IsNotNull(result, "API did not return anything!");
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");

            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Create (POST) fails with duplicate data
        /// </summary>
        [TestMethod]
        public void API_Room_Post_Fails_Duplicate()
        {
            // Arrange
            ////TO DO Validate in field values in new object
            var officeId = 1;
			var data = new Tbl_DimRoom { RoomID = 1, OfficeID = officeId };
            var expectedMessage = "This value already exists!";

            // Act
            var result = controller.PostRoom(data);

            // Assert
            Assert.IsNotNull(result, "API did not return anything!");
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Create (POST) Works with good data
        /// </summary>
        [TestMethod]
        public void API_Room_Post_Works()
        {
			// Arrange
			////TO DO Validate in field values in new object
			var officeId = 1;
			var data = new Tbl_DimRoom { RoomID = 99, RoomName = "Unit Test POST", OfficeID = officeId };

            // Act
            var result = controller.PostRoom(data);
            var resultDelete = controller.DeleteRoom(data.RoomID);

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
        /// Tbl_DimRoom Put fails with empty data
        /// </summary>
        [TestMethod]
        public void API_Room_Put_Fails_EmptyData()
        {
            // Arrange
            var expectedMessage = "Update Tbl_DimRoom Failed!";

            // Act
            var result = controller.PutRoom(null);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Update (PUT) fails with bad data
        /// </summary>
        [TestMethod]
        public void API_Room_Put_Fails_BadData()
        {
            // Arrange
            var data = new Tbl_DimRoom();
            var expectedMessage = "Update Tbl_DimRoom Failed!";

            // Act
            var result = controller.PutRoom(data);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode, "Invalid Status Code returned!");
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedMessage, content, "Invalid Error message returned!");
        }

        /// <summary>
        /// Tbl_DimRoom Update (PUT) Works with good data
        /// </summary>
        [TestMethod]
        public void API_Room_Put_Works()
        {
            // Arrange
            ////TO DO Validate in field values in new object
            var data = new Tbl_DimRoom { RoomID = SampleDataManager.Test_Tbl_DimRoom[0].RoomID, RoomName = SampleDataManager.Test_Tbl_DimRoom[0].RoomName };

            // Act
            data.RoomName = "Unit Test 2";
            var result = controller.PutRoom(data);

            // Assert
            var content = result.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "Invalid Status Code returned! " + content);
        }

        /// <summary>
        /// Tbl_DimRoom Repository Save works
        /// </summary>
        [TestMethod]
        public void Repository_Room_Save_Works()
        {
            // Arrange
            ////TO DO Validate in field values in new object
            var data = SampleDataManager.Test_Tbl_DimRoom[0];

            // Act
            var result = repository.Save(UserNameAdmin, data);

            // Assert
            Assert.AreEqual(true, result, "Invalid Status Code returned! ");
        }
        #endregion

        #region Delete Data Tests
        /// <summary>
        /// Tbl_DimRoom Delete fails with empty data
        /// </summary>
        [TestMethod]
        public void API_Room_Delete_Fails_MissingData()
        {
            // Arrange
            // Act
            var result = controller.DeleteRoom(-1);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode, "API did not return the expected HTTP Status!");
        }

        /// <summary>
        /// Tbl_DimRoom Delete Works with good data
        /// </summary>
        [TestMethod]
        public void API_Room_Delete_Works()
        {
			// Arrange
			////TO DO Validate in field values in new object
			var officeId = 1;
			var data = new Tbl_DimRoom { RoomID = 97, RoomName = "Unit Test DELETE", OfficeID = officeId };

            // Act
            var resultAdd = controller.PostRoom(data);
			var contentAdd = resultAdd.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.Created, resultAdd.StatusCode, "Add returned invalid Status Code! " + contentAdd);
            var result = controller.DeleteRoom(data.RoomID);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode, "API did not return the expected HTTP Status!");
        }

        /// <summary>
        /// Tbl_DimRoom Delete Check Works
        /// </summary>
        [TestMethod]
        public void API_Room_DeleteCheck_Works()
        {
            // Arrange
            var msg = string.Empty;

            // Act
            var result = repository.DeleteCheck(UserNameAdmin, SampleDataManager.Test_Tbl_DimRoom[0].RoomID, ref msg);

            // Assert
            Assert.AreEqual(true, result, "API did not return the expected HTTP Status!");
        }
        #endregion

        #region Exception Tests
        /// <summary>
        /// Grid Search throws exception
        /// </summary>
        [TestMethod]
        public void Repository_Room_Grid_Throws_Exception()
        {
            //// Arrange
            var mockRepo = new Tbl_DimRoomRepository(null);

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
        public void API_Room_Put_Throws_Exception()
        {
            //// Arrange
            var data = new Tbl_DimRoom { RoomID = SampleDataManager.Test_Tbl_DimRoom[0].RoomID, RoomName = SampleDataManager.Test_Tbl_DimRoom[0].RoomName };

            var mockRepo = MockRepository.GenerateStub<ITbl_DimRoomRepository>();
            mockRepo.Stub(x => x.Save(UserNameAdmin, data.RoomID, data)).Throw(new Exception("Yikes!"));

            var controller = BuildMockingRoomController(mockRepo);

            //// Act
            var result = controller.PutRoom(data);

            //// Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
        }

        /// <summary>
        /// Delete throws exception
        /// </summary>
        [TestMethod]
        public void API_Room_Delete_Throws_Exception()
        {
            //// Arrange
            var data = new Tbl_DimRoom { RoomID = SampleDataManager.Test_Tbl_DimRoom[0].RoomID, RoomName = SampleDataManager.Test_Tbl_DimRoom[0].RoomName };

            var mockRepo = MockRepository.GenerateStub<ITbl_DimRoomRepository>();
            mockRepo.Stub(x => x.FindOne(UserNameAdmin, data.RoomID)).Return(data);
            mockRepo.Stub(x => x.Delete(UserNameAdmin, data.RoomID)).Throw(new Exception("Yikes!"));

            var controller = BuildMockingRoomController(mockRepo);

            //// Act
            var result = controller.DeleteRoom(data.RoomID);

            //// Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == HttpStatusCode.InternalServerError, "API did not return the expected HTTP Status!");
        }
        #endregion

        #region Coverage Tests
        /// <summary>
        /// Tbl_DimRoom Create/Dispose Test
        /// </summary>
        [TestMethod]
        public void API_Room_Create_Dispose_Works()
        {
            // Arrange
            // Act
            controller.Dispose();

            // Assert
            Assert.IsNotNull(controller, "Controller dispose failed!");
        }

        /// <summary>
        /// Create/Dispose Tbl_DimRoom Test
        /// </summary>
        [TestMethod]
        public void API_Room_Create_NoParm_Works()
        {
            //// Arrange
            var controller = new RoomController();

            //// Act
            controller.Dispose();

            //// Assert
            Assert.IsNotNull(controller, "Controller dispose failed!");
        }

        /// <summary>
        /// Create/Dispose Tbl_DimRoom Test
        /// </summary>
        [TestMethod]
        public void API_Room_Create_Repository_Works()
        {
            //// Arrange
            BuildTbl_DimRoomRepository();
            var controller = new RoomController(repository);

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
            BuildTbl_DimRoomRepository();
            BuildRoomController();
        }

        /// <summary>
        /// Build Repository
        /// </summary>
        public void BuildTbl_DimRoomRepository()
        {
            if (repository == null)
            {
                CreateTestData();
                repository = new Tbl_DimRoomRepository(dbContext);
            }
        }

        /// <summary>
        /// Build Controller
        /// </summary>
        public void BuildRoomController()
        {
            if (controller == null)
            {
                if (repository == null)
                {
                    BuildTbl_DimRoomRepository();
                }
                //controller = new RoomController(DbContext, repository)
				controller = new RoomController(dbContext)
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
        /// Builds the Tbl_DimRoom Controller with a mock repository
        /// </summary>
        /// <param name="mockRepo">The mock repository</param>
        /// <returns>Mocked Controller</returns>
        public RoomController BuildMockingRoomController(ITbl_DimRoomRepository mockRepo)
        {
            var controller = new RoomController(mockRepo)
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
