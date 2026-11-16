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
        private SQLiteCommand _cmd;

        [PMLNetCallable()]
        public PMLSQLiteStatement()
        {
        }

        public PMLSQLiteStatement(SQLiteCommand c)
        {
            _cmd = c;
        }

        [PMLNetCallable()]
        public void Assign(PMLSQLiteStatement that)
        {
            _cmd?.Dispose();
            _cmd = that._cmd;
        }

        public override string ToString()
        {
            return _cmd.CommandText;
        }

        /// <summary>
        /// Binds a NULL value to the specified placeholder name.
        /// </summary>
        /// <param name="param">Placeholder name</param>
        /// <returns>This instance itself</returns>
        [PMLNetCallable()]
        public PMLSQLiteStatement BindNull(string param)
        {
            _cmd.Parameters.AddWithValue(param, null);
            return this;
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
            _cmd.Parameters.AddWithValue(param, value);
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
            _cmd.Parameters.AddWithValue(param, value);
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
            _cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        /// <summary>
        /// Executes the statement.
        /// </summary>
        [PMLNetCallable()]
        public void Execute()
        {
            _cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes the statement and returns the first column of the first row.
        /// </summary>
        /// <returns>The first column of the first row</returns>
        [PMLNetCallable()]
        public string QueryOne()
        {
            return _cmd.ExecuteScalar().ToString();
        }

        /// <summary>
        /// Executes the statement and returns a new <see cref="PMLSQLiteResult"/> instance.
        /// </summary>
        /// <returns>A new <see cref="PMLSQLiteResult"/> instance</returns>
        [PMLNetCallable()]
        public PMLSQLiteResult Query()
        {
            return new PMLSQLiteResult(_cmd.ExecuteReader());
        }
    }
}