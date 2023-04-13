using System.Collections;
using System.Data.SQLite;
using Aveva.Core.PMLNet;

namespace PMLSQLite
{
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

        [PMLNetCallable()]
        public void Close()
        {
            dr.Close();
        }

        [PMLNetCallable()]
        public bool IsClosed()
        {
            return dr.IsClosed;
        }

        [PMLNetCallable()]
        public bool NextResult()
        {
            return dr.NextResult();
        }

        [PMLNetCallable()]
        public bool HasRows()
        {
            return dr.HasRows;
        }

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

        [PMLNetCallable()]
        public bool Read()
        {
            return dr.Read();
        }

        [PMLNetCallable()]
        public string Fetch(string name)
        {
            return dr[name].ToString();
        }

        [PMLNetCallable()]
        public Hashtable FetchRow()
        {
            var row = new Hashtable();
            for (int i = 0; i < dr.FieldCount; i++)
            {
                double key = i + 1;
                if (dr[i] is bool)
                {
                    row.Add(key, (bool)dr[i]);
                }
                else if (dr[i] is double)
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