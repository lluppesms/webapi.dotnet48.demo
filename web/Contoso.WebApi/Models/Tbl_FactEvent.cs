//-----------------------------------------------------------------------
// <copyright file="Tbl_FactEvent.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_FactEvent Table
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_FactEvent Table
    /// </summary>
    [Table("Tbl_FactEvent")]
    public class Tbl_FactEvent
    {
        /// <summary>
        /// Event ID
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Event ID is required")]
        [Display(Name = "Event ID", Description = "This is the Event ID field.", Prompt = "Enter Event ID")]
        [JsonProperty("id")]
        public int EventID { get; set; }

        /// <summary>
        /// Room ID
        /// </summary>
        [Display(Name = "Room ID", Description = "This is the Room ID field.", Prompt = "Enter Room ID")]
        [JsonProperty("roomId")]
        public int? RoomID { get; set; }

        /// <summary>
        /// Event Name
        /// </summary>
        [Required(ErrorMessage = "Event Name is required")]
        [Display(Name = "Event Name", Description = "This is the Event Name field.", Prompt = "Enter Event Name")]
        [StringLength(255)]
        [JsonProperty("name")]
        public string EventName { get; set; }

        /// <summary>
        /// Event Owner
        /// </summary>
        [Required(ErrorMessage = "Event Owner is required")]
        [Display(Name = "Event Owner", Description = "This is the Event Owner field.", Prompt = "Enter Event Owner")]
        [StringLength(255)]
        [JsonProperty("owner")]
        public string EventOwner { get; set; }

        /// <summary>
        /// Event Start Date Time
        /// </summary>
        [Required(ErrorMessage = "Event Start Date Time is required")]
        [Display(Name = "Event Start Date Time", Description = "This is the Event Start Date Time field.", Prompt = "Enter Event Start Date Time")]
        [DataType(DataType.DateTime)]
        [JsonProperty("startDateTime")]
        public DateTime EventStartDateTime { get; set; }

        /// <summary>
        /// Event End Date Time
        /// </summary>
        [Required(ErrorMessage = "Event End Date Time is required")]
        [Display(Name = "Event End Date Time", Description = "This is the Event End Date Time field.", Prompt = "Enter Event End Date Time")]
        [DataType(DataType.DateTime)]
        [JsonProperty("endDateTime")]
        public DateTime EventEndDateTime { get; set; }
    }
}
