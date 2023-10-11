//-----------------------------------------------------------------------
// <copyright file="Tbl_DimRoom.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimRoom Table
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimRoom Table
    /// </summary>
    [Table("Tbl_DimRoom")]
    [ExcludeFromCodeCoverage]
    public class Tbl_DimRoom
    {
        /// <summary>
        /// Room ID
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Room ID is required")]
        [Display(Name = "Room ID", Description = "This is the Room ID field.", Prompt = "Enter Room ID")]
        [JsonProperty("id")]
        public int RoomID { get; set; }

        /// <summary>
        /// Office ID
        /// </summary>
        [Display(Name = "Office ID", Description = "This is the Office ID field.", Prompt = "Enter Office ID")]
        [JsonProperty("officeId")]
        public int? OfficeID { get; set; }

        /// <summary>
        /// Room Name
        /// </summary>
        [Required(ErrorMessage = "Room Name is required")]
        [Display(Name = "Room Name", Description = "This is the Room Name field.", Prompt = "Enter Room Name")]
        [StringLength(255)]
        [JsonProperty("name")]
        public string RoomName { get; set; }
    }
}
