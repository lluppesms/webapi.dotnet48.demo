//-----------------------------------------------------------------------
// <copyright file="EffortProviderFactory.cs" company="Contoso, Inc.">
// Copyright 2023, Contoso, Inc. All rights reserved.
// </copyright>
// <summary>
// Database Provider for Effort Tests
// </summary>
//-----------------------------------------------------------------------

using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace Contoso.WebApi.Tests
{
    public class EffortProviderFactory : IDbConnectionFactory
    {
        private static DbConnection _connection;
        private readonly static object _lock = new object();

        public static void ResetDb()
        {
            lock (_lock)
            {
                _connection = null;
            }
        }

        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            lock (_lock)
            {
                if (_connection == null)
                {
                    _connection = Effort.DbConnectionFactory.CreateTransient();
                }

                return _connection;
            }
        }
    }
}