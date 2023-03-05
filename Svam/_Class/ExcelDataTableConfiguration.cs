﻿using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam._Class
{
    public class ExcelDataTableConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating the prefix of generated column names.
        /// </summary>
        public string EmptyColumnNamePrefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a row from the data as column names.
        /// </summary>
        public bool UseHeaderRow { get; set; }

        /// <summary>
        /// Gets or sets a callback to determine which row is the header row. Only called when UseHeaderRow = true.
        /// </summary>
        public Action<IExcelDataReader> ReadHeaderRow { get; set; }

        /// <summary>
        /// Gets or sets a callback to determine whether to include the current row in the DataTable.
        /// </summary>
        public Func<IExcelDataReader, bool> FilterRow { get; set; }

        /// <summary>
        /// Gets or sets a callback to determine whether to include the specific column in the DataTable. Called once per column after reading the headers.
        /// </summary>
        public Func<IExcelDataReader, int, bool> FilterColumn { get; set; }
    }
}