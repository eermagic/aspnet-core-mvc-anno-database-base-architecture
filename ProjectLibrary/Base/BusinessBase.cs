using ProjectLibrary.DB;
using System.Collections;
using System.Dynamic;
using System.Text;

namespace ProjectLibrary.Base
{
    public class BusinessBase
    {
        #region 屬性
        public Hashtable UsedParamName = new Hashtable();
        public DBManager dbManager;
        public Hashtable htColumn = new Hashtable();
        public int TotalRowCount = 0;
        #endregion

        #region 建構子
        public BusinessBase(DBManager _dbManager)
        {
            dbManager = _dbManager;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 產生 SQL 條件語法
        /// </summary>
        /// <param name="sbWhere"></param>
        /// <param name="param"></param>
        /// <param name="column"></param>
        /// <param name="oper"></param>
        /// <param name="value"></param>
        public void GenWhere(StringBuilder sbWhere, dynamic param, string column, string oper, object value)
        {
            if (value != null && value.ToString() != "")
            {
                switch (oper.ToUpper().Trim())
                {
                    case "=":
                    case ">":
                    case "<":
                    case ">=":
                    case "<=":
                    case "<>":
                        sbWhere.Append(" AND " + column + " " + oper + " @" + CreateParamName(column) + " ");
                        AddProperty(param, CreateParamName(column), value);
                        break;
                    case "%LIKE%":
                        sbWhere.Append(" AND " + column + " " + oper + " @" + CreateParamName(column) + " ");
                        AddProperty(param, CreateParamName(column), "%" + value + "%");
                        break;
                }
            }
        }

        /// <summary>
        /// 產生 SQL 新增語法
        /// </summary>
        /// <param name="sbColumn"></param>
        /// <param name="sbValue"></param>
        /// <param name="param"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void GenInsert(StringBuilder sbColumn, StringBuilder sbValue, dynamic param, string column, object value)
        {
            if (value != null && value.ToString() != "")
            {
                if (sbColumn.Length > 0)
                    sbColumn.Append(", ");
                sbColumn.Append(column);

                if (sbValue.Length > 0)
                    sbValue.Append(", ");
                sbValue.Append("@" + CreateParamName(column));

                AddProperty(param, CreateParamName(column), value);
            }
        }

        /// <summary>
        /// 產生 SQL 修改語法
        /// </summary>
        /// <param name="sbColumn"></param>
        /// <param name="param"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void GenUpdate(StringBuilder sbColumn, dynamic param, string column, object value)
        {
            if (value != null && value.ToString() != "")
            {
                if (sbColumn.Length > 0)
                    sbColumn.Append(", ");
                sbColumn.Append(column + " = @" + CreateParamName(column));
                AddProperty(param, CreateParamName(column), value);
            }
        }

        /// <summary>
        /// 建立參數名稱
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string CreateParamName(string column)
        {
            if (UsedParamName.Contains(column) == false)
            {
                UsedParamName.Add(column, column + "_" + UsedParamName.Count);
            }

            return UsedParamName[column].ToString();
        }

        /// <summary>
        /// 附加物件屬性
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        /// <summary>
        /// 重設欄位
        /// </summary>
        public void ResetColumn()
        {
            htColumn.Clear();
        }
        #endregion

        #region 欄位
        public int PageNo
        {
            get { return Convert.ToInt32(htColumn["PageNo"]); }
            set { htColumn["PageNo"] = value; }
        }

        public int PageSize
        {
            get { return Convert.ToInt32(htColumn["PageSize"]); }
            set { htColumn["PageSize"] = value; }
        }
        #endregion
    }
}
