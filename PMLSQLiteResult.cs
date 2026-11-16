using System.Collections;
using System.Data.SQLite;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
    /// <summary>
    /// Provides methods for reading the result sets of the statement executed.
    /// </summary>
    [PMLNetCallable()]
    public class PMLSQLiteResult
    {
        private SQLiteDataReader _dr;

        [PMLNetCallable()]
        public PMLSQLiteResult()
        {
        }

        public PMLSQLiteResult(SQLiteDataReader dr)
        {
            _dr = dr;
        }

        [PMLNetCallable()]
        public void Assign(PMLSQLiteResult that)
        {
            _dr?.Dispose();
            _dr = that._dr;
        }

        /// <summary>
        /// Closes the result.
        /// </summary>
        [PMLNetCallable()]
        public void Close()
        {
            _dr.Close();
        }

        /// <summary>
        /// Returns if the result sets is closed.
        /// </summary>
        /// <returns>true if the result is closed, false otherwise</returns>
        [PMLNetCallable()]
        public bool IsClosed()
        {
            return _dr.IsClosed;
        }

        /// <summary>
        /// Moves to the next result set in multiple result sets.
        /// </summary>
        /// <returns>true if the method was successful, false otherwise</returns>
        [PMLNetCallable()]
        public bool NextResult()
        {
            return _dr.NextResult();
        }

        /// <summary>
        /// Returns if the result set has any rows.
        /// </summary>
        /// <returns>true if the result has any rows, false otherwise</returns>
        [PMLNetCallable()]
        public bool HasRows()
        {
            return _dr.HasRows;
        }

        /// <summary>
        /// Returns the column names in the result set.
        /// </summary>
        /// <returns>The column names</returns>
        [PMLNetCallable()]
        public Hashtable ColumnNames()
        {
            var names = new Hashtable();
            for (int i = 0; i < _dr.FieldCount; i++)
            {
                names.Add(i + 1.0, _dr.GetName(i));
            }
            return names;
        }

        /// <summary>
        /// Returns the type names of the columns in the result set.
        /// </summary>
        /// <returns>The type names</returns>
        [PMLNetCallable()]
        public Hashtable ColumnTypes()
        {
            var types = new Hashtable();
            for (int i = 0; i < _dr.FieldCount; i++)
            {
                types.Add(i + 1.0, _dr.GetDataTypeName(i));
            }
            return types;
        }

        /// <summary>
        /// Reads the next row from the result set.
        /// </summary>
        /// <returns>true if the method was successful, false otherwise</returns>
        [PMLNetCallable()]
        public bool Read()
        {
            return _dr.Read();
        }

        /// <summary>
        /// Fetches the value in the specified column name.
        /// </summary>
        /// <param name="name">Column name</param>
        /// <returns>The value in the column</returns>
        [PMLNetCallable()]
        public string Fetch(string name)
        {
            return _dr[name].ToString();
        }

        /// <summary>
        /// Fetches the values in the current row.
        /// </summary>
        /// <returns>The values in the current row</returns>
        [PMLNetCallable()]
        public Hashtable FetchRow()
        {
            var row = new Hashtable();
            for (int i = 0; i < _dr.FieldCount; i++)
            {
                if (_dr[i] is double v)
                {
                    row.Add(i + 1.0, v);
                }
                else
                {
                    row.Add(i + 1.0, _dr[i].ToString());
                }
            }
            return row;
        }
    }
}