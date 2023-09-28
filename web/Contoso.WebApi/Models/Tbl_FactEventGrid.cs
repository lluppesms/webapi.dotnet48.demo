//-----------------------------------------------------------------------
// <copyright file="Tbl_FactEventGrid.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Tbl_FactEvent Grid
// </summary>
//-----------------------------------------------------------------------

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Tbl_FactEvent Grid Records
    /// </summary>
    public class Tbl_FactEventGrid
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
        public List<Tbl_FactEvent> Data { get; set; }

        /// <summary>
        /// Return Message
        /// </summary>
        public string ReturnMessageTxt { get; set; }

        /// <summary>
        /// Initialize Model
        /// </summary>
        public Tbl_FactEventGrid()
        {
            Total = 0;
        }

        /// <summary>
        /// Initialize Model
        /// </summary>
        /// <param name="data">Data Records</param>
        /// <param name="count">Total Record Count</param>
        public Tbl_FactEventGrid(List<Tbl_FactEvent> data, int count)
        {
            Data = data;
            Total = count;
        }
    }
}        
