using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ReqDTO
{
    public class UpdateProjectReqDTO
    {
        [Required(ErrorMessage = "Mã dự án không được để trống.")]
        [StringLength(50, ErrorMessage = "Mã dự án không được vượt quá 50 ký tự.")]
        public string ProjectCode { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Tên dự án không được vượt quá 100 ký tự.")]
        public string ProjectName { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "ID khách hàng không hợp lệ.")]
        public int CustomerId { get; set; }

        [StringLength(50, ErrorMessage = "Loại dự án không được vượt quá 50 ký tự.")]
        public string ProjectType { get; set; } = null!;

        public DateOnly? EndDate { get; set; }

        [StringLength(500, ErrorMessage = "Mục tiêu không được vượt quá 500 ký tự.")]
        public string? Objective { get; set; }

        [StringLength(1000, ErrorMessage = "Mô tả không được vượt quá 1000 ký tự.")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự.")]
        public string Status { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Ngân sách không hợp lệ.")]
        public decimal? Budget { get; set; }

        [Range(0, 100, ErrorMessage = "Tiến độ thực tế phải từ 0 đến 100.")]
        public decimal? ActualProgress { get; set; }
    }
}