//-----------------------------------------------------------------------
// <copyright file="Tbl_DimOfficeController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Office API Controller
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contoso.WebApi.API
{
	[Authorize]
	//[AllowAnonymous]

	/// <summary>
	/// Office API Controller
	/// </summary>
	public class OfficeController : _BaseAPIController
    {
        #region Initialization
        /// <summary>
        /// Office Repository
        /// </summary>
        public ITbl_DimOfficeRepository Tbl_DimOfficeDb { get; private set; }

        /// <summary>
        /// Office API Controller
        /// </summary>
        public OfficeController() : this(new Tbl_DimOfficeRepository())
        {
        }

        /// <summary>
        /// Office API Controller
        /// </summary>
        /// <param name="repositoryTbl_DimOffice">Tbl_DimOffice Repository</param>
        public OfficeController(ITbl_DimOfficeRepository repositoryTbl_DimOffice)
        {
            Tbl_DimOfficeDb = repositoryTbl_DimOffice;
        }

        /// <summary>
        /// Office API Controller
        /// </summary>
        /// <param name="dbContext">The user tables data context.</param>
        public OfficeController(DatabaseEntities dbContext)
        {
			Tbl_DimOfficeDb = new Tbl_DimOfficeRepository(dbContext);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns>
        [Route("api/office/list")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var res = Tbl_DimOfficeDb.FindAll(GetUserName());
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed.");
        }

        ///// <summary>
        ///// Get Paged List of Records for Grid
        ///// </summary>
        ///// <returns>Records</returns>
        //[Route("api/office/Grid")]
        //[HttpPost]
        //public Tbl_DimOfficeGrid GetGrid()
        //{
        //    var request = HttpContext.Current.Request;
        //    var skip = CIntNull(request["skip"], 0);
        //    var take = CIntNull(request["take"], 0);
        //    var searchTxt = CStrNull(request["SearchTxt"]);
        //    return Tbl_DimOfficeDb.FindGridRecords(GetUserName(), searchTxt, skip, take);
        //}

        /// <summary>
        /// Get One Office Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns>
        [Route("api/office/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetOffice(int id)
        {
            var res = Tbl_DimOfficeDb.FindOne(GetUserName(), id);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Failed.");
        }

		/// <summary>
		/// Get One Office Details
		/// </summary>
		/// <param name="id">Key</param>
		/// <returns>Record</returns>
		[Route("api/office/details/{id:int}")]
		[HttpGet]
		public HttpResponseMessage GetOfficeDetails(int id)
		{
			var res = Tbl_DimOfficeDb.FindOneDetails(GetUserName(), id);
			if (res != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, res);
			}
			return Request.CreateResponse(HttpStatusCode.NotFound, "Failed.");
		}

		/// <summary>
		/// Get One Office Rooms
		/// </summary>
		/// <param name="id">Key</param>
		/// <returns>Record</returns>
		[Route("api/office/rooms/{id:int}")]
		[HttpGet]
		public HttpResponseMessage GetOfficeRooms(int id)
		{
			var res = Tbl_DimOfficeDb.FindOneRooms(GetUserName(), id);
			if (res != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, res);
			}
			return Request.CreateResponse(HttpStatusCode.NotFound, "Failed.");
		}

		/// <summary>
		/// Post (Add) One Record
		/// </summary>
		/// <param name="tbl_DimOffice">Record</param>
		/// <returns>Response</returns>
		[Route("api/office/")]
        [HttpPost]
        public HttpResponseMessage PostOffice(Tbl_DimOffice tbl_DimOffice)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (tbl_DimOffice == null)
                {
                    response.Content = new StringContent("0");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                // ReSharper disable once PossibleNullReferenceException
                if (Tbl_DimOfficeDb.DupCheck(tbl_DimOffice.OfficeID, tbl_DimOffice.OfficeName, ref fieldName, ref errorMessage))
                {
                    response.Content = new StringContent(errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (Tbl_DimOfficeDb.Add(GetUserName(), tbl_DimOffice))
                {
                    response.Content = new StringContent(string.Format("{0}", tbl_DimOffice.OfficeID));
                    response.Headers.Location = new Uri(Request.RequestUri, string.Format("/api/Office/{0}", tbl_DimOffice.OfficeID));
                    response.StatusCode = HttpStatusCode.Created;
                }
                else
                {
                    response.Content = new StringContent("0");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Content = new StringContent(errorMessage);
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        /// <summary>
        /// Put (Update) One Record
        /// </summary>
        /// <param name="tbl_DimOffice">Record</param>
        /// <returns>Response</returns>
        [Route("api/office/")]
        [HttpPut]
        public HttpResponseMessage PutOffice(Tbl_DimOffice tbl_DimOffice)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (tbl_DimOffice != null && Tbl_DimOfficeDb.Save(GetUserName(), tbl_DimOffice.OfficeID, tbl_DimOffice))
                {
                    response.Content = new StringContent(string.Format("Updated Tbl_DimOffice {0}", tbl_DimOffice.OfficeID));
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Content = new StringContent("Update Tbl_DimOffice Failed!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Content = new StringContent(errorMessage);
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        /// <summary>
        /// Delete One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Response</returns>
        [Route("api/office/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteOffice(int id)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                var tbl_DimOffice = Tbl_DimOfficeDb.FindOne(GetUserName(), id);
                if (tbl_DimOffice == null)
                {
                    response.Content = new StringContent("Delete Tbl_DimOffice Failed!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (Tbl_DimOfficeDb.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted Tbl_DimOffice {0}", tbl_DimOffice.OfficeID));
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Content = new StringContent("Delete Tbl_DimOffice Failed!");
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Content = new StringContent(errorMessage);
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}