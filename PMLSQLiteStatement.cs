using System;
using System.Data.SQLite;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
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

        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, string value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, double value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        [PMLNetCallable()]
        public PMLSQLiteStatement Bind(string param, bool value)
        {
            cmd.Parameters.AddWithValue(param, value);
            return this;
        }

        [PMLNetCallable()]
        public void Execute()
        {
            cmd.ExecuteNonQuery();
        }

        [PMLNetCallable()]
        public string QueryOne()
        {
            return cmd.ExecuteScalar().ToString();
        }

        [PMLNetCallable()]
        public PMLSQLiteResult Query()
        {
            return new PMLSQLiteResult(cmd.ExecuteReader());
        }
    }
}