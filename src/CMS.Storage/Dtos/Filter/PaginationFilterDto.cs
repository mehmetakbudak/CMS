namespace CMS.Storage.Dtos.Filter
{
    public class PaginationFilterDto
    {
        public int Skip { get; set; }
        public int PageSize { get; set; }       

        public PaginationFilterDto(int? skip, int? pageSize)
        {
            this.Skip = skip.HasValue ? skip.Value : 0;
            this.PageSize = pageSize.HasValue ? pageSize.Value : 5;
        }
    }
}
