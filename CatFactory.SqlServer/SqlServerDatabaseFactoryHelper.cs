﻿using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DocumentObjectModel.Queries;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Contains helper methods to import feature
    /// </summary>
    public static class SqlServerDatabaseFactoryHelper
    {
        /// <summary>
        /// Adds user defined data types for database
        /// </summary>
        /// <param name="database">Instance of <see cref="Database"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        public static void AddUserDefinedDataTypes(Database database, DbConnection connection)
        {
            var sysTypes = connection.GetSysTypes().ToList();

            foreach (var type in sysTypes)
            {
                if (type.IsUserDefined == false)
                    continue;

                var parent = sysTypes.FirstOrDefault(item => item.IsUserDefined == false && item.SystemTypeId == type.SystemTypeId);

                if (parent == null)
                    continue;

                database.DatabaseTypeMaps.Add(new DatabaseTypeMap
                {
                    DatabaseType = type.Name,
                    Collation = type.CollationName,
                    IsUserDefined = (bool)type.IsUserDefined,
                    ParentDatabaseType = parent.Name
                });
            }
        }

        /// <summary>
        /// Gets the column names from data reader
        /// </summary>
        /// <param name="dataReader">Instance of <see cref="DbDataReader"/> class</param>
        /// <returns>A sequence of <see cref="string"/> that contains column names</returns>
        public static IEnumerable<string> GetNames(DbDataReader dataReader)
        {
            if (dataReader.HasRows)
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                    yield return dataReader.GetName(i);
            }
        }

        /// <summary>
        /// Gets a column from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Column"/> class</returns>
        public static Column GetColumn(IDictionary<string, object> dictionary)
        {
            var column = new Column
            {
                Name = string.Concat(dictionary["Column_name"])
            };

            column.Type = string.Concat(dictionary["Type"]);
            column.Computed = string.Concat(dictionary["Computed"]);
            column.Length = int.Parse(string.Concat(dictionary["Length"]));
            column.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dictionary["Prec"]));
            column.Scale = string.Concat(dictionary["Scale"]).Trim().Length == 0 ? default(short) : short.Parse(string.Concat(dictionary["Scale"]));
            column.Nullable = string.Compare(string.Concat(dictionary["Nullable"]), "yes", true) == 0 ? true : false;
            column.TrimTrailingBlanks = string.Concat(dictionary["TrimTrailingBlanks"]);
            column.FixedLenNullInSource = string.Concat(dictionary["FixedLenNullInSource"]);
            column.Collation = string.Concat(dictionary["Collation"]);

            return column;
        }

        /// <summary>
        /// Gets a parameter from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Parameter"/> class</returns>
        public static Parameter GetParameter(IDictionary<string, object> dictionary)
        {
            var parameter = new Parameter
            {
                Name = string.Concat(dictionary["Parameter_name"])
            };

            parameter.Type = string.Concat(dictionary["Type"]);
            parameter.Length = short.Parse(string.Concat(dictionary["Length"]));
            parameter.Prec = string.Concat(dictionary["Prec"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dictionary["Prec"]));
            parameter.Order = string.Concat(dictionary["Param_order"]).Trim().Length == 0 ? default(int) : int.Parse(string.Concat(dictionary["Param_order"]));
            parameter.Collation = string.Concat(dictionary["Collation"]);

            return parameter;
        }

        /// <summary>
        /// Gets an index from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="Index"/> class</returns>
        public static Index GetIndex(IDictionary<string, object> dictionary)
            => new Index
            {
                IndexName = string.Concat(dictionary["index_name"]),
                IndexDescription = string.Concat(dictionary["index_description"]),
                IndexKeys = string.Concat(dictionary["index_keys"])
            };

        /// <summary>
        /// Gets a constraint detail from row dictionary
        /// </summary>
        /// <param name="dictionary">Dictionary from data reader</param>
        /// <returns>An instance of <see cref="ConstraintDetail"/> class</returns>
        public static ConstraintDetail GetConstraintDetail(IDictionary<string, object> dictionary)
            => new ConstraintDetail
            {
                ConstraintType = string.Concat(dictionary["constraint_type"]),
                ConstraintName = string.Concat(dictionary["constraint_name"]),
                DeleteAction = string.Concat(dictionary["delete_action"]),
                UpdateAction = string.Concat(dictionary["update_action"]),
                StatusEnabled = string.Concat(dictionary["status_enabled"]),
                StatusForReplication = string.Concat(dictionary["status_for_replication"]),
                ConstraintKeys = string.Concat(dictionary["constraint_keys"])
            };

        /// <summary>
        /// Gets the first result sets for stored procedure
        /// </summary>
        /// <param name="storedProcedure">Instance of <see cref="StoredProcedure"/> class</param>
        /// <param name="connection">Instance of <see cref="DbConnection"/> class</param>
        /// <returns>A sequence of <see cref="FirstResultSetForObject"/> class</returns>
        public static IEnumerable<FirstResultSetForObject> GetFirstResultSetForObject(StoredProcedure storedProcedure, DbConnection connection)
        {
            foreach (var item in connection.DmExecDescribeFirstResultSetForObject(storedProcedure.FullName))
            {
                yield return new FirstResultSetForObject
                {
                    ColumnOrdinal = item.ColumnOrdinal,
                    Name = item.Name,
                    IsNullable = item.IsNullable,
                    SystemTypeName = item.SystemTypeName
                };
            }
        }
    }
}
