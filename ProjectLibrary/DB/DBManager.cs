using System.Data;
using System.Text;
using Dapper;

namespace ProjectLibrary.DB
{
    public class DBManager
    {
        #region 屬性
        string connStr = "";//資料庫連線字串
        List<DBConn> conns = new List<DBConn>(); //資料庫連線集合
        #endregion

        #region 建構子
        public DBManager(string _connStr)
        {
            connStr = _connStr;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 取得資料庫連線
        /// </summary>
        /// <param name="transName">交易名稱</param>
        /// <returns></returns>
        public DBConn GetConn(string transName)
        {
            DBConn? dbConn = conns.Where(x => x.transName == transName).FirstOrDefault();
            if (dbConn == null)
            {
                dbConn = new DBConn(transName, connStr);
                dbConn.conn.Open();
                if (transName != "")
                {
                    dbConn.trans = dbConn.conn.BeginTransaction(transName);
                }
                conns.Add(dbConn);
            }
            return dbConn;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable GetData(string sql, object param, int pageNo, int pageSize, ref int _TotalRowCount)
        {
            DBConn dbConn = GetConn("");

            // 還原可執行 SQL (Debug 與 Log 使用)
            string sqlLog = sql;
            var expandoDict = param as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> kv in expandoDict)
            {
                sqlLog = sqlLog.Replace("@" + kv.Key, "'" + kv.Value + "'");
            }

            IDataReader list;
            DataTable dt = new DataTable();
            if (pageNo > 0)
            {
                // 分頁處理

                // 取得總筆數
                string orderBy = "";
                string totalRowSql = sql;
                if (totalRowSql.ToUpper().IndexOf("ORDER BY") > -1)
                {
                    orderBy = totalRowSql.Substring(sql.ToUpper().LastIndexOf("ORDER BY"));
                    totalRowSql = totalRowSql.Replace(orderBy, "");
                }
                totalRowSql = "SELECT COUNT(*) AS CNT FROM (" + totalRowSql + ") CNT_TABLE";
                var rowCnt = dbConn.conn.Query(totalRowSql, param);
                foreach (var item in rowCnt)
                {
                    _TotalRowCount = item.CNT;
                }

                // 取得分頁 SQL
                int startRow = ((pageNo - 1) * pageSize) + 1;
                int endRow = (startRow + pageSize) - 1;
                orderBy = sql.Substring(sql.ToString().ToUpper().LastIndexOf("ORDER BY"));
                sql = sql.Replace(orderBy, "");
                // 去除 Order by 別名
                orderBy = orderBy.ToUpper().Replace("ORDER BY", "");
                StringBuilder newOrderBy = new StringBuilder();
                int index = 0;
                string[] orderBys = orderBy.Split(',');
                for (int i = 0; i < orderBys.Length; i++)
                {
                    if (newOrderBy.Length > 0) { newOrderBy.Append(","); }
                    string ob = orderBys[i];
                    index = ob.IndexOf('.');
                    if (index > -1)
                    {
                        newOrderBy.Append(ob.Substring(index + 1));
                    }
                    else
                    {
                        newOrderBy.Append(ob);
                    }
                }
                newOrderBy.Insert(0, "ORDER BY ");

                sql = string.Concat(
                    new object[] {
                            "SELECT * FROM (SELECT *, ROW_NUMBER() OVER (", newOrderBy.ToString(), ") AS RCOUNT FROM (", sql, ") PAGE_SQL ) PAGE_SQL2 WHERE PAGE_SQL2.RCOUNT BETWEEN "
                            , startRow, " AND ", endRow, " ", newOrderBy.ToString() });
                list = dbConn.conn.ExecuteReader(sql, param);
                dt.Load(list);
            }
            else
            {
                // 直接查詢 SQL
                list = dbConn.conn.ExecuteReader(sql, param);
                dt.Load(list);
                _TotalRowCount = dt.Rows.Count;
            }
            return dt;
        }

        /// <summary>
        /// 執行新增
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnSql"></param>
        /// <param name="valueSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Insert(string tableName, string columnSql, string valueSql, object param)
        {
            // 取得連線
            DBConn dbConn = GetConn("DefaultTrans");

            // 執行 SQL
            string sql = "INSERT INTO " + tableName + "(" + columnSql + ") VALUES (" + valueSql + ")";

            // 還原可執行 SQL (Debug 與 Log 使用)
            string sqlLog = sql;
            var expandoDict = param as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> kv in expandoDict)
            {
                sqlLog = sqlLog.Replace("@" + kv.Key, "'" + kv.Value + "'");
            }

            int ret = dbConn.conn.Execute(sql, param, dbConn.trans);
            return ret;
        }

        /// <summary>
        /// 執行修改
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Update(string tableName, string columnSql, string whereSql, object param)
        {
            // 取得連線
            DBConn dbConn = GetConn("DefaultTrans");

            // 執行 SQL
            string sql = "UPDATE " + tableName + " SET " + columnSql + " WHERE 1=1 " + whereSql;

            // 還原可執行 SQL (Debug 與 Log 使用)
            string sqlLog = sql;
            var expandoDict = param as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> kv in expandoDict)
            {
                sqlLog = sqlLog.Replace("@" + kv.Key, "'" + kv.Value + "'");
            }

            int ret = dbConn.conn.Execute(sql, param, dbConn.trans);
            return ret;
        }

        /// <summary>
        /// 執行刪除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int Delete(string tableName, string whereSql, object param)
        {
            // 取得連線
            DBConn dbConn = GetConn("DefaultTrans");

            // 執行 SQL
            string sql = "DELETE FROM " + tableName + " WHERE 1=1 " + whereSql;

            // 還原可執行 SQL (Debug 與 Log 使用)
            string sqlLog = sql;
            var expandoDict = param as IDictionary<string, object>;
            foreach (KeyValuePair<string, object> kv in expandoDict)
            {
                sqlLog = sqlLog.Replace("@" + kv.Key, "'" + kv.Value + "'");
            }

            int ret = dbConn.conn.Execute(sql, param, dbConn.trans);
            return ret;
        }

        /// <summary>
        /// 提交異動
        /// </summary>
        public void Commit()
        {
            foreach (DBConn dbConn in conns)
            {
                if (dbConn.trans != null)
                {
                    dbConn.trans.Commit();
                    dbConn.trans.Dispose();
                    dbConn.trans = null;
                }
            }

        }

        /// <summary>
        /// 回復異動
        /// </summary>
        public void Rollback()
        {
            foreach (DBConn dbConn in conns)
            {
                if (dbConn.trans != null)
                {
                    dbConn.trans.Rollback();
                    dbConn.trans.Dispose();
                    dbConn.trans = null;
                }
            }

        }

        /// <summary>
        /// 結束連線
        /// </summary>
        public void Close()
        {
            foreach (DBConn dbConn in conns)
            {
                if (dbConn.conn != null)
                {
                    dbConn.conn.Close();
                    dbConn.conn.Dispose();
                    dbConn.conn = null;
                }
            }
        }
        #endregion
    }
}
