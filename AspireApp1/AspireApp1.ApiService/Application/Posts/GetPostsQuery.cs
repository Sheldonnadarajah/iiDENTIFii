namespace AspireApp1.ApiService.Application.Posts
{
    public class GetPostsQuery
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? UserId { get; set; }
        #nullable enable annotations
        public List<string>? Tags { get; set; }
        public string? SortBy { get; set; }
    }
}
