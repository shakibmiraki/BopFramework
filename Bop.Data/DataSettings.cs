﻿using System.Collections.Generic;
using FluentMigrator.Runner.Initialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bop.Data
{
    /// <summary>
    /// Represents the data settings
    /// </summary>
    public partial class DataSettings : IConnectionStringAccessor
    {
        #region Ctor

        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a connection string
        /// </summary>
        [JsonProperty(PropertyName = "DataConnectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a data provider
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DataProviderType DataProvider { get; set; }

        /// <summary>
        /// Gets or sets a raw settings
        /// </summary>
        public IDictionary<string, string> RawDataSettings { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the information is entered
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public bool IsValid => DataProvider != DataProviderType.Unknown && !string.IsNullOrEmpty(ConnectionString);

        /// <summary>
        /// Gets or sets a is database installed
        /// </summary>
        public bool IsDatabaseInstalled { get; set; }

        /// <summary>
        /// Gets or sets a phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a collation
        /// </summary>
        public string Collation { get; set; }
        

        #endregion
    }
}