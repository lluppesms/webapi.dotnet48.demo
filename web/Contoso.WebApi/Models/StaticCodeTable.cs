//-----------------------------------------------------------------------
// <copyright file="StaticCodeTable.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Static Code Tables
// </summary>
//-----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
using System.Diagnostics.CodeAnalysis;

namespace Contoso.WebApi.Data
{
    /// <summary>
    /// Code Table of strings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StaticCodeTable
	{
		/// <summary>
		/// Initialize Code Table Record
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="dscr">Description</param>
		public StaticCodeTable(string value, string dscr)
		{
			CodeValue = value;
			CodeDscr = dscr;
		}

		/// <summary>
		/// Value
		/// </summary>
		public string CodeValue { get; set; }

		/// <summary>
		/// Description
		/// </summary>
		public string CodeDscr { get; set; }
	}

    /// <summary>
    /// Code Table of numbers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StaticNumberTable
	{
		/// <summary>
		/// Initialize Code Table Record
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="dscr">Description</param>
		public StaticNumberTable(int value, int dscr)
		{
			CodeValue = value;
			CodeDscr = dscr;
		}

		/// <summary>
		/// Value
		/// </summary>
		public int CodeValue { get; set; }

		/// <summary>
		/// Description
		/// </summary>
		public int CodeDscr { get; set; }
	}

    /// <summary>
    /// Code Table of bytes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class StaticByteTable
	{
		/// <summary>
		/// Initialize Code Table Record
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="dscr">Description</param>
		public StaticByteTable(byte value, byte dscr)
		{
			CodeValue = value;
			CodeDscr = dscr;
		}

		/// <summary>
		/// Value
		/// </summary>
		public byte CodeValue { get; set; }

		/// <summary>
		/// Description
		/// </summary>
		public byte CodeDscr { get; set; }
	}
}