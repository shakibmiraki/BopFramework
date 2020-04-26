using System;
using System.Globalization;
using FluentMigrator;

namespace Bop.Data.Migrations
{
    /// <summary>
    /// Attribute for a migration
    /// </summary>
    public partial class BopMigrationAttribute : MigrationAttribute
    {
        private static readonly string[] _dateFormats = { "yyyy-MM-dd hh:mm:ss", "yyyy.MM.dd hh:mm:ss", "yyyy/MM/dd hh:mm:ss", "yyyy-MM-dd hh:mm:ss:fffffff", "yyyy.MM.dd hh:mm:ss:fffffff", "yyyy/MM/dd hh:mm:ss:fffffff" };

        /// <summary>
        /// Initializes a new instance of the NopMigrationAttribute class
        /// </summary>
        /// <param name="dateTime">The migration date time string to convert on version</param>
        public BopMigrationAttribute(string dateTime) :
            base(DateTime.ParseExact(dateTime, _dateFormats, CultureInfo.InvariantCulture).Ticks, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NopMigrationAttribute class
        /// </summary>
        /// <param name="dateTime">The migration date time string to convert on version</param>
        /// <param name="description">The migration description</param>
        public BopMigrationAttribute(string dateTime, string description) :
            base(DateTime.ParseExact(dateTime, _dateFormats, CultureInfo.InvariantCulture).Ticks, description)
        {
        }
    }
}
