//-----------------------------------------------------------------------
// <copyright file="Tbl_DimRoomGrid.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimRoom Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimRoom Grid Records
    /// </summary>
    public class Tbl_DimRoomGrid
    {
        /// <summary>
        /// Total Record Count
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int Total { get; set; }

        /// <summary>
        /// Data Records
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public List<Tbl_DimRoom> Data { get; set; }

        /// <summary>
        /// Return Message
        /// </summary>
        public string ReturnMessageTxt { get; set; }

        /// <summary>
        /// Initialize Model
        /// </summary>
        public Tbl_DimRoomGrid()
        {
            Total = 0;
        }

        /// <summary>
        /// Initialize Model
        /// </summary>
        /// <param name="data">Data Records</param>
        /// <param name="count">Total Record Count</param>
        public Tbl_DimRoomGrid(List<Tbl_DimRoom> data, int count)
        {
            Data = data;
            Total = count;
        }
    }
}        
