using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.Text;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
    [PMLNetCallable()]
    public class PMLSQLite
    {
        private SQLiteConnection conn;

        [PMLNetCallable()]
        public PMLSQLite()
        {
            conn = new SQLiteConnection("");
        }

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

        [PMLNetCallable()]
        public string FileName()
        {
            return conn.FileName;
        }

        [PMLNetCallable()]
        public void SetDataSource(string filename)
        {
            var csb = new SQLiteConnectionStringBuilder(conn.ConnectionString);
            csb.DataSource = Environment.ExpandEnvironmentVariables(filename);
            conn.ConnectionString = csb.ToString();
        }

        [PMLNetCallable()]
        public void Open()
        {
            conn.Open();
        }

        [PMLNetCallable()]
        public void Close()
        {
            conn.Close();
        }

        [PMLNetCallable()]
        public bool IsOpen()
        {
            return (conn.State & ConnectionState.Open) == ConnectionState.Open;
        }

        [PMLNetCallable()]
        public string Version()
        {
            return conn.ServerVersion;
        }

        [PMLNetCallable()]
        public void Backup(PMLSQLite dest)
        {
            conn.BackupDatabase(dest.conn, dest.conn.Database, conn.Database, -1, null, -1);
        }

        [PMLNetCallable()]
        public bool AutoCommit()
        {
            return conn.AutoCommit;
        }

        [PMLNetCallable()]
        public double Changes()
        {
            return conn.Changes;
        }

        [PMLNetCallable()]
        public double LastInsertRowId()
        {
            return conn.LastInsertRowId;
        }

        [PMLNetCallable()]
        public PMLSQLiteStatement Prepare(string stmt)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = stmt;
            return new PMLSQLiteStatement(cmd);
        }

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