//-----------------------------------------------------------------------
// <copyright file="SampleDataAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Sample Data API Controller
// </summary>
//-----------------------------------------------------------------------

using Contoso.WebApi.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contoso.WebApi.API
{
    [Authorize]
    // [AllowAnonymous]

    /// <summary>
    /// Sample Data API Controller
    /// </summary>
    public class SampleDataController : _BaseAPIController
    {
        #region Initialization
        /// <summary>
        /// Event Repository
        /// </summary>
        public ISampleDataRepository SampleDataRepo { get; private set; }

        /// <summary>
        /// Sample Data API Controller
        /// </summary>
        public SampleDataController() : this(new SampleDataRepository())
        {
        }

        /// <summary>
        /// Event API Controller
        /// </summary>
        /// <param name="repositoryTbl_FactEvent">Tbl_FactEvent Repository</param>
        public SampleDataController(ISampleDataRepository repositorySampleData)
        {
            SampleDataRepo = repositorySampleData;
        }

		/// <summary>
		/// Event API Controller
		/// </summary>
		/// <param name="dbContext">The user tables data context.</param>
		public SampleDataController(DatabaseEntities dbContext)
		{
            SampleDataRepo = new SampleDataRepository(dbContext);
        }
        #endregion

        /// <summary>
        /// Create sample data if none exists
        /// </summary>
        /// <returns>Result</returns>
        [Route("api/SampleData/Create")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var res = SampleDataRepo.PopulateSampleData(GetUserName(), false);
            if (res != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed.");
        }

        /// <summary>
        /// Create sample data - wipe out database if data exists!
        /// </summary>
        /// <returns>Result</returns>
        [Route("api/SampleData/Force")]
		[HttpGet]
		public HttpResponseMessage GetEventsForOneRoom()
		{
            var res = SampleDataRepo.PopulateSampleData(GetUserName(), true);
            if (res != null)
			{
				return Request.CreateResponse(HttpStatusCode.OK, res);
			}
			return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed.");
		}
	}
}