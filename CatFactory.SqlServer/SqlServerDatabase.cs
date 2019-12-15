﻿using System.Collections.Generic;
using System.Diagnostics;
using CatFactory.ObjectRelationalMapping;
using CatFactory.ObjectRelationalMapping.Programmability;
using CatFactory.SqlServer.DocumentObjectModel;

namespace CatFactory.SqlServer
{
    /// <summary>
    /// Represents the model for SQL Server databases
    /// </summary>
    public class SqlServerDatabase : Database, ISqlServerDatabase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<ExtendedProperty> m_extendedProperties;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<ScalarFunction> m_scalarFunctions;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<TableFunction> m_tableFunctions;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<StoredProcedure> m_storedProcedures;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Sequence> m_sequences;

        /// <summary>
        /// Initializes a new instance of <see cref="SqlServerDatabase"/> class
        /// </summary>
        public SqlServerDatabase()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the extended properties
        /// </summary>
        public List<ExtendedProperty> ExtendedProperties
        {
            get => m_extendedProperties ?? (m_extendedProperties = new List<ExtendedProperty>());
            set => m_extendedProperties = value;
        }

        /// <summary>
        /// Gets or sets the scalar functions
        /// </summary>
        public List<ScalarFunction> ScalarFunctions
        {
            get => m_scalarFunctions ?? (m_scalarFunctions = new List<ScalarFunction>());
            set => m_scalarFunctions = value;
        }

        /// <summary>
        /// Gets or sets the table functions
        /// </summary>
        public List<TableFunction> TableFunctions
        {
            get => m_tableFunctions ?? (m_tableFunctions = new List<TableFunction>());
            set => m_tableFunctions = value;
        }

        /// <summary>
        /// Gets or sets the store procedures
        /// </summary>
        public List<StoredProcedure> StoredProcedures
        {
            get => m_storedProcedures ?? (m_storedProcedures = new List<StoredProcedure>());
            set => m_storedProcedures = value;
        }

        /// <summary>
        /// Gets or sets the sequences
        /// </summary>
        public List<Sequence> Sequences
        {
            get => m_sequences ?? (m_sequences = new List<Sequence>());
            set => m_sequences = value;
        }
    }
}