using System.Data.SQLite;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
    /// <summary>
    /// Represents a SQL statement to be executed.
    /// </summary>
    [PMLNetCallable()]
    public class PMLSQLiteStatement
    {
        private SQLiteCommand cmd;

        [PMLNetCallable()]
        public PMLSQLiteStatement()
        {
        }

        public PMLSQLiteStatement(SQLiteCommand c)
        {
            cmd = c;
        }

        [PMLNetCallable()]
        public void Assign(PMLSQLiteStatement that)
        {
            if (cmd != null) cmd.Dispose();
            cmd = that.cmd;
        }

        public override string ToString()
        {
            return cmd.CommandText;
        }

        /// <summary>
        /// Binds a parameter to the specified placeholder name.
        /// </summary>
        /// <param name="param">Placeholder name</param>
        /// <param name="value">Value to bind to the placeholder</param>
        /// <returns>This instance itself</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, string value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        /// <summary>
        /// Binds a value to the specified placeholder name.
        /// </summary>
        /// <param name="param">Placeholder name</param>
        /// <param name="value">Value to bind</param>
        /// <returns>This instance itself</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, double value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        /// <summary>
        /// Binds a value to the specified placeholder name.
        /// </summary>
        /// <param name="param">Placeholder name</param>
        /// <param name="value">Value to bind</param>
        /// <returns>This instance itself</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, bool value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        /// <summary>
        /// Executes the statement.
        /// </summary>
        [PMLNetCallable()]
        public void Execute()
        {
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes the statement and returns the first column of the first row.
        /// </summary>
        /// <returns>The first column of the first row</returns>
        [PMLNetCallable()]
        public string QueryOne()
        {
            return cmd.ExecuteScalar().ToString();
        }

        /// <summary>
        /// Executes the statement and returns a new <see cref="PMLSQLiteResult"/> instance.
        /// </summary>
        /// <returns>A new <see cref="PMLSQLiteResult"/> instance</returns>
        [PMLNetCallable()]
        public PMLSQLiteResult Query()
        {
            return new PMLSQLiteResult(cmd.ExecuteReader());
        }
    }
}