using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syn2DB
{
    public sealed class Table
    {
        // Singleton Class
        private static Table instance = null;
        private static readonly object padlock = new object();
        private String[] tableName;

        #region Constructor
        public Table() { }

        public static Table Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Table();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public String[] List{
            get {
                FbConnection conn = null;
                try
                {
                conn = NewDatabase.Instance.Connection;
                conn.Open();
                String sqlCommand = "select rdb$relation_name"
                                    + " from rdb$relations "
                                    + "where rdb$view_blr is null "
                                    + "and(rdb$system_flag is null or rdb$system_flag = 0); ";
                FbCommand commandRead = new FbCommand(sqlCommand, conn);
                commandRead.CommandType = CommandType.Text;
                
               
                    
                    FbDataReader dtReader = commandRead.ExecuteReader();
                    int index = 0;
                    while (dtReader.Read())
                    {
                        if (dtReader.HasRows)
                        {
                            this.List[index] = dtReader[0].ToString();
                            index++;
                        }
                    }
                }
                catch(System.Data.SqlClient.SqlException ex)
                {

                }
                finally
                {
                    conn.Close();
                }
                
                return this.tableName;
            }
        }
        
    }
}
