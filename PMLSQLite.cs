using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
    /// <summary>
    /// Represents a connection between PML and SQLite database.
    /// </summary>
    [PMLNetCallable()]
    public class PMLSQLite
    {
        private SQLiteConnection conn;

        [PMLNetCallable()]
        public PMLSQLite()
        {
            conn = new SQLiteConnection("");
        }

        /// <summary>
        /// Initializes a new instance with the specified connection string.
        /// </summary>
        /// <param name="constr">Connection string</param>
        [PMLNetCallable()]
        public PMLSQLite(string constr)
        {
            conn = new SQLiteConnection(constr);
        }

        [PMLNetCallable()]
        public void Assign(PMLSQLite that)
        {
            if (conn != null) conn.Dispose();
            conn = that.conn;
        }

        public override string ToString()
        {
            return conn.ConnectionString;
        }

        /// <summary>
        /// Returns the full path for the currently open database.
        /// </summary>
        /// <returns>The full path for the currently open database</returns>
        [PMLNetCallable()]
        public string FileName()
        {
            return conn.FileName;
        }

        /// <summary>
        /// Sets a filename for the data source. the environment variables in the filename are expanded.
        /// </summary>
        /// <param name="filename">Filename</param>
        [PMLNetCallable()]
        public void SetDataSource(string filename)
        {
            var csb = new SQLiteConnectionStringBuilder(conn.ConnectionString);
            csb.DataSource = Environment.ExpandEnvironmentVariables(filename);
            conn.ConnectionString = csb.ToString();
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        [PMLNetCallable()]
        public void Open()
        {
            conn.Open();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        [PMLNetCallable()]
        public void Close()
        {
            conn.Close();
        }

        /// <summary>
        /// Returns if the connection is open.
        /// </summary>
        /// <returns>true if the connection is openl, false otherwise</returns>
        [PMLNetCallable()]
        public bool IsOpen()
        {
            return (conn.State & ConnectionState.Open) == ConnectionState.Open;
        }

        /// <summary>
        /// Returns the version of the SQLite database engine.
        /// </summary>
        /// <returns>The version of the SQLite database engine</returns>
        [PMLNetCallable()]
        public string Version()
        {
            return conn.ServerVersion;
        }

        /// <summary>
        /// Backs up the database to another database.
        /// </summary>
        /// <param name="dest">Destination for backup</param>
        [PMLNetCallable()]
        public void Backup(PMLSQLite dest)
        {
            conn.BackupDatabase(dest.conn, dest.conn.Database, conn.Database, -1, null, -1);
        }

        /// <summary>
        /// Returns if the connection is in autocommit mode.
        /// </summary>
        /// <returns>true if the connection is in autocommit mode, false otherwise</returns>
        [PMLNetCallable()]
        public bool AutoCommit()
        {
            return conn.AutoCommit;
        }

        /// <summary>
        /// Returns the number of rows changed by the last INSERT, UPDATE, or DELETE statement.
        /// </summary>
        /// <returns>The number of rows changed by the last statement</returns>
        [PMLNetCallable()]
        public double Changes()
        {
            return conn.Changes;
        }

        /// <summary>
        /// Returns the rowid set by the last INSERT statement.
        /// </summary>
        /// <returns>The rowid set by the last INSERT statement</returns>
        [PMLNetCallable()]
        public double LastInsertRowId()
        {
            return conn.LastInsertRowId;
        }

        /// <summary>
        /// Returns a new <see cref="PMLSQLiteStatement"/> instance with the specified SQL statement.
        /// </summary>
        /// <param name="stmt">SQL statement</param>
        /// <returns>A new <see cref="PMLSQLiteStatement"/> instance</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement Prepare(string stmt)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = stmt;
            return new PMLSQLiteStatement(cmd);
        }

        /// <summary>
        /// Returns a new <see cref="PMLSQLiteStatement"/> instance with the specified SQL statement.
        /// </summary>
        /// <param name="stmt">PML array of SQL statement strings</param>
        /// <returns>A new <see cref="PMLSQLiteStatement"/> instance</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement Prepare(Hashtable stmt)
        {
            var sb = new StringBuilder();
            for (double i = 1; i <= stmt.Count; i++)
            {
                sb.Append(stmt[i]).Append("\r\n");
            }
            return Prepare(sb.ToString());
        }
    }
}