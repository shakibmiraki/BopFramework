﻿using System.Collections.Generic;


namespace Bop.Web.Framework.Models
{
    /// <summary>
    /// Represents the base paged list model (implementation for KendoUI grids)
    /// </summary>
    public abstract partial class BasePagedListModel<T> : BaseModel
    {
        /// <summary>
        /// Gets or sets data records
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets total records number
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets an extra data
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// Gets or sets an errors
        /// </summary>
        public object Errors { get; set; }
    }
}
