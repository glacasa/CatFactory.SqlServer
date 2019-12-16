﻿using System.Diagnostics;

namespace CatFactory.SqlServer.DatabaseObjectModel
{
    /// <summary>
    /// Represents a row Guid column
    /// </summary>
    [DebuggerDisplay("Name={Name}")]
    public class RowGuidCol
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of <see cref="RowGuidCol"/> class
        /// </summary>
        public RowGuidCol()
        {
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the column's name
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
