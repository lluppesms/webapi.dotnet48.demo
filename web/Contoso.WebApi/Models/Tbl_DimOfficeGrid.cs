//-----------------------------------------------------------------------
// <copyright file="Tbl_DimOfficeGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_DimOffice Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_DimOffice Grid Records
    /// </summary>
    public class Tbl_DimOfficeGrid
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
        public List<Tbl_DimOffice> Data { get; set; }

        /// <summary>
        /// Return Message
        /// </summary>
        public string ReturnMessageTxt { get; set; }

        /// <summary>
        /// Initialize Model
        /// </summary>
        public Tbl_DimOfficeGrid()
        {
            Total = 0;
        }

        /// <summary>
        /// Initialize Model
        /// </summary>
        /// <param name="data">Data Records</param>
        /// <param name="count">Total Record Count</param>
        public Tbl_DimOfficeGrid(List<Tbl_DimOffice> data, int count)
        {
            Data = data;
            Total = count;
        }
    }
}
