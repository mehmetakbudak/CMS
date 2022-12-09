namespace CMS.Storage.Model
{
    public class PaginationFilterModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilterModel()
        {
            this.PageNumber = 1;
            this.PageSize = 5;
        }
        public PaginationFilterModel(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 5 ? 5 : pageSize;
        }
    }
}
