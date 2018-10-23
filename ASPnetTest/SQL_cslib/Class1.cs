using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using System.Data.SqlClient;
using System.Data;

namespace SQL_cslib
{
    public class SQL_cs_connector
    {
        //Here is the once-per-class call to initialize the log object
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private log4net.ILog log = null;
        private SqlConnection conn = null;
        private DataTable dt = null;

        public SQL_cs_connector()
        {
            //log4net.GlobalContext.Properties["testProperty"] = "This is my test property information";
        }

        public SQL_cs_connector(string ip, string db, string user, string pass)
        {
            string connStr = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",
                ip, db, user, pass);

            Connect(connStr);
        }

        ~SQL_cs_connector() { Disconnect(); }

        public void SetLogger(log4net.ILog log) { this.log = log; }

        public void Connect(string connStr)
        {
            try
            {
                if (conn == null)
                {
                    string msg = String.Format("Connecting to DB with {0}", connStr);
                    log.Info(msg);
                    conn = new SqlConnection(connStr);
                    conn.Open();
                }
            }
            catch (System.Exception ex)
            {
                string msg = String.Format("Error connecting DB");
                log.Error(msg);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                log.Info("Info error logging", ex);
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
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader dr = command.ExecuteReader();
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
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader dr = command.ExecuteReader();
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
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader dr = command.ExecuteReader();
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
                    SqlCommand command = new SqlCommand(sql, conn);
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
                    SqlCommand command = new SqlCommand(sql, conn);
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
