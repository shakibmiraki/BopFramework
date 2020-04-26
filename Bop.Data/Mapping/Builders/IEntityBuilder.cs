﻿using FluentMigrator.Builders.Create.Table;

namespace Bop.Data.Mapping.Builders
{
    /// <summary>
    /// Represents database entity builder
    /// </summary>
    public interface IEntityBuilder
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        void MapEntity(CreateTableExpressionBuilder table);

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }
    }
}
