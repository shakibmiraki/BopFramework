using Bop.Core;
using FluentMigrator.Builders.Create.Table;

namespace Bop.Data.Mapping.Builders
{
    /// <summary>
    /// Represents base entity builder
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class BopEntityBuilder<TEntity> : IEntityBuilder where TEntity : BaseEntity
    {
        /// <summary>
        /// Table name
        /// </summary>
        public string TableName => typeof(TEntity).Name;

        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public abstract void MapEntity(CreateTableExpressionBuilder table);
    }
}