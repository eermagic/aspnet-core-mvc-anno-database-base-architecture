namespace TeachAnnouncement.Models
{
    /// <summary>
    /// ViewModel 共用
    /// </summary>
    public class BaseModel
    {
        public string? ErrMsg { get; set; }
    }

    /// <summary>
    // 分頁 Model
    /// </summary>
    public class PaginationModel
    {
        public List<string> pages { get; set; }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
        public int totalCount { get; set; }
    }
}
