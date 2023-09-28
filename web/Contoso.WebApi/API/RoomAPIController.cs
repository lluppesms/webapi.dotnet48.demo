//-----------------------------------------------------------------------
// <copyright file="Tbl_DimRoomController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Room API Controller
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
	/// Room API Controller
	/// </summary>
	public class RoomController : _BaseAPIController
    {
        #region Initialization
        /// <summary>
        /// Room Repository
        /// </summary>
        public ITbl_DimRoomRepository Tbl_DimRoomDb { get; private set; }

        /// <summary>
        /// Room API Controller
        /// </summary>
        public RoomController() : this(new Tbl_DimRoomRepository())
        {
        }

        /// <summary>
        /// Room API Controller
        /// </summary>
        /// <param name="repositoryTbl_DimRoom">Tbl_DimRoom Repository</param>
        public RoomController(ITbl_DimRoomRepository repositoryTbl_DimRoom)
        {
            Tbl_DimRoomDb = repositoryTbl_DimRoom;
        }

		/// <summary>
		/// Room API Controller
		/// </summary>
		/// <param name="dbContext">The user tables data context.</param>
		public RoomController(DatabaseEntities dbContext)
		{
            Tbl_DimRoomDb = new Tbl_DimRoomRepository(dbContext);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns>
        [Route("api/room/List")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var res = Tbl_DimRoomDb.FindAll(GetUserName());
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
        //[Route("api/roomGrid")]
        //[HttpPost]
        //public Tbl_DimRoomGrid GetGrid()
        //{
        //    var request = HttpContext.Current.Request;
        //    var skip = CIntNull(request["skip"], 0);
        //    var take = CIntNull(request["take"], 0);
        //    var searchTxt = CStrNull(request["SearchTxt"]);
        //    return Tbl_DimRoomDb.FindGridRecords(GetUserName(), searchTxt, skip, take);
        //}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns>
        [Route("api/room/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetRoom(int id)
        {
            var res = Tbl_DimRoomDb.FindOne(GetUserName(), id);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Failed.");
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="tbl_DimRoom">Record</param>
        /// <returns>Response</returns>
        [Route("api/room/")]
        [HttpPost]
        public HttpResponseMessage PostRoom(Tbl_DimRoom tbl_DimRoom)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (tbl_DimRoom == null)
                {
                    response.Content = new StringContent("0");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (Tbl_DimRoomDb.DupCheck(tbl_DimRoom.RoomID, tbl_DimRoom.RoomName, ref fieldName, ref errorMessage))
                {
                    response.Content = new StringContent(errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (Tbl_DimRoomDb.Add(GetUserName(), tbl_DimRoom))
                {
                    response.Content = new StringContent(string.Format("{0}", tbl_DimRoom.RoomID));
                    response.Headers.Location = new Uri(Request.RequestUri, string.Format("/api/room/{0}", tbl_DimRoom.RoomID));
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
        /// <param name="tbl_DimRoom">Record</param>
        /// <returns>Response</returns>
        [Route("api/room/")]
        [HttpPut]
        public HttpResponseMessage PutRoom(Tbl_DimRoom tbl_DimRoom)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (tbl_DimRoom != null && Tbl_DimRoomDb.Save(GetUserName(), tbl_DimRoom.RoomID, tbl_DimRoom))
                {
                    response.Content = new StringContent(string.Format("Updated Tbl_DimRoom {0}", tbl_DimRoom.RoomID));
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Content = new StringContent("Update Tbl_DimRoom Failed!");
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
        [Route("api/room/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteRoom(int id)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                var tbl_DimRoom = Tbl_DimRoomDb.FindOne(GetUserName(), id);
                if (tbl_DimRoom == null)
                {
                    response.Content = new StringContent("Delete Tbl_DimRoom Failed!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (Tbl_DimRoomDb.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted Tbl_DimRoom {0}", tbl_DimRoom.RoomID));
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Content = new StringContent("Delete Tbl_DimRoom Failed!");
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