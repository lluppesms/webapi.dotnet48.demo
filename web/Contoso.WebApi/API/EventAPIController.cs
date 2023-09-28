//-----------------------------------------------------------------------
// <copyright file="Tbl_FactEventController.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Event API Controller
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
	/// Event API Controller
	/// </summary>
	public class EventController : _BaseAPIController
    {
        #region Initialization
        /// <summary>
        /// Event Repository
        /// </summary>
        public ITbl_FactEventRepository Tbl_FactEventDb { get; private set; }

        /// <summary>
        /// Event API Controller
        /// </summary>
        public EventController() : this(new Tbl_FactEventRepository())
        {
        }

        /// <summary>
        /// Event API Controller
        /// </summary>
        /// <param name="repositoryTbl_FactEvent">Tbl_FactEvent Repository</param>
        public EventController(ITbl_FactEventRepository repositoryTbl_FactEvent)
        {
            Tbl_FactEventDb = repositoryTbl_FactEvent;
        }

		/// <summary>
		/// Event API Controller
		/// </summary>
		/// <param name="dbContext">The user tables data context.</param>
		public EventController(DatabaseEntities dbContext)
		{
            Tbl_FactEventDb = new Tbl_FactEventRepository(dbContext);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns>
        [Route("api/event/list")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var res = Tbl_FactEventDb.FindAll(GetUserName());
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed.");
        }

		/// <summary>
		/// Get Events for one Room
		/// </summary>
		/// <returns>Records</returns>
		[Route("api/event/room/{id:int}")]
		[HttpGet]
		public HttpResponseMessage GetEventsForOneRoom(int id)
		{
			var res = Tbl_FactEventDb.FindAllForOneRoom(GetUserName(), id);
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
		//[Route("api/EventGrid")]
		//[HttpPost]
		//public Tbl_FactEventGrid GetGrid()
		//{
		//    var request = HttpContext.Current.Request;
		//    var skip = CIntNull(request["skip"], 0);
		//    var take = CIntNull(request["take"], 0);
		//    var searchTxt = CStrNull(request["SearchTxt"]);
		//    return Tbl_FactEventDb.FindGridRecords(GetUserName(), searchTxt, skip, take);
		//}

		/// <summary>
		/// Get One Record
		/// </summary>
		/// <param name="id">Key</param>
		/// <returns>Record</returns>
		[Route("api/event/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetEvent(int id)
        {
            var res = Tbl_FactEventDb.FindOne(GetUserName(), id);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "Failed.");
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="tbl_FactEvent">Record</param>
        /// <returns>Response</returns>
        [Route("api/event/")]
        [HttpPost]
        public HttpResponseMessage PostEvent(Tbl_FactEvent tbl_FactEvent)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (tbl_FactEvent == null)
                {
                    response.Content = new StringContent("0");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (Tbl_FactEventDb.DupCheck(tbl_FactEvent.EventID, tbl_FactEvent.EventName, ref fieldName, ref errorMessage))
                {
                    response.Content = new StringContent(errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (Tbl_FactEventDb.Add(GetUserName(), tbl_FactEvent))
                {
                    response.Content = new StringContent(string.Format("{0}", tbl_FactEvent.EventID));
                    response.Headers.Location = new Uri(Request.RequestUri, string.Format("/api/Event/{0}", tbl_FactEvent.EventID));
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
        /// <param name="tbl_FactEvent">Record</param>
        /// <returns>Response</returns>
        [Route("api/event/")]
        [HttpPut]
        public HttpResponseMessage PutEvent(Tbl_FactEvent tbl_FactEvent)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (tbl_FactEvent != null && Tbl_FactEventDb.Save(GetUserName(), tbl_FactEvent.EventID, tbl_FactEvent))
                {
                    response.Content = new StringContent(string.Format("Updated Tbl_FactEvent {0}", tbl_FactEvent.EventID));
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Content = new StringContent("Update Tbl_FactEvent Failed!");
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
		/// Delete One Event
		/// </summary>
		/// <param name="id">Key</param>
		/// <returns>Response</returns>
		[Route("api/event/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int id)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                var tbl_FactEvent = Tbl_FactEventDb.FindOne(GetUserName(), id);
                if (tbl_FactEvent == null)
                {
                    response.Content = new StringContent("Delete Tbl_FactEvent Failed!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (Tbl_FactEventDb.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted Tbl_FactEvent {0}", tbl_FactEvent.EventID));
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Content = new StringContent("Delete Tbl_FactEvent Failed!");
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