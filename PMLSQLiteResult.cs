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
        private SQLiteDataReader dr;

        [PMLNetCallable()]
        public PMLSQLiteResult()
        {
        }

        public PMLSQLiteResult(SQLiteDataReader r)
        {
            dr = r;
        }

        [PMLNetCallable()]
        public void Assign(PMLSQLiteResult that)
        {
            if (dr != null) dr.Dispose();
            dr = that.dr;
        }

        /// <summary>
        /// Closes the result.
        /// </summary>
        [PMLNetCallable()]
        public void Close()
        {
            dr.Close();
        }

        /// <summary>
        /// Returns if the result sets is closed.
        /// </summary>
        /// <returns>true if the result is closed, false otherwise</returns>
        [PMLNetCallable()]
        public bool IsClosed()
        {
            return dr.IsClosed;
        }

        /// <summary>
        /// Moves to the next result set in multiple result sets.
        /// </summary>
        /// <returns>true if the method was successful, false otherwise</returns>
        [PMLNetCallable()]
        public bool NextResult()
        {
            return dr.NextResult();
        }

        /// <summary>
        /// Returns if the result set has any rows.
        /// </summary>
        /// <returns>true if the result has any rows, false otherwise</returns>
        [PMLNetCallable()]
        public bool HasRows()
        {
            return dr.HasRows;
        }

        /// <summary>
        /// Returns the column names in the result set.
        /// </summary>
        /// <returns>The column names</returns>
        [PMLNetCallable()]
        public Hashtable ColumnNames()
        {
            var names = new Hashtable();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                double key = i + 1;
                names.Add(key, dr.GetName(i));
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
            for (int i = 0; i < dr.FieldCount; i++)
            {
                double key = i + 1;
                types.Add(key, dr.GetDataTypeName(i));
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
            return dr.Read();
        }

        /// <summary>
        /// Fetches the value in the specified column name.
        /// </summary>
        /// <param name="name">Column name</param>
        /// <returns>The value in the column</returns>
        [PMLNetCallable()]
        public string Fetch(string name)
        {
            return dr[name].ToString();
        }

        /// <summary>
        /// Fetches the values in the current row.
        /// </summary>
        /// <returns>The values in the current row</returns>
        [PMLNetCallable()]
        public Hashtable FetchRow()
        {
            var row = new Hashtable();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                double key = i + 1;
                if (dr[i] is double)
                {
                    row.Add(key, (double)dr[i]);
                }
                else
                {
                    row.Add(key, dr[i].ToString());
                }
            }
            return row;
        }
    }
}