﻿using System.Linq;
using CatFactory.ObjectRelationalMapping;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents a naming convention for database
    /// </summary>
    public class SqlServerDatabaseNamingConvention : IDatabaseNamingConvention
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SqlServerDatabaseNamingConvention"/> class
        /// </summary>
        public SqlServerDatabaseNamingConvention()
        {
        }

        /// <summary>
        /// Valids a name for current naming convention
        /// </summary>
        /// <param name="name">Object name</param>
        /// <returns>A string as a valid name</returns>
        public virtual string ValidName(string name)
            => name;

        /// <summary>
        /// Gets the name for object according to current naming convention
        /// </summary>
        /// <param name="names">Names</param>
        /// <returns>A string as object's name</returns>
        public virtual string GetObjectName(params string[] names)
            => string.Join(".", names.Select(item => string.Format("[{0}]", item)));

        /// <summary>
        /// Gets the name for parameter according to current naming convention
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>A string as parameter's name</returns>
        public virtual string GetParameterName(string name)
            => string.Format("@{0}", NamingConvention.GetCamelCase(name));

        /// <summary>
        /// Gets the name for primary key constraint
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="key">Key for constraint</param>
        /// <returns>A string as constraint's name</returns>
        public virtual string GetPrimaryKeyConstraintName(ITable table, string[] key)
            => string.Join("_", "PK", table.Schema, table.Name);

        /// <summary>
        /// Gets the name for unique constraint
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="key">Key for constraint</param>
        /// <returns>A string as constraint's name</returns>
        public virtual string GetUniqueConstraintName(ITable table, string[] key)
            => string.Join("_", "UQ", table.Schema, table.Name, string.Join("_", key));

        /// <summary>
        /// Gets the name for foreign key constraint
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="key">Key for constraint</param>
        /// <param name="references">References (Parent table)</param>
        /// <returns>A string as constraint's name</returns>
        public virtual string GetForeignKeyConstraintName(ITable table, string[] key, ITable references)
            => string.Join("_", "FK", table.Schema, table.Name, string.Join("_", key), references.Schema, references.Name);

        /// <summary>
        /// Gets the name for default constraint
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="key">Key for constraint</param>
        /// <returns>A string as constraint's name</returns>
        public virtual string GetDefaultConstraintName(ITable table, string key)
            => string.Join("_", "DF", table.Schema, table.Name, key);

        /// <summary>
        /// Gets the name for check constraint
        /// </summary>
        /// <param name="table">Table</param>
        /// <param name="key">Key for constraint</param>
        /// <returns>A string as constraint's name</returns>
        public virtual string GetCheckConstraintName(ITable table, string key)
            => string.Join("_", "CK", table.Schema, table.Name, key);
    }
}
