using System.Data.SqlClient;

namespace ProjectLibrary.DB
{
    public class DBConn
    {
        #region 屬性
        public string transName = "";
        public SqlConnection? conn;
        public SqlTransaction? trans;
        #endregion

        #region 建構子
        public DBConn(string _transName, string connStr)
        {
            transName = _transName;
            conn = new SqlConnection(connStr);
        }
        #endregion
    }
}
