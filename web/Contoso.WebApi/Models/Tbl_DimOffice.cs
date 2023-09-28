//-----------------------------------------------------------------------
// <copyright file="Tbl_DimOffice.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimOffice Table
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contoso.WebApi.Data
{
	/// <summary>
	/// Tbl_DimOffice Table
	/// </summary>
	[Table("Tbl_DimOffice")]
    public class Tbl_DimOffice
    {
        /// <summary>
        /// Office ID
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Office ID is required")]
        [Display(Name = "Office ID", Description = "This is the Office ID field.", Prompt = "Enter Office ID")]
        [JsonProperty("id")]
        public int OfficeID { get; set; }

        /// <summary>
        /// Office Name
        /// </summary>
        [Required(ErrorMessage = "Office Name is required")]
        [Display(Name = "Office Name", Description = "This is the Office Name field.", Prompt = "Enter Office Name")]
        [StringLength(255)]
        [JsonProperty("name")]
        public string OfficeName { get; set; }

        /// <summary>
        /// Office Address
        /// </summary>
        [Required(ErrorMessage = "Office Address is required")]
        [Display(Name = "Office Address", Description = "This is the Office Address field.", Prompt = "Enter Office Address")]
        [StringLength(255)]
        [JsonProperty("address")]
        public string OfficeAddress { get; set; }

        /// <summary>
        /// Office City
        /// </summary>
        [Required(ErrorMessage = "Office City is required")]
        [Display(Name = "Office City", Description = "This is the Office City field.", Prompt = "Enter Office City")]
        [StringLength(100)]
        [JsonProperty("city")]
        public string OfficeCity { get; set; }

        /// <summary>
        /// Office State
        /// </summary>
        [Required(ErrorMessage = "Office State is required")]
        [Display(Name = "Office State", Description = "This is the Office State field.", Prompt = "Enter Office State")]
        [StringLength(50)]
        [JsonProperty("state")]
        public string OfficeState { get; set; }

        /// <summary>
        /// Office Zip
        /// </summary>
        [Required(ErrorMessage = "Office Zip is required")]
        [Display(Name = "Office Zip", Description = "This is the Office Zip field.", Prompt = "Enter Office Zip")]
        [StringLength(10)]
        [JsonProperty("zipcode")]
        public string OfficeZip { get; set; }

        /// <summary>
        /// Office Country
        /// </summary>
        [Required(ErrorMessage = "Office Country is required")]
        [Display(Name = "Office Country", Description = "This is the Office Country field.", Prompt = "Enter Office Country")]
        [StringLength(100)]
        [JsonProperty("country")]
        public string OfficeCountry { get; set; }
    }
}
