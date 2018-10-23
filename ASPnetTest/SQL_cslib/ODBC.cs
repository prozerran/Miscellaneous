using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Odbc;
using System.Data;

namespace SQL_cslib
{
    public class SQL_odbc_connector
    {
        private OdbcConnection conn = null;
        private DataTable dt = null;

        public SQL_odbc_connector(string dsn, string user, string pass)
        {
            Connect(dsn, user, pass);
        }

        ~SQL_odbc_connector() { Disconnect(); }

        public void Connect(string dsn, string user, string pass)
        {
            try
            {
                if (conn == null)
                {
                    conn = new OdbcConnection();
                    conn.ConnectionString = string.Format("Dsn={0};Uid={1};Pwd={2};",
                        dsn, user, pass);

                    conn.Open();
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void Disconnect()
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }

        public DataTable SelectTable(string tbname)
        {
            if (conn != null)
            {
                try
                {
                    string sql = string.Format("SELECT * FROM {0}", tbname);
                    OdbcCommand command = new OdbcCommand(sql, conn);
                    OdbcDataReader dr = command.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);

                    dr.Close();
                    command.Dispose();
                    return dt;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());                	
                }
            }
            return null;
        }

        public DataTable SelectTable(string tbname, int id)
        {
            if (conn != null)
            {
                try
                {
                    string sql = string.Format("SELECT * FROM {0} WHERE id = {1}", tbname, id);
                    OdbcCommand command = new OdbcCommand(sql, conn);
                    OdbcDataReader dr = command.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);

                    dr.Close();
                    command.Dispose();
                    return dt;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return null;
        }

        public DataTable CallStoreProc(string procname, int id)
        {
            if (conn != null)
            {
                try
                {
                    string sql = string.Format("EXEC {0} {1}", procname, id);
                    OdbcCommand command = new OdbcCommand(sql, conn);
                    OdbcDataReader dr = command.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);

                    dr.Close();
                    command.Dispose();
                    return dt;
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return null;
        }

        public void UpdateTable(string tbname, string cust, int id)
        {
            if (conn != null)
            {
                try
                {
                    string sql = string.Format("INSERT INTO {0} (Customer, id) Values ('{1}', {2})", tbname, cust, id);
                    OdbcCommand command = new OdbcCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        public void EmptyTable(string tbname)
        {
            if (conn != null)
            {
                try
                {
                    string sql = string.Format("TRUNCATE TABLE {0}", tbname);
                    OdbcCommand command = new OdbcCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

    }
}
