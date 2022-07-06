using Microsoft.AspNetCore.Mvc;
using static TeachAnnouncement.Models.AdmAnnoViewModel;
using System.Data;
using TeachAnnouncement.Business;

namespace TeachAnnouncement.Controllers
{
    public class AdmAnnoController : BaseController
    {
        #region 頁面載入動作
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region 查詢相關
        /// <summary>
        /// 查詢公告
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public IActionResult Query(QueryIn inModel)
        {
            QueryOut outModel = new QueryOut();
            outModel.Grid = new List<AnnoModel>();

            // 業務邏輯類別
            BusAnnouncement bus = new BusAnnouncement(dbManager);

            // 分頁參數
            bus.PageNo = inModel.pagination.pageNo;
            bus.PageSize = inModel.pagination.pageSize;

            // View 欄位
            bus.AnnoSubject = inModel.AnnoSubject;
            bus.AnnoStatus = inModel.AnnoStatus;

            // 查詢結果
            DataTable dt = bus.GetAnno();

            // 將 DataRow 資料綁定到 Model 裡面
            foreach (DataRow dr in dt.Rows)
            {
                AnnoModel gRow = new AnnoModel();
                gRow = (AnnoModel)BindModelByDataRow(dr, dt.Columns, gRow);
                outModel.Grid.Add(gRow);
            }

            // 計算分頁
            outModel.pagination = this.PreparePage(inModel.pagination, bus.TotalRowCount);

            return Json(outModel);
        }
        #endregion

        #region 新增相關
        /// <summary>
        /// 新增公告
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public IActionResult AddSave(AddSaveIn inModel)
        {
            AddSaveOut outModel = new AddSaveOut();

            // 檢查參數
            if (ModelState.IsValid == false)
            {
                outModel.ErrMsg = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(outModel);
            }

            // 業務邏輯類別
            BusAnnouncement bus = new BusAnnouncement(dbManager);

            // View 欄位
            bus.AnnoDate = inModel.AnnoDate;
            bus.AnnoSubject = inModel.AnnoSubject;
            bus.AnnoContent = inModel.AnnoContent;
            bus.AnnoStatus = inModel.AnnoStatus;

            // 執行新增
            bus.InsertAnno();

            outModel.ResultMsg = "新增完成";

            return Json(outModel);
        }
        #endregion

        #region 修改相關
        /// <summary>
        /// 修改公告
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public IActionResult EditSave(EditSaveIn inModel)
        {
            EditSaveOut outModel = new EditSaveOut();

            // 檢查參數
            if (ModelState.IsValid == false)
            {
                outModel.ErrMsg = string.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return Json(outModel);
            }

            // 業務邏輯類別
            BusAnnouncement bus = new BusAnnouncement(dbManager);

            // View 欄位
            bus.AnnoDate = inModel.AnnoDate;
            bus.AnnoSubject = inModel.AnnoSubject;
            bus.AnnoContent = inModel.AnnoContent;
            bus.AnnoStatus = inModel.AnnoStatus;
            bus.Pkey = inModel.Pkey;

            // 執行修改
            int cnt = bus.UpdateAnno();
            if (cnt > 0)
            {
                outModel.ResultMsg = "修改完成";
            }
            else
            {
                outModel.ErrMsg = "未異動資料";
            }
            return Json(outModel);
        }
        #endregion

        #region 刪除相關
        /// <summary>
        /// 刪除公告
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public IActionResult DelCheck(DelCheckIn inModel)
        {
            DelCheckOut outModel = new DelCheckOut();

            // 檢查參數
            if (inModel.checks.Count == 0)
            {
                outModel.ErrMsg = "缺少輸入資料";
                return Json(outModel);
            }

            // 業務邏輯類別
            BusAnnouncement bus = new BusAnnouncement(dbManager);

            int ret = 0;
            foreach (AnnoModel model in inModel.checks)
            {
                bus.Pkey = model.Pkey;
                // 執行刪除
                ret += bus.DeleteAnno();
            }

            if (ret > 0)
            {
                outModel.ResultMsg = "成功刪除 " + ret + " 筆資料";
            }
            return Json(outModel);
        }
        #endregion
    }
}
