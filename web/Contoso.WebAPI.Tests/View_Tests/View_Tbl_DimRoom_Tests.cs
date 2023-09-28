////-----------------------------------------------------------------------
//// <copyright file="View_Tbl_DimRoom_Tests.cs" company="Luppes Consulting, Inc.">
//// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
//// </copyright>
//// <summary>
//// Tbl_DimRoom View Controller Tests
//// </summary>
////-----------------------------------------------------------------------

//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Text;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Contoso.WebApi.Controllers;
//using Contoso.WebApi.Data;
//using Contoso.WebApi.SampleData;

//// ReSharper disable once CheckNamespace
//namespace Contoso.WebApi.UnitTests.Views
//{
//    /// <summary>
//    /// Tbl_DimRoom Controller Test
//    /// </summary>
//    [ExcludeFromCodeCoverage]
//    [TestClass]
//    public class Tbl_DimRoomControllerEffortTest : BaseEffortTestController
//    {
//        #region Variables
//        /// <summary>
//        /// Repository
//        /// </summary>
//        private Tbl_DimRoomRepository repository;

//        /// <summary>
//        /// The controller
//        /// </summary>
//        private Tbl_DimRoomController controller;

//        /// <summary>
//        /// Runs when test is initialized to set up context and sample data
//        /// </summary>
//        [TestInitialize]
//        public void SetupTestData()
//        {
//            BuildControllerWithMockRequestObject();
//        }

//        /// <summary>
//        /// Creates the test data
//        /// </summary>
//        public void CreateTestData()
//        {
//            CreateDbContextIfNeeded();
//            SampleDataManager.CreateSampleData(DbContext);
//        }
//        #endregion

//        #region Index/Detail View Tests
//        /// <summary>
//        /// Index View Test
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Index_Works()
//        {
//            // Arrange

//            // Act
//            ViewResult result = controller.Index() as ViewResult;

//            // Assert
//            Assert.IsNotNull(result);
//        }

//        /// <summary>
//        /// Details View Test
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Details_NotFound_Fails()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom();
//            var expectedHttpStatus = 404;

//            // Act
//            var result = controller.Details(-1) as HttpStatusCodeResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.StatusCode.Equals(expectedHttpStatus), "Controller did not return the expected status code!");
//        }

//        /// <summary>
//        /// Details View Test
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Details_Works()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom();

//            // Act
//            var result = controller.Details(SampleDataManager.TestTbl_DimRoom[0].RoomID) as ViewResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsNotNull(result.Model, "Controller did not return a record!");
//            Assert.IsTrue(result.Model.GetType() == expectedData.GetType(), "Controller did not return the expected records!");
//        }
//        #endregion

//        #region Create View Tests
//        /// <summary>
//        /// Create Tbl_DimRoom GET Test
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Create_Get_Works()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom();

//            // Act
//            var result = controller.Create() as ViewResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.Model.GetType() == expectedData.GetType(), "Controller did not return the expected records!");
//        }

//        /// <summary>
//        /// Create Tbl_DimRoom POST Duplicate Fails
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Create_Post_Fails_Duplicate()
//        {
//            // Arrange
//            var record1 = new Tbl_DimRoom { RoomID = 89, RoomName = "Unit Test 89" };
//            var record2 = new Tbl_DimRoom { RoomID = 90, RoomName = record1.RoomName };

//            // Act
//            try
//            {
//                var resultInitialAdd = controller.Create(record1) as ActionResult;
//                Assert.IsNotNull(resultInitialAdd, "Controller did not return anything!");

//                var result = controller.Create(record2) as ActionResult;

//                // Assert
//                Assert.IsNotNull(result, "Controller did not return anything!");
//                Assert.IsTrue(result.GetType().Name != "RedirectToRouteResult", "Controller returned a redirect action!");
//                var viewResult = (ViewResult)result;
//                var modelStateMessages = GetModelStateMessage(viewResult.ViewData.ModelState);
//                Assert.IsTrue(!string.IsNullOrEmpty(modelStateMessages), "This test should return a validation error and it didn't!");
//            }
//            catch (Exception ex)
//            {
//                var errorMessage = GetExceptionMessage(ex);
//                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error: " + errorMessage);
//            }
//        }

//        /// <summary>
//        /// Create Tbl_DimRoom POST Works
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Create_Post_Works()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom { RoomID = 88, RoomName = "Unit Test 88" };

//            // Act
//            try
//            {
//                var result = controller.Create(expectedData);

//                // Assert
//                Assert.IsNotNull(result, "Controller did not return anything!");
//                Assert.IsTrue(result.GetType().Name == "RedirectToRouteResult", "Controller did not return a redirect action!");
//                Assert.IsTrue(((RedirectToRouteResult)result).RouteValues["action"].ToString() == "Index", "Controller did not redirect to the Index page!");
//            }
//            catch (Exception ex)
//            {
//                var errorMessage = GetExceptionMessage(ex);
//                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error: " + errorMessage);
//            }
//        }
//        #endregion

//        #region Edit View Tests
//        /// <summary>
//        /// Edit Tbl_DimRoom GET Works
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Edit_Get_Works()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom();

