//-----------------------------------------------------------------------
// <copyright file="OfficeVM.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// DimOffice View Models
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Office Details VM
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OfficeDetailsVM
    {
        /// <summary>
        /// Office ID
        /// </summary>
        [JsonProperty("id")]
        public int OfficeID { get; set; }

        /// <summary>
        /// Office Name
        /// </summary>
        [JsonProperty("name")]
        public string OfficeName { get; set; }

        /// <summary>
        /// Office Address
        /// </summary>
        [JsonProperty("address")]
        public string OfficeAddress { get; set; }

        /// <summary>
        /// Office City
        /// </summary>
        [JsonProperty("city")]
        public string OfficeCity { get; set; }

        /// <summary>
        /// Office State
        /// </summary>
        [JsonProperty("state")]
        public string OfficeState { get; set; }

        /// <summary>
        /// Office Zip
        /// </summary>
        [JsonProperty("zipcode")]
        public string OfficeZip { get; set; }

        /// <summary>
        /// Office Country
        /// </summary>
        [JsonProperty("country")]
        public string OfficeCountry { get; set; }

		/// <summary>
		/// Rooms List
		/// </summary>
		[JsonProperty("rooms")]
		public List<int> RoomList { get; set; }

        public OfficeDetailsVM()
        {
			RoomList = new List<int>();
		}

		public OfficeDetailsVM(Tbl_DimOffice office)
		{
            OfficeID = office.OfficeID;
            OfficeName = office.OfficeName;
            OfficeAddress = office.OfficeAddress;
            OfficeCity = office.OfficeCity;
            OfficeState = office.OfficeState;
            OfficeZip = office.OfficeZip;
            OfficeCountry = office.OfficeCountry;
			RoomList = new List<int>();
		}
		public OfficeDetailsVM(Tbl_DimOffice office, List<int> rooms)
		{
			OfficeID = office.OfficeID;
			OfficeName = office.OfficeName;
			OfficeAddress = office.OfficeAddress;
			OfficeCity = office.OfficeCity;
			OfficeState = office.OfficeState;
			OfficeZip = office.OfficeZip;
			OfficeCountry = office.OfficeCountry;
			RoomList = rooms;
		}
	}
}
