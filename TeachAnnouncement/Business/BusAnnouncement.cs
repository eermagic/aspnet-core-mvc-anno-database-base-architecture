using ProjectLibrary.Base;
using ProjectLibrary.DB;
using System.Data;
using System.Dynamic;
using System.Text;

namespace TeachAnnouncement.Business
{
    public class BusAnnouncement : BusinessBase
    {
        #region 建構子
        public BusAnnouncement(DBManager _dbManager) : base(_dbManager)
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 取得公告
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnno()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT Pkey, CONVERT(varchar(12) , AnnoDate, 111 ) as AnnoDate, AnnoSubject, AnnoContent, AnnoStatus, Case AnnoStatus when '1' then '顯示' when '0' then '隱藏' end As AnnoStatusName ");
            sql.Append("FROM Announcement ");
            sql.Append("WHERE 1=1 ");

            StringBuilder sbWhere = new StringBuilder();
            dynamic param = new ExpandoObject();

            // 動態組合 SQL 條件
            GenWhere(sbWhere, param, "AnnoSubject", "%LIKE%", AnnoSubject);
            GenWhere(sbWhere, param, "AnnoStatus", "=", AnnoStatus);

            sql.Append(sbWhere);
            sql.Append("ORDER BY AnnoDate desc, AnnoStatus");

            // 執行查詢
            DataTable dt = dbManager.GetData(sql.ToString(), param, PageNo, PageSize, ref TotalRowCount);
            ResetColumn();
            return dt;
        }

        /// <summary>
        /// 新增公告
        /// </summary>
        /// <returns></returns>
        public int InsertAnno()
        {
            StringBuilder sbColumn = new StringBuilder();
            StringBuilder sbValue = new StringBuilder();
            dynamic param = new ExpandoObject();

            // 動態組合 SQL 欄位
            GenInsert(sbColumn, sbValue, param, "AnnoDate", AnnoDate);
            GenInsert(sbColumn, sbValue, param, "AnnoSubject", AnnoSubject);
            GenInsert(sbColumn, sbValue, param, "AnnoContent", AnnoContent);
            GenInsert(sbColumn, sbValue, param, "AnnoStatus", AnnoStatus);

            // 執行新增
            int cnt = dbManager.Insert("Announcement", sbColumn.ToString(), sbValue.ToString(), param);
            ResetColumn();
            return cnt;
        }

        /// <summary>
        /// 修改公告
        /// </summary>
        /// <returns></returns>
        public int UpdateAnno()
        {
            StringBuilder sbColumn = new StringBuilder();
            StringBuilder sbWhere = new StringBuilder();
            dynamic param = new ExpandoObject();

            // 動態組合 SQL 欄位
            GenUpdate(sbColumn, param, "AnnoDate", AnnoDate);
            GenUpdate(sbColumn, param, "AnnoSubject", AnnoSubject);
            GenUpdate(sbColumn, param, "AnnoContent", AnnoContent);
            GenUpdate(sbColumn, param, "AnnoStatus", AnnoStatus);

            // 動態組合 SQL 條件
            GenWhere(sbWhere, param, "Pkey", "=", this.Pkey);

            // 執行修改
            int cnt = dbManager.Update("Announcement", sbColumn.ToString(), sbWhere.ToString(), param);
            ResetColumn();
            return cnt;
        }

        /// <summary>
        /// 刪除公告
        /// </summary>
        /// <returns></returns>
        public int DeleteAnno()
        {
            StringBuilder sbWhere = new StringBuilder();
            dynamic param = new ExpandoObject();

            // 動態組合 SQL 條件
            GenWhere(sbWhere, param, "Pkey", "=", this.Pkey);

            // 執行修改
            int cnt = dbManager.Delete("Announcement", sbWhere.ToString(), param);
            ResetColumn();
            return cnt;
        }
        #endregion

        #region 欄位
        public object Pkey
        {
            get { return htColumn["Pkey"]; }
            set { htColumn["Pkey"] = value; }
        }

        public object AnnoSubject
        {
            get { return htColumn["AnnoSubject"]; }
            set { htColumn["AnnoSubject"] = value; }
        }

        public object AnnoStatus
        {
            get { return htColumn["AnnoStatus"]; }
            set { htColumn["AnnoStatus"] = value; }
        }

        public object AnnoDate
        {
            get { return htColumn["AnnoDate"]; }
            set { htColumn["AnnoDate"] = value; }
        }

        public object AnnoContent
        {
            get { return htColumn["AnnoContent"]; }
            set { htColumn["AnnoContent"] = value; }
        }

        #endregion
    }
}
