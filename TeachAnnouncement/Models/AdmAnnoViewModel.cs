using System.ComponentModel.DataAnnotations;

namespace TeachAnnouncement.Models
{
    public class AdmAnnoViewModel
    {
        public class QueryIn : BaseModel
        {
            public string AnnoSubject { get; set; }
            public string AnnoStatus { get; set; }

            public PaginationModel pagination { get; set; }
        }

        public class QueryOut : BaseModel
        {
            public List<AnnoModel> Grid { get; set; }
            public PaginationModel pagination { get; set; }
        }

        public class AnnoModel : BaseModel
        {
            public string Pkey { get; set; }
            public string AnnoDate { get; set; }
            public string AnnoSubject { get; set; }
            public string AnnoContent { get; set; }
            public string AnnoStatus { get; set; }
            public string AnnoStatusName { get; set; }
        }

        public class AddSaveIn : BaseModel
        {
            [Required]
            public string AnnoDate { get; set; }
            [Required]
            public string AnnoSubject { get; set; }
            [Required]
            public string AnnoContent { get; set; }
            [Required]
            public string AnnoStatus { get; set; }
        }

        public class AddSaveOut : BaseModel
        {
            public string ResultMsg { get; set; }
        }

        public class EditSaveIn : BaseModel
        {
            [Required]
            public string Pkey { get; set; }
            [Required]
            public string AnnoDate { get; set; }
            [Required]
            public string AnnoSubject { get; set; }
            [Required]
            public string AnnoContent { get; set; }
            [Required]
            public string AnnoStatus { get; set; }
        }

        public class EditSaveOut : BaseModel
        {
            public string ResultMsg { get; set; }
        }

        public class DelCheckIn : BaseModel
        {
            public List<AnnoModel> checks { get; set; }
        }

        public class DelCheckOut : BaseModel
        {
            public string ResultMsg { get; set; }
        }
    }
}
