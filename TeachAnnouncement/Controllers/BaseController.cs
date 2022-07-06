using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectLibrary.DB;
using System.Data;
using System.Reflection;
using TeachAnnouncement.Models;

namespace TeachAnnouncement.Controllers
{
    public class BaseController : Controller
    {
        #region 屬性
        public DBManager dbManager;
        #endregion

        #region 方法
        /// <summary>
        /// Action 執行前動作
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

            // 資料庫連線字串
            string connStr = Config.GetConnectionString("SqlServer");
            dbManager = new DBManager(connStr);

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Action 執行後動作
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                // 當有錯誤時，執行回復資料庫
                dbManager.Rollback();
            }
            else
            {
                // 當正常執行時，則執行資料庫提交動作
                dbManager.Commit();
            }
            // 關閉資料庫連線
            dbManager.Close();

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 將 DataRow 資料綁定到 Model 裡面
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public BaseModel BindModelByDataRow(DataRow dr, DataColumnCollection column, BaseModel model)
        {
            Type typeM = model.GetType();
            PropertyInfo? propM;

            for (int i = 0; i < column.Count; i++)
            {
                string fieldName = column[i].ColumnName;

                propM = typeM.GetProperty(fieldName);

                if (propM != null)
                {
                    string value = "";
                    if (dr[i] != null || Convert.IsDBNull(dr[i]) == false)
                    {
                        value = dr[i].ToString();
                    }
                    propM.SetValue(model, value);
                }
            }

            return model;
        }

        /// <summary>
        /// 計算分頁
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TotalRowCount"></param>
        /// <returns></returns>
        public PaginationModel PreparePage(PaginationModel model, int TotalRowCount)
        {
            List<string> pages = new List<string>();
            int pageStart = ((model.pageNo - 1) / 10) * 10;
            model.totalCount = TotalRowCount;
            model.totalPage =
                    Convert.ToInt16(Math.Ceiling(
                     double.Parse(model.totalCount.ToString()) / double.Parse(model.pageSize.ToString())
                    ));

            if (model.pageNo > 10)
                pages.Add("<<");
            if (model.pageNo > 1)
                pages.Add("<");
            for (int i = 1; i <= 10; ++i)
            {
                if (pageStart + i > model.totalPage)
                    break;
                pages.Add((pageStart + i).ToString());
            }
            if (model.pageNo < model.totalPage)
                pages.Add(">");
            if ((pageStart + 10) < model.totalPage)
                pages.Add(">>");
            model.pages = pages;
            return model;
        }
        #endregion
    }
}