//            // Act
//            var result = controller.Edit(SampleDataManager.TestTbl_DimRoom[0].RoomID) as ViewResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.Model.GetType() == expectedData.GetType(), "Controller did not return the expected records!");
//        }

//        /// <summary>
//        /// Edit Tbl_DimRoom GET Not Found Fails
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Edit_NotFound_Fails()
//        {
//            // Arrange
//            var expectedHttpStatus = 404;

//            // Act
//            var result = controller.Edit(-1) as HttpStatusCodeResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.StatusCode.Equals(expectedHttpStatus), "Controller did not return the expected status code!");
//        }

//        /// <summary>
//        /// Edit Tbl_DimRoom POST Works
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Edit_Post_Works()
//        {
//            // Arrange
//            var errorMessage = string.Empty;
//            var id = 1;
//            var expectedData = SampleDataManager.TestTbl_DimRoom[0];
//            expectedData.RoomName = "UnitTest 1";

//            // Act
//            var result = controller.Edit(id, expectedData);

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.GetType().Name == "RedirectToRouteResult", "Controller returned a redirect action!");
//            Assert.IsTrue(((RedirectToRouteResult)result).RouteValues["action"].ToString() == "Details", "Controller did not redirect to the Details page!");
//        }
//        #endregion

//        #region Delete View Tests
//        /// <summary>
//        /// Delete Tbl_DimRoom GET Works
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Delete_Get_Works()
//        {
//            // Arrange
//            var expectedData = new Tbl_DimRoom();

//            // Act
//            var result = controller.Delete(SampleDataManager.TestTbl_DimRoom[0].RoomID) as ViewResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.Model.GetType() == expectedData.GetType(), "Controller did not return the expected records!");
//        }

//        /// <summary>
//        /// Delete Tbl_DimRoom GET Not Found Fails
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Delete_NotFound_Fails()
//        {
//            // Arrange
//            var expectedHttpStatus = 404;

//            // Act
//            var result = controller.Delete(-1) as HttpStatusCodeResult;

//            // Assert
//            Assert.IsNotNull(result, "Controller did not return anything!");
//            Assert.IsTrue(result.StatusCode.Equals(expectedHttpStatus), "Controller did not return the expected status code!");
//        }

//        /// <summary>
//        /// Delete Tbl_DimRoom POST Works
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Delete_Post_Works()
//        {
//            // Arrange
//            var data = new Tbl_DimRoom() { RoomID = 87, RoomName = "Unit Test 87" };

//            // Act
//            try
//            {
//                var resultsAdd = repository.Add(UserNameAdmin, data);
//                var resultFind = repository.FindOne(UserNameAdmin, data.RoomID);
//                var result = controller.Delete(resultFind.RoomID, resultFind);

//                // Assert
//                Assert.IsNotNull(result, "Controller did not return anything!");
//                Assert.IsTrue(result.GetType().Name == "RedirectToRouteResult", "Controller did not return a redirect action!");
//                Assert.IsTrue(((RedirectToRouteResult)result).RouteValues["action"].ToString() == "Index", "Controller did not redirect to the Index page!");
//            }
//            catch (Exception ex)
//            {
//                var errorMessage = GetExceptionMessage(ex);
//                Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error: " + errorMessage);
//            }
//        }
//        #endregion

//        #region Create/Dispose Coverage Test
//        /// <summary>
//        /// Tbl_DimRoom Create/Dispose Test
//        /// </summary>
//        [TestMethod]
//        public void View_Tbl_DimRoom_Create_Dispose_Works()
//        {
//            var errorMessage = string.Empty;

//            // Arrange
//            var controller = new Tbl_DimRoomController();

//            // Act
//            controller.Dispose();

//            // Assert
//            Assert.IsNotNull(controller, "Controller dispose failed!");
//        }
//        #endregion

//        #region Helper Functions
//        /// <summary>
//        /// Build Controller And Repository
//        /// </summary>
//        public void BuildControllerWithMockRequestObject()
//        {
//            BuildTbl_DimRoomRepository();
//            BuildTbl_DimRoomController();
//        }

//        /// <summary>
//        /// Build Repository
//        /// </summary>
//        public void BuildTbl_DimRoomRepository()
//        {
//            if (repository == null)
//            {
//                CreateTestData();
//                repository = new Tbl_DimRoomRepository(DbContext);
//            }
//        }

//        /// <summary>
//        /// Build Controller
//        /// </summary>
//        public void BuildTbl_DimRoomController()
//        {
//            if (controller == null)
//            {
//                BuildTbl_DimRoomRepository();
//                controller = new Tbl_DimRoomController(repository);
//                controller.ControllerContext = new ControllerContext(BuildMockHttpContext(), new RouteData(), controller);
//                AddUserIdentityClaims();
//            }
//        }
//        #endregion

//        #region Cleanup
//        /// <summary>
//        /// Disposal
//        /// </summary>
//        public void Dispose()
//        {
//            if (controller != null)
//            {
//                controller = null;
//            }
//            if (repository != null)
//            {
//                repository = null;
//            }
//            if (DbContext != null)
//            {
//                DbContext = null;
//            }
//        }
//        #endregion
//    }
//}
