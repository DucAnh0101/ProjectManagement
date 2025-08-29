namespace DataAccessLayer.ResDTO
{
    public class ProjectGetResDTO
    {
        public int ProjectId { get; set; }

        public string ProjectCode { get; set; } = null!;

        public string ProjectName { get; set; } = null!;

        public int CustomerId { get; set; }

        public string ProjectType { get; set; } = null!;

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string? Objective { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public bool IsActive { get; set; }

        public decimal? Budget { get; set; }

        public decimal? ActualProgress { get; set; }
    }
}
