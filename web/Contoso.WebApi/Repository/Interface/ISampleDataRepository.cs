//-----------------------------------------------------------------------
// <copyright file="ISampleDataRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// SampleDataRepository Interface
// </summary>
//-----------------------------------------------------------------------

using System.Threading.Tasks;

namespace Contoso.WebApi.Data
{
    /// <summary>
    /// SampleDataRepository Interface
    /// </summary>
    public interface ISampleDataRepository
    {
        /// <summary>
        /// Populate Sample Data
        /// </summary>
        Task<(bool result, string message)> PopulateSampleData(string requestingUserName, bool forceReset = false);

#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        /// <summary>
        /// Disposal
        /// </summary>
        void Dispose();
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
    }
}
